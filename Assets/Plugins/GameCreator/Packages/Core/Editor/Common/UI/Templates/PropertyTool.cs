using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Common
{
    public class PropertyTool : VisualElement
    {
        private const float LABEL_WIDTH = 120f;

        // MEMBERS: -------------------------------------------------------------------------------

        private readonly string m_Text;

        // PROPERTIES: ----------------------------------------------------------------------------

        public SerializedProperty Property { get; }
        public PropertyField PropertyField { get; }

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<SerializedPropertyChangeEvent> EventChange;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public PropertyTool(SerializedProperty property)
        {
            this.Property = property;
            this.PropertyField = new PropertyField(property);
            
            this.m_Text = property.displayName;
            
            this.Add(this.PropertyField);
            this.Bind(property.serializedObject);
            
            this.PropertyField.RegisterValueChangeCallback(this.OnChangeValue);
            this.PropertyField.RegisterCallback<GeometryChangedEvent>(this.OnChaneGeometry);
        }

        public PropertyTool(SerializedProperty property, string text) : this(property)
        {
            this.m_Text = text;
            this.PropertyField.label = text;
            this.RegisterCallback<GeometryChangedEvent>(this.OnCompleteDraw);
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void OnChangeValue(SerializedPropertyChangeEvent eventChange)
        {
            EventChange?.Invoke(eventChange);
        }
        
        private void OnChaneGeometry(GeometryChangedEvent geometryEvent)
        {
            this.RefreshSize();
        }
        
        private void OnCompleteDraw(GeometryChangedEvent geometryEvent)
        {
            this.UnregisterCallback<GeometryChangedEvent>(this.OnCompleteDraw);
            this.RefreshLabel();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshLabel()
        {
            Label label = FirstLabel();
            if (label != null) label.text = this.m_Text; 
        }
        
        private void RefreshSize()
        {
            if (string.IsNullOrEmpty(this.m_Text)) return;
            
            Label label = FirstLabel();
            if (label != null) label.style.width = LABEL_WIDTH;
        }

        private Label FirstLabel()
        {
            return this.PropertyField.Q<Label>(className: "unity-property-field__label");
        }
    }
}