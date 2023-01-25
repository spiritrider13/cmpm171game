using GameCreator.Runtime.Common;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameCreator.Runtime.Inventory
{
    public class InventorySettings : AssetRepository<InventoryRepository>
    {
        public override IIcon Icon => new IconItem(ColorTheme.Type.TextLight);
        public override string Name => "Inventory";
        
        #if UNITY_EDITOR

        private void OnEnable()
        {
            string[] itemsGuids = AssetDatabase.FindAssets($"t:{nameof(Item)}");
            Item[] items = new Item[itemsGuids.Length];

            for (int i = 0; i < itemsGuids.Length; i++)
            {
                string itemsGuid = itemsGuids[i];
                string itemPath = AssetDatabase.GUIDToAssetPath(itemsGuid);
                items[i] = AssetDatabase.LoadAssetAtPath<Item>(itemPath);
            }

            this.Get().Items.Set(items);
        }

        #endif
    }
}