using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public abstract class TBagContent : IBagContent
    {
        public static readonly Vector2Int INVALID = new Vector2Int(-1, -1);
        
        // CLASSES: -------------------------------------------------------------------------------
        
        [Serializable]
        protected class Cells : TSerializableDictionary<IdString, Cell>
        { }
        
        [Serializable]
        protected class Stack : TSerializableDictionary<IdString, IdString>
        { }

        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventChange;
        
        public event Action<RuntimeItem> EventAdd;
        public event Action<RuntimeItem> EventRemove;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected Bag Bag { get; private set; }

        public abstract List<Cell> List { get; }

        public int CountWithStack
        {
            get
            {
                int count = 0;
                List<Cell> items = this.List;
                foreach (Cell cell in items)
                {
                    count += cell.Count;
                }

                return count;
            }
        }

        public int CountWithoutStack => this.List.Count;

        public virtual int CurrentWeight
        {
            get
            {
                int weight = 0;
                List<Cell> items = this.List;
                foreach (Cell cell in items)
                {
                    weight += cell.Weight;
                }
                
                return weight;
            }
        }

        // CHECKERS: ------------------------------------------------------------------------------
        
        public abstract bool IsAvailable(Vector2Int position, Vector2Int size);

        public abstract bool CanAdd(RuntimeItem runtimeItem, bool canStack);
        public abstract bool CanAddType(Item item, bool canStack);
        
        public abstract bool CanMove(Vector2Int positionA, Vector2Int positionB, bool canStack);

        public abstract bool Contains(IdString runtimeItemID);
        public abstract bool Contains(RuntimeItem runtimeItem);
        public abstract bool ContainsType(Item item, int amount);
        
        // GETTERS: -------------------------------------------------------------------------------

        public virtual RuntimeItem FindRuntimeItem(Item item)
        {
            foreach (Cell cell in this.List)
            {
                if (cell == null || cell.Available) continue;
                if (!cell.Item.InheritsFrom(item.ID)) continue;

                return cell.RootRuntimeItem;
            }

            return null;
        }
        
        public abstract RuntimeItem GetRuntimeItem(IdString runtimeItemID);
        public abstract Cell GetContent(Vector2Int position);
        
        public abstract Vector2Int FindPosition(IdString runtimeItemID);
        public abstract Vector2Int FindStartPosition(Vector2Int position);

        public virtual Cell[] GetTypes(Item item)
        {
            List<Cell> result = new List<Cell>();
            List<Cell> cells = this.List;
            
            foreach (Cell cell in cells)
            {
                if (cell == null || cell.Available) continue;
                if (!cell.Item.InheritsFrom(item.ID)) continue;

                result.Add(cell);
            }

            return result.ToArray();
        }
        
        public virtual int CountType(Item item)
        {
            int count = 0;
            List<Cell> cells = this.List;
            
            foreach (Cell cell in cells)
            {
                if (cell == null || cell.Available) continue;
                if (!cell.Item.InheritsFrom(item.ID)) continue;

                count += cell.Count;
            }

            return count;
        }

        // SETTERS: -------------------------------------------------------------------------------
        
        public abstract bool Move(Vector2Int positionA, Vector2Int positionB, bool canStack);
        public abstract bool Add(RuntimeItem runtimeItem, Vector2Int position, bool canStack);
        public abstract bool Add(RuntimeItem runtimeItem, bool canStack);
        
        public abstract RuntimeItem AddType(Item item, Vector2Int position, bool canStack);
        public abstract RuntimeItem AddType(Item item, bool canStack);
        
        public abstract RuntimeItem Remove(Vector2Int position);
        public abstract RuntimeItem Remove(RuntimeItem runtimeItem);
        public abstract RuntimeItem RemoveType(Item item);

        public abstract void Sort(Func<int, int, int> comparison);

        public virtual bool Use(RuntimeItem runtimeItem)
        {
            if (!this.Contains(runtimeItem)) return false;
            if (!runtimeItem.CanUse()) return false;

            _ = runtimeItem.Use();
            if (runtimeItem.Item.Usage.ConsumeWhenUse) this.Remove(runtimeItem);
            
            return true;
        }
        
        public virtual bool Use(Vector2Int position)
        {
            Cell cell = this.GetContent(position);
            if (cell == null || cell.Available) return false;

            RuntimeItem runtimeItem = cell.Peek();
            return this.Use(runtimeItem);
        }

        public GameObject Drop(RuntimeItem runtimeItem, Vector3 point)
        {
            if (runtimeItem == null) return null;
            if (!this.Contains(runtimeItem)) return null;
            
            if (this.Bag.Wearer == null) return null;
            if (!runtimeItem.Item.HasPrefab) return null;
            if (!runtimeItem.Item.CanDrop) return null;

            RuntimeItem removeRuntimeItem = this.Remove(runtimeItem);
            return Item.Drop(removeRuntimeItem, point, Quaternion.identity);
        }

        public GameObject[] Drop(Vector2Int position, int maxAmount, Vector3 point)
        {
            List<GameObject> instances = new List<GameObject>();
            
            Cell cell = this.GetContent(position);
            if (cell == null || cell.Available) return instances.ToArray();

            maxAmount = Math.Min(cell.Count, maxAmount);
            for (int i = 0; i < maxAmount; ++i)
            {
                RuntimeItem runtimeItem = cell.Peek();
                GameObject instance = this.Drop(runtimeItem, point); 
                if (instance != null) instances.Add(instance);
            }

            return instances.ToArray();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public virtual void OnAwake(Bag bag)
        {
            this.Bag = bag;
        }
        
        public virtual void OnLoad(TokenBagItems tokenBagItems)
        {
            for (int i = this.List.Count - 1; i >= 0; --i)
            {
                Cell cell = this.List[i];
                if (cell.Available) continue;

                for (int j = cell.List.Count - 1; j >= 0; --j)
                {
                    IdString runtimeItemID = cell.List[j];
                    RuntimeItem runtimeItem = this.GetRuntimeItem(runtimeItemID);
                    this.Remove(runtimeItem);
                }
            }

            foreach (RuntimeItem runtimeItem in tokenBagItems.Items)
            {
                this.Add(runtimeItem, true);
            }
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected void ExecuteEventChange()
        {
            this.EventChange?.Invoke();
        }

        protected void ExecuteEventAdd(RuntimeItem runtimeItem)
        {
            this.EventAdd?.Invoke(runtimeItem);
        }

        protected void ExecuteEventRemove(RuntimeItem runtimeItem)
        {
            this.EventRemove?.Invoke(runtimeItem);
        }
    }
}