using GameCreator.Editor.Common;
using GameCreator.Runtime.Variables;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Variables
{
    [CustomPropertyDrawer(typeof(NameVariableRuntime))]
    public class NameVariablesRuntimeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty list = property.FindPropertyRelative("m_List");
            NameVariableRuntime runtime = property.GetValue<NameVariableRuntime>();

            Object target = property.serializedObject.targetObject;
            bool isPrefab = PrefabUtility.IsPartOfPrefabAsset(target);

            switch (EditorApplication.isPlayingOrWillChangePlaymode && !isPrefab)
            {
                case true:
                    root.Add(new NameListView(runtime));
                    break;
                
                case false:
                    root.Add(new NameListTool(list));
                    break;
            }

            return root;
        }
    }
}