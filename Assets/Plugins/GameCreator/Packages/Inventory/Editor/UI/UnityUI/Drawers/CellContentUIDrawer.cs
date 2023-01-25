using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory.UnityUI
{
    [CustomPropertyDrawer(typeof(CellContentUI))]
    public class CellContentUIDrawer : RuntimeItemUIDrawer
    {
        protected override void AddBefore(VisualElement root, SerializedProperty property)
        {
            SerializedProperty activeCorner = property.FindPropertyRelative("m_ActiveCorner");
            root.Add(new GameCreator.Editor.Common.PropertyTool(activeCorner));

            base.AddBefore(root, property);
        }

        protected override void AddAfter(VisualElement root, SerializedProperty property)
        {
            base.AddAfter(root, property);
            
            SerializedProperty displayStack = property.FindPropertyRelative("m_DisplayStack");
            SerializedProperty stackContent = property.FindPropertyRelative("m_StackContent");
            SerializedProperty stackCount = property.FindPropertyRelative("m_StackCount");
            
            root.Add(new SpaceSmall());
            root.Add(new GameCreator.Editor.Common.PropertyTool(displayStack));
            root.Add(new GameCreator.Editor.Common.PropertyTool(stackContent));
            root.Add(new GameCreator.Editor.Common.PropertyTool(stackCount));
        }
    }
}
