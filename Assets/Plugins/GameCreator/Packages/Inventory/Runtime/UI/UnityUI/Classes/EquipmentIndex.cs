using System;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Serializable]
    public class EquipmentIndex
    {
        [SerializeField] private Equipment m_Equipment;
        [SerializeField] private int m_Index;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public int Index => this.m_Index;
    }
}