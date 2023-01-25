using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("On Remove")]
    [Category("Inventory/On Remove")]
    [Description("Executes after removing an item from the specified Bag")]
    
    [Keywords("Bag", "Inventory", "Item", "Take")]
    [Image(typeof(IconBagSolid), ColorTheme.Type.Red, typeof(OverlayMinus))]

    [Serializable]
    public class EventInventoryOnRemoveFomBag : VisualScripting.Event
    {
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        private Bag Bag { get; set; }
        private Args Args { get; set; }
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            
            this.Bag = this.m_Bag.Get<Bag>(trigger);
            if (this.Bag == null) return;

            this.Args = new Args(this.Self, this.Bag.gameObject);
            
            this.Bag.Content.EventRemove -= this.OnRemove;
            this.Bag.Content.EventRemove += this.OnRemove;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            if (this.Bag == null) return;
            this.Bag.Content.EventRemove -= this.OnRemove;
        }

        private void OnRemove(RuntimeItem runtimeItem)
        {
            _ = this.m_Trigger.Execute(this.Args);
        }
    }
}