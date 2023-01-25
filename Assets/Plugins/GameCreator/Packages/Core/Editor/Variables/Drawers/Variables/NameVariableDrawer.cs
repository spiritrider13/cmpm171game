using GameCreator.Editor.Common;
using GameCreator.Runtime.Variables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Variables
{
    [CustomPropertyDrawer(typeof(NameVariable))]
    public class NameVariableDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty propertyName = property.FindPropertyRelative("m_Name");
            SerializedProperty propertyValue = property.FindPropertyRelative("m_Value");

            PropertyTool fieldName = new PropertyTool(propertyName);
            PropertyElement fieldValue = new PropertyElement(propertyValue, "Value", true);

            root.Add(fieldName);
            root.Add(fieldValue);

            return root;
        }
    }
}