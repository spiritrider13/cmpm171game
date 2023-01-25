using System;
using GameCreator.Runtime.Inventory;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class TokenBag : Token
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private TokenBagShape m_Shape;
        [SerializeField] private TokenBagItems m_Items;
        [SerializeField] private TokenBagWealth m_Wealth;
        [SerializeField] private TokenBagEquipment m_Equipment;

        // PROPERTIES: ----------------------------------------------------------------------------

        public TokenBagShape Shape => this.m_Shape;
        public TokenBagItems Items => this.m_Items;
        public TokenBagWealth Wealth => this.m_Wealth;
        public TokenBagEquipment Equipment => this.m_Equipment;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public TokenBag(Bag target, bool shape, bool items, bool wealth, bool equipment) : base()
        {
            this.m_Shape = new TokenBagShape(shape ? target.Shape : null);
            this.m_Items = new TokenBagItems(items ? target.Content : null);
            this.m_Wealth = new TokenBagWealth(wealth ? target.Wealth : null);
            this.m_Equipment = new TokenBagEquipment(equipment ? target.Equipment : null);
        }
    }
}