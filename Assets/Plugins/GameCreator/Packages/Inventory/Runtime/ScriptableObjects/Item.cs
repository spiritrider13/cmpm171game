using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [CreateAssetMenu(
        fileName = "Item",
        menuName = "Game Creator/Inventory/Item"
    )]
    
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoItem.png")]
    [Serializable]
    public class Item : ScriptableObject, ISerializationCallbackReceiver
    {
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnEnterPlayMode]
        private static void InitializeOnEnterPlayMode()
        {
            EventInstantiate = null;
            LastItemInstanceInstantiated = null;
            LastItemInstantiated = null;
            LastItemDropped = null;
        }
        
        #endif
        
        public static GameObject LastItemInstanceInstantiated;
        public static RuntimeItem LastItemInstantiated;
        public static RuntimeItem LastItemDropped;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private UniqueID m_ID = new UniqueID();
        [SerializeField] private Item m_Parent;
        
        [SerializeField] private GameObject m_Prefab;
        [SerializeField] private bool m_CanDrop = true;

        [SerializeField] private Info m_Info = new Info();
        [SerializeField] private Shape m_Shape = new Shape();
        [SerializeField] private Price m_Price = new Price();

        [SerializeField] private Properties m_Properties = new Properties();
        [SerializeField] private Sockets m_Sockets = new Sockets();
        [SerializeField] private Equip m_Equip = new Equip();
        [SerializeField] private Usage m_Usage = new Usage();
        [SerializeField] private Crafting m_Crafting = new Crafting();

        // PROPERTIES: ----------------------------------------------------------------------------

        public IdString ID => this.m_ID.Get;
        public Item Parent => this.m_Parent;

        public bool HasPrefab => this.m_Prefab != null;
        public bool CanDrop => this.m_CanDrop;

        public Info Info => this.m_Info;
        public Shape Shape => this.m_Shape;
        public Price Price => this.m_Price;

        public Properties Properties => this.m_Properties;
        public Sockets Sockets => this.m_Sockets;
        public Equip Equip => this.m_Equip;
        public Usage Usage => this.m_Usage;
        public Crafting Crafting => this.m_Crafting;
        
        // EVENTS: --------------------------------------------------------------------------------

        public static event Action EventInstantiate;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool InheritsFrom(IdString itemID)
        {
            if (this.ID.Hash == itemID.Hash) return true;
            return this.m_Parent != null && this.Parent.InheritsFrom(itemID);
        }
        
        public RuntimeItem CreateRuntimeItem()
        {
            return new RuntimeItem(this);
        }
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static GameObject Drop(RuntimeItem runtimeItem, Vector3 position, Quaternion rotation)
        {
            if (runtimeItem?.Item == null) return null;
            return runtimeItem.Item.m_CanDrop 
                ? Instantiate(runtimeItem, position, rotation) 
                : null;
        }
        
        public static GameObject Instantiate(RuntimeItem runtimeItem, Vector3 position, Quaternion rotation)
        {
            if (runtimeItem?.Item == null) return null;
            if (runtimeItem.Item.m_Prefab == null) return null;
            
            GameObject instance = Instantiate(runtimeItem.Item.m_Prefab, position, rotation);
            if (instance == null) return null;
            
            LastItemInstanceInstantiated = instance;
            LastItemInstantiated = runtimeItem;
            EventInstantiate?.Invoke();
                
            return instance;
        }
        
        // SERIALIZATION CALLBACKS: ---------------------------------------------------------------

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (AssemblyUtils.IsReloading) return;
            
            this.m_Shape.OnBeforeSerialize(this);
            this.Properties.OnBeforeSerialize(this);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        { }
    }
}
