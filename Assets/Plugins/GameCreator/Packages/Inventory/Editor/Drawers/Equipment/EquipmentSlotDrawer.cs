using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(EquipmentSlot))]
    public class EquipmentSlotDrawer : PropertyDrawer
    {
        public const string PROP_BASE = "m_Base";
        public const string PROP_BONE = "m_Bone";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty propertyBase = property.FindPropertyRelative(PROP_BASE);
            SerializedProperty propertyBone = property.FindPropertyRelative(PROP_BONE);
            
            var fieldBase = new GameCreator.Editor.Common.PropertyTool(propertyBase);
            var fieldBone = new GameCreator.Editor.Common.PropertyTool(propertyBone);
            
            root.Add(fieldBase);
            root.Add(fieldBone);
            
            return root;
        }
    }
}