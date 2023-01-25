using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class BagContentList : TBagContent
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Cells m_Cells = new Cells();
        [SerializeField] private Stack m_Stack = new Stack();
        [SerializeField] private List<IdString> m_Items = new List<IdString>();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override List<Cell> List => new List<Cell>(this.m_Cells.Values);
        
        // INITIALIZERS: --------------------------------------------------------------------------

        public override void OnAwake(Bag bag)
        {
            base.OnAwake(bag);
        }

        // CHECKERS: ------------------------------------------------------------------------------

        public override bool IsAvailable(Vector2Int position, Vector2Int size)
        {
            return position.y >= 0 && position.y < this.Bag.Shape.MaxHeight;
        }

        public override bool CanAdd(RuntimeItem runtimeItem, bool canStack)
        {
            if (runtimeItem == null) return false;
            if (this.m_Cells.ContainsKey(runtimeItem.RuntimeID)) return false;
            if (this.m_Stack.ContainsKey(runtimeItem.RuntimeID)) return false;

            foreach (KeyValuePair<IdString, Cell> entry in this.m_Cells)
            {
                if (canStack && entry.Value.CanStack(runtimeItem)) return true;
            }

            return this.m_Items.Count + 1 <= this.Bag.Shape.MaxHeight;
        }

        public override bool CanAddType(Item item, bool canStack)
        {
            if (item == null) return false;
            
            RuntimeItem tempRuntimeItem = item.CreateRuntimeItem();
            return this.CanAdd(tempRuntimeItem, canStack);
        }

        public override bool CanMove(Vector2Int positionA, Vector2Int positionB, bool canStack)
        {
            if (positionA.y < 0 || positionA.y >= this.m_Items.Count) return false;
            if (positionB.y < 0 || positionB.y >= this.m_Items.Count) return false;
            
            Cell cellA = this.GetContent(positionA);
            if (cellA == null || cellA.Available) return false;

            Cell cellB = this.GetContent(positionB);
            if (cellB == null || cellB.Available) return false;

            return true;
        }

        public override bool Contains(IdString runtimeItemID)
        {
            runtimeItemID = this.FindStackRoot(runtimeItemID);
            return this.m_Cells.ContainsKey(runtimeItemID);
        }

        public override bool Contains(RuntimeItem runtimeItem)
        {
            return runtimeItem != null && this.Contains(runtimeItem.RuntimeID);
        }

        public override bool ContainsType(Item item, int amount)
        {
            int count = 0;
            foreach (KeyValuePair<IdString, Cell> entry in this.m_Cells)
            {
                if (entry.Value.Available) continue;
                if (entry.Value.ContainsType(item.ID))
                {
                    count += entry.Value.Count;
                    if (count >= amount) return true;
                }
            }

            return false;
        }

        // GETTERS: -------------------------------------------------------------------------------

        public override RuntimeItem GetRuntimeItem(IdString runtimeItemID)
        {
            if (this.m_Stack.TryGetValue(runtimeItemID, out IdString rootRuntimeItemID))
            {
                return this.m_Cells[rootRuntimeItemID].Get(runtimeItemID);
            }

            return this.m_Cells.TryGetValue(runtimeItemID, out Cell cell)
                ? cell.Get(runtimeItemID)
                : null;
        }

        public override Cell GetContent(Vector2Int position)
        {
            if (position.y < 0 || position.y >= this.m_Items.Count) return null;
            return this.m_Cells.TryGetValue(this.m_Items[position.y], out Cell cell)
                ? cell 
                : null;
        }

        public override Vector2Int FindPosition(IdString runtimeItemID)
        {
            runtimeItemID = this.FindStackRoot(runtimeItemID);

            int itemsCount = this.m_Items.Count;
            for (int i = 0; i < itemsCount; ++i)
            {
                if (this.m_Items[i].Hash == runtimeItemID.Hash) return new Vector2Int(0, i);
            }

            return INVALID;
        }

        public override Vector2Int FindStartPosition(Vector2Int position)
        {
            return position;
        }

        // SETTERS: -------------------------------------------------------------------------------

        public override bool Move(Vector2Int positionA, Vector2Int positionB, bool canStack)
        {
            if (!this.CanMove(positionA, positionB, canStack)) return false;

            Cell cellA = this.GetContent(positionA);
            Cell cellB = this.GetContent(positionB);
            
            if (canStack && cellB != null && cellB.CanStack(cellA.RootRuntimeItem, cellA.Count))
            {
                while (cellA.Count > 0)
                {
                    RuntimeItem removeRuntimeItem = this.Remove(positionA);
                    this.Add(removeRuntimeItem, positionB, true);
                }

                this.ExecuteEventChange();
                return true;
            }

            IdString sourceRuntimeItemID = this.m_Items[positionA.y];
            this.m_Items.RemoveAt(positionA.y);
            this.m_Items.Insert(positionB.y, sourceRuntimeItemID);
            
            this.ExecuteEventChange();
            return true;
        }

        public override bool Add(RuntimeItem runtimeItem, Vector2Int position, bool canStack)
        {
            if (runtimeItem == null) return false;
            if (this.Contains(runtimeItem)) return false;

            IdString rootRuntimeItemID = position.y >= 0 && position.y < this.m_Items.Count 
                ? this.m_Items[position.y] 
                : IdString.EMPTY;
            
            if (this.m_Cells.TryGetValue(rootRuntimeItemID, out Cell cell))
            {
                if (cell.Add(runtimeItem, canStack))
                {
                    this.m_Stack[runtimeItem.RuntimeID] = rootRuntimeItemID;
                    
                    runtimeItem.Bag = this.Bag;
                    RuntimeItem.Bag_LastItemAdded = runtimeItem;
            
                    this.ExecuteEventAdd(runtimeItem);
                    this.ExecuteEventChange();
                    
                    return true;
                }
            }

            if (this.m_Items.Count + 1 > this.Bag.Shape.MaxHeight) return false;
            
            IdString runtimeItemID = runtimeItem.RuntimeID;
            this.m_Cells.Add(runtimeItemID, new Cell(runtimeItem));
            this.m_Items.Add(runtimeItemID);

            runtimeItem.Bag = this.Bag;
            RuntimeItem.Bag_LastItemAdded = runtimeItem;
            
            this.ExecuteEventAdd(runtimeItem);
            this.ExecuteEventChange();

            return true;
        }

        public override bool Add(RuntimeItem runtimeItem, bool canStack)
        {
            for (int j = 0; j < this.m_Items.Count; ++j)
            {
                Vector2Int stackPosition = new Vector2Int(0, j);
                IdString parentRuntimeItemID = this.m_Items[j];
                
                if (this.m_Cells.TryGetValue(parentRuntimeItemID, out Cell cell))
                {
                    if (canStack && cell.CanStack(runtimeItem))
                    {
                        return this.Add(runtimeItem, stackPosition, true);
                    }
                }
            }

            Vector2Int position = new Vector2Int(0, this.m_Items.Count);
            return this.Add(runtimeItem, position, canStack);
        }

        public override RuntimeItem AddType(Item item, Vector2Int position, bool canStack)
        {
            if (item == null) return null;

            RuntimeItem runtimeItem = item.CreateRuntimeItem();
            return this.Add(runtimeItem, position, canStack)
                ? runtimeItem
                : null;
        }

        public override RuntimeItem AddType(Item item, bool canStack)
        {
            if (item == null) return null;
            
            RuntimeItem runtimeItem = item.CreateRuntimeItem();
            return this.Add(runtimeItem, canStack)
                ? runtimeItem
                : null;
        }

        public override RuntimeItem Remove(Vector2Int position)
        {
            return this.Remove(position, null);
        }

        public override RuntimeItem Remove(RuntimeItem runtimeItem)
        {
            if (runtimeItem == null) return null;
            
            Vector2Int position = this.FindPosition(runtimeItem.RuntimeID);
            return position != INVALID ? this.Remove(position, runtimeItem) : null;
        }

        public override RuntimeItem RemoveType(Item item)
        {
            if (item == null) return null;

            foreach (KeyValuePair<IdString, Cell> entry in this.m_Cells)
            {
                if (entry.Value.Available) continue;
                if (entry.Value.ContainsType(item.ID))
                {
                    Vector2Int position = this.FindPosition(entry.Value.RootRuntimeItemID);
                    if (position != INVALID) return this.Remove(position);
                }
            }

            return null;
        }

        public override void Sort(Func<int, int, int> comparison)
        {
            throw new NotImplementedException();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private IdString FindStackRoot(IdString runtimeItemID)
        {
            return this.m_Stack.TryGetValue(runtimeItemID, out IdString sourceRuntimeItemID) 
                ? sourceRuntimeItemID 
                : runtimeItemID;
        }

        private RuntimeItem Remove(Vector2Int position, RuntimeItem runtimeItem)
        {
            Cell cell = this.GetContent(position);
            if (cell == null || cell.Available) return null;

            runtimeItem ??= cell.Peek();
            
            this.Bag.Equipment.Unequip(runtimeItem);
            runtimeItem = cell.Remove(runtimeItem.RuntimeID);

            if (runtimeItem == null) return null;
            this.m_Stack.Remove(runtimeItem.RuntimeID);

            if (cell.Count == 0)
            {
                this.m_Cells.Remove(runtimeItem.RuntimeID);
                this.m_Items.RemoveAt(position.y);
            }
            
            RuntimeItem.Bag_LastItemRemoved = runtimeItem;
            runtimeItem.Bag = null;
            
            this.ExecuteEventRemove(runtimeItem);
            this.ExecuteEventChange();

            return runtimeItem;
        }
    }
}