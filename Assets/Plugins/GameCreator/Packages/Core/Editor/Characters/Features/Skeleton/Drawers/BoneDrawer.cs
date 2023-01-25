using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Characters
{
    [CustomPropertyDrawer(typeof(Bone))]
    public class BoneDrawer : PropertyDrawer
    {
        public const string PROP_TYPE = "m_Type";
        public const string PROP_HUMAN = "m_Human";
        public const string PROP_PATH = "m_Path";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return CreatePropertyGUI(property, property.displayName);
        }

        public static VisualElement CreatePropertyGUI(SerializedProperty property, string label)
        {
            VisualElement root = new VisualElement();
            VisualElement head = new VisualElement();
            VisualElement body = new VisualElement();

            root.Add(head);
            root.Add(body);

            SerializedProperty type = property.FindPropertyRelative(PROP_TYPE);
            SerializedProperty human = property.FindPropertyRelative(PROP_HUMAN);
            SerializedProperty path = property.FindPropertyRelative(PROP_PATH);

            PropertyTool fieldType = new PropertyTool(type, label);
            PropertyTool fieldHuman = new PropertyTool(human, " ");
            PropertyTool fieldPath = new PropertyTool(path, " ");

            fieldType.EventChange += changeEvent =>
            {
                RefreshBody(body, fieldHuman, fieldPath, changeEvent.changedProperty);
            };
            
            head.Add(fieldType);
            RefreshBody(body, fieldHuman, fieldPath, type);
            
            return root;
        }

        public static string GetTitle(SerializedProperty propertyBone)
        {
            SerializedProperty type = propertyBone.FindPropertyRelative(PROP_TYPE);
            SerializedProperty human = propertyBone.FindPropertyRelative(PROP_HUMAN);
            SerializedProperty path = propertyBone.FindPropertyRelative(PROP_PATH);

            return type.enumValueIndex switch
            {
                0 => "(none)",
                1 => human.enumDisplayNames[human.enumValueIndex],
                2 => path.stringValue,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static void RefreshBody(VisualElement body, PropertyTool fieldHuman, 
            PropertyTool fieldPath, SerializedProperty type)
        {
            body.Clear();
            switch (type.enumValueIndex)
            {
                case 1: // Human
                    body.Add(fieldHuman);
                    fieldHuman.Bind(type.serializedObject);
                    break;
                    
                case 2: // Path
                    body.Add(fieldPath);
                    fieldPath.Bind(type.serializedObject);
                    break;
            }
        }
    }
}