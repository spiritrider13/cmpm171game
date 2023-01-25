using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public struct TokenBagItems
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private RuntimeItem[] m_Items;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public RuntimeItem[] Items
        {
            get => this.m_Items;
            internal set => this.m_Items = value;
        }
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public TokenBagItems(IBagContent bagContent)
        {
            if (bagContent == null)
            {
                this.m_Items = Array.Empty<RuntimeItem>();
                return;
            }

            List<RuntimeItem> runtimeItems = new List<RuntimeItem>();
            
            List<Cell> content = bagContent.List;
            foreach (Cell cell in content)
            {
                if (cell.Available) continue;

                List<IdString> stack = cell.List;
                foreach (IdString runtimeItemID in stack)
                {
                    RuntimeItem runtimeItem = cell.Get(runtimeItemID);
                    
                    RuntimeItem runtimeItemClone = CloneUtils.Deep(runtimeItem);
                    runtimeItems.Add(runtimeItemClone);
                }
            }

            this.m_Items = runtimeItems.ToArray();
        }
    }
}