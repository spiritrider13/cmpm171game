using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("On Add")]
    [Category("Inventory/On Add")]
    [Description("Executes after adding an item to the specified Bag")]
    
    [Keywords("Bag", "Inventory", "Item", "Add")]
    [Image(typeof(IconBagSolid), ColorTheme.Type.Yellow, typeof(OverlayPlus))]

    [Serializable]
    public class EventInventoryOnAddToBag : VisualScripting.Event
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
            
            this.Bag.Content.EventAdd -= this.OnAdd;
            this.Bag.Content.EventAdd += this.OnAdd;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            if (this.Bag == null) return;
            this.Bag.Content.EventAdd -= this.OnAdd;
        }

        private void OnAdd(RuntimeItem runtimeItem)
        {
            _ = this.m_Trigger.Execute(this.Args);
        }
    }
}