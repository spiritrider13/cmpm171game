using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    public class SpatialHash
    {
        private const int MAX_SIZE = 256; // Must be POT
        private const int MIN_SIZE = 4;   // Must be POT
        
        private const float MAX_RADIUS = 500f;

        ///////////////////////////////////////////////////////////////////////////////////////////
        // SPATIAL TREE: --------------------------------------------------------------------------

        private abstract class SpatialTree
        {
            [NonSerialized] protected readonly int m_Size;
            [NonSerialized] private readonly Dictionary<int, Vector3Int> m_HashCache;

            protected SpatialTree(int size)
            {
                this.m_Size = size;
                this.m_HashCache = new Dictionary<int, Vector3Int>();
            }

            protected Vector3Int Hash(ISpatialHash element)
            {
                int uniqueCode = element.UniqueCode;
                if (!this.m_HashCache.TryGetValue(uniqueCode, out Vector3Int hash))
                {
                    hash = this.Hash(element.Position);
                    this.m_HashCache.Add(uniqueCode, hash);
                }

                return hash;
            }

            protected Vector3Int Hash(Vector3 point)
            {
                return new Vector3Int(
                    Mathf.FloorToInt(point.x / this.m_Size),
                    Mathf.FloorToInt(point.y / this.m_Size),
                    Mathf.FloorToInt(point.z / this.m_Size)
                );
            }

            protected SpatialTree CreateSubTree(List<ISpatialHash> set, int size)
            {
                int nextSize = size >> 1;
                return nextSize > MIN_SIZE
                    ? new SpatialTreeNode(set, nextSize)
                    : new SpatialTreeLeaf(set, size);
            }

            public void UpdateHashCache(ISpatialHash element)
            {
                int uniqueCode = element.UniqueCode;
                this.m_HashCache[uniqueCode] = this.Hash(element.Position);
            }

            public abstract void Insert(Node node);
            public abstract bool Remove(Node node);
            public abstract bool Update(Node node);

            public abstract bool Remove(int uniqueCode, Vector3 point);
            public abstract HashSet<int> Query(Vector3[] bounds);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // SPATIAL TREE NODE: ---------------------------------------------------------------------

        private class SpatialTreeNode : SpatialTree
        {
            [NonSerialized] private readonly Dictionary<Vector3Int, SpatialTree> m_Tree;

            public SpatialTreeNode(List<ISpatialHash> set, int size) : base(size)
            {
                this.m_Tree = new Dictionary<Vector3Int, SpatialTree>();
                var group = new Dictionary<Vector3Int, List<ISpatialHash>>();

                int setCount = set.Count;
                for (int i = 0; i < setCount; ++i)
                {
                    Vector3Int hash = this.Hash(set[i]);

                    if (!group.TryGetValue(hash, out List<ISpatialHash> list))
                    {
                        list = new List<ISpatialHash>();
                        group.Add(hash, list);
                    }

                    list.Add(set[i]);
                }

                foreach (KeyValuePair<Vector3Int, List<ISpatialHash>> cluster in group)
                {
                    this.m_Tree.Add(
                        cluster.Key,
                        this.CreateSubTree(cluster.Value, this.m_Size)
                    );
                }
            }

            public override void Insert(Node node)
            {
                Vector3Int hash = this.Hash(node.Element);
                if (this.m_Tree.TryGetValue(hash, out SpatialTree subTree))
                {
                    subTree.Insert(node);
                }
                else
                {
                    List<ISpatialHash> set = new List<ISpatialHash> { node.Element };
                    this.m_Tree.Add(hash, this.CreateSubTree(set, this.m_Size));
                }
            }

            public override bool Remove(Node node)
            {
                Vector3Int hash = this.Hash(node.Element);
                if (this.m_Tree.TryGetValue(hash, out SpatialTree subTree))
                {
                    bool isEmpty = subTree.Remove(node);
                    if (isEmpty) this.m_Tree.Remove(hash);
                }

                return this.m_Tree.Count == 0;
            }

            public override bool Remove(int uniqueCode, Vector3 point)
            {
                Vector3Int hash = this.Hash(point);
                if (this.m_Tree.TryGetValue(hash, out SpatialTree subTree))
                {
                    bool isEmpty = subTree.Remove(uniqueCode, point);
                    if (isEmpty) this.m_Tree.Remove(hash);
                }

                return this.m_Tree.Count == 0;
            }

            public override bool Update(Node node)
            {
                this.UpdateHashCache(node.Element);

                Vector3Int prevHash = this.Hash(node.Position);
                Vector3Int nextHash = this.Hash(node.Element);

                if (prevHash == nextHash)
                {
                    if (this.m_Tree.TryGetValue(nextHash, out SpatialTree subTree))
                    {
                        return subTree.Update(node);
                    }

                    return false;
                }

                this.Insert(node);
                this.Remove(node.Element.UniqueCode, node.Position);
                return true;
            }

            public override HashSet<int> Query(Vector3[] bounds)
            {
                Vector3Int cellA = this.Hash(bounds[0]); // (-1,  1,  1)
                Vector3Int cellB = this.Hash(bounds[1]); // ( 1,  1,  1)
                Vector3Int cellD = this.Hash(bounds[2]); // (-1, -1,  1)
                Vector3Int cellG = this.Hash(bounds[6]); // (-1,  1, -1)

                HashSet<int> matches = new HashSet<int>();
                for (int x = cellA.x; x <= cellB.x; ++x)
                {
                    for (int y = cellD.y; y <= cellA.y; ++y)
                    {
                        for (int z = cellD.z; z <= cellG.z; ++z)
                        {
                            Vector3Int hash = new Vector3Int(x, y, z);
                            if (this.m_Tree.TryGetValue(hash, out SpatialTree subTree))
                            {
                                matches.UnionWith(subTree.Query(bounds));
                            }
                        }
                    }
                }

                return matches;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // SPATIAL TREE LEAF: ---------------------------------------------------------------------

        private class SpatialTreeLeaf : SpatialTree
        {
            [NonSerialized] private readonly HashSet<int> m_Data;

            public SpatialTreeLeaf(List<ISpatialHash> set, int size) : base(size)
            {
                this.m_Data = new HashSet<int>();
                int setCount = set.Count;

                for (int i = 0; i < setCount; ++i)
                {
                    this.m_Data.Add(set[i].UniqueCode);
                }
            }

            public override void Insert(Node node)
            {
                this.m_Data.Add(node.Element.UniqueCode);
            }

            public override bool Remove(Node node)
            {
                this.m_Data.Remove(node.Element.UniqueCode);
                return this.m_Data.Count == 0;
            }

            public override bool Remove(int uniqueCode, Vector3 point)
            {
                this.m_Data.Remove(uniqueCode);
                return this.m_Data.Count == 0;
            }

            public override bool Update(Node node)
            {
                return false;
            }

            public override HashSet<int> Query(Vector3[] bounds)
            {
                return this.m_Data;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // NODE: ----------------------------------------------------------------------------------

        private class Node
        {
            public ISpatialHash Element { get; }
            public Vector3 Position { get; set; }

            public Node(ISpatialHash element)
            {
                this.Element = element;
                this.Position = element.Position;
            }
        }

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly SpatialTree m_Tree;
        [NonSerialized] private readonly Dictionary<int, Node> m_Nodes;

        [NonSerialized] private int m_UpdatedFrame = -1;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public SpatialHash() : this(new List<ISpatialHash>())
        { }

        public SpatialHash(List<ISpatialHash> set)
        {
            this.m_Tree = new SpatialTreeNode(set, MAX_SIZE);

            this.m_Nodes = new Dictionary<int, Node>();
            int setCount = set.Count;
            for (int i = 0; i < setCount; ++i)
            {
                this.m_Nodes.Add(set[i].UniqueCode, new Node(set[i]));
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Insert(ISpatialHash element)
        {
            if (this.m_Nodes.ContainsKey(element.UniqueCode)) return;
            Node node = new Node(element);

            this.m_Nodes.Add(element.UniqueCode, node);
            this.m_Tree.Insert(node);
        }

        public void Remove(ISpatialHash element)
        {
            if (!this.m_Nodes.ContainsKey(element.UniqueCode)) return;

            this.m_Tree.Remove(this.m_Nodes[element.UniqueCode]);
            this.m_Nodes.Remove(element.UniqueCode);
        }

        public bool ForceUpdate(ISpatialHash element)
        {
            if (!this.m_Nodes.TryGetValue(element.UniqueCode, out Node node)) return false;
            if (node.Position == element.Position) return false;
                
            bool isUpdate = this.m_Tree.Update(node);
            node.Position = element.Position;
        
            return isUpdate;
        }

        public bool Contains(ISpatialHash element)
        {
            return this.m_Nodes.ContainsKey(element.UniqueCode);
        }

        public List<ISpatialHash> Query(Vector3 point, float radius)
        {
            radius = Math.Min(radius, MAX_RADIUS);
            this.RequestRefresh();
            
            Vector3[] bounds =
            {
                new Vector3(point.x - radius, point.y + radius, point.z - radius),
                new Vector3(point.x + radius, point.y + radius, point.z - radius),
                new Vector3(point.x - radius, point.y - radius, point.z - radius),
                new Vector3(point.x + radius, point.y - radius, point.z - radius),
                new Vector3(point.x - radius, point.y + radius, point.z + radius),
                new Vector3(point.x + radius, point.y + radius, point.z + radius),
                new Vector3(point.x - radius, point.y - radius, point.z + radius),
                new Vector3(point.x + radius, point.y - radius, point.z + radius),
            };

            HashSet<int> matches = this.m_Tree.Query(bounds);
            List<ISpatialHash> elements = new List<ISpatialHash>();

            foreach(int match in matches)
            {
                if (!this.m_Nodes.TryGetValue(match, out Node node)) continue;
                float distance = Vector3.Distance(node.Position, point);
                
                if (!(distance <= radius)) continue;
                elements.Add(node.Element);
            }

            return elements;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RequestRefresh()
        {
            if (this.m_UpdatedFrame == Time.frameCount) return;
            this.m_UpdatedFrame = Time.frameCount;
            
            foreach (KeyValuePair<int, Node> entry in this.m_Nodes)
            {
                if (entry.Value.Element == null) continue;
                if (entry.Value.Position == entry.Value.Element.Position) continue;
                
                this.m_Tree.Update(entry.Value);
                entry.Value.Position = entry.Value.Element.Position;
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    // INTERFACE: ---------------------------------------------------------------------------------

    public interface ISpatialHash
    {
        Vector3 Position { get; }
        int UniqueCode { get; }
    }
}