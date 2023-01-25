using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.VisualScripting
{
    public class ClipTool : VisualElement
    {
        private const string NAME_CLIP_CONNECTION_L = "GC-Sequence-Clip-Connection-L";
        private const string NAME_CLIP_CONNECTION_M = "GC-Sequence-Clip-Connection-M";
        private const string NAME_CLIP_CONNECTION_R = "GC-Sequence-Clip-Connection-R";
        
        private const string NAME_CLIP_A = "GC-Sequence-Clip-A";
        private const string NAME_CLIP_B = "GC-Sequence-Clip-B";
        
        private static readonly IIcon ICON_CLIP_DRAG = new IconSequenceClip(ColorTheme.Type.TextNormal);
        
        private const int CLICK_THRESHOLD = 5;
        private const float CLIP_WIDTH = 14f;

        // MEMBERS: -------------------------------------------------------------------------------
        
        private readonly VisualElement m_ClipConnectionL;
        private readonly VisualElement m_ClipConnectionM;
        private readonly VisualElement m_ClipConnectionR;
        
        private readonly Image m_ClipA;
        private readonly Image m_ClipB;
        
        private readonly HandleDragManipulator m_Manipulator;
        
        private float m_DragOffset;

        private readonly IIcon m_IconClipNormal;
        private readonly IIcon m_IconClipSelect;

        // PROPERTIES: ----------------------------------------------------------------------------

        public SerializedProperty Property => this.TrackTool.PropertyClips
            .GetArrayElementAtIndex(this.ClipIndex);
        
        private TrackTool TrackTool { get; }
        public int ClipIndex { get; internal set; }

        public SerializedProperty PropertyTime
        {
            get
            {
                SerializedObject serializedObject = this.Property.serializedObject;
                SerializationUtils.ApplyUnregisteredSerialization(serializedObject);
                return this.Property.FindPropertyRelative("m_Time");
            }
        }

        public SerializedProperty PropertyDuration
        {
            get
            {
                SerializedObject serializedObject = this.Property.serializedObject;
                SerializationUtils.ApplyUnregisteredSerialization(serializedObject);
                return this.Property.FindPropertyRelative("m_Duration");
            }
        }

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<int, int> EventSelect;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ClipTool(TrackTool trackTool, int clipIndex)
        {
            this.TrackTool = trackTool;
            this.ClipIndex = clipIndex;
            this.pickingMode = PickingMode.Ignore;

            this.m_IconClipNormal = new IconSequenceClip(trackTool.Track.ColorClipNormal);
            this.m_IconClipSelect = new IconSequenceClip(trackTool.Track.ColorClipSelect);

            this.m_Manipulator = new HandleDragManipulator(
                this.OnChangeStart,
                this.OnChangeFinish,
                this.OnChangeMove
            );
            
            this.AddManipulator(this.m_Manipulator);
            
            this.m_ClipConnectionL = new VisualElement
            {
                name = NAME_CLIP_CONNECTION_L,
                pickingMode = PickingMode.Ignore
            };
            
            this.m_ClipConnectionM = new VisualElement
            {
                name = NAME_CLIP_CONNECTION_M,
                pickingMode = PickingMode.Ignore
            };
            
            this.m_ClipConnectionR = new VisualElement
            {
                name = NAME_CLIP_CONNECTION_R,
                pickingMode = PickingMode.Ignore
            };

            Color connectionL = trackTool.Track.ColorConnectionLeft;
            Color connectionM = trackTool.Track.ColorConnectionMiddle;
            Color connectionR = trackTool.Track.ColorConnectionRight;

            if (connectionL != default) this.m_ClipConnectionL.style.backgroundColor = connectionL;
            if (connectionM != default) this.m_ClipConnectionM.style.backgroundColor = connectionM;
            if (connectionR != default) this.m_ClipConnectionR.style.backgroundColor = connectionR;

            this.m_ClipA = new Image
            {
                name = NAME_CLIP_A,
                focusable = true,
                pickingMode = PickingMode.Position,
                image = this.m_IconClipNormal.Texture
            };
            
            this.m_ClipB = new Image
            {
                name = NAME_CLIP_B,
                focusable = true,
                pickingMode = PickingMode.Position,
                image = this.m_IconClipNormal.Texture
            };
            
            this.m_ClipA.AddManipulator(new ContextualMenuManipulator(this.OnContextClipA));
            this.m_ClipB.AddManipulator(new ContextualMenuManipulator(this.OnContextClipB));

            this.Add(this.m_ClipConnectionL);
            this.Add(this.m_ClipConnectionM);
            this.Add(this.m_ClipConnectionR);
            this.Add(this.m_ClipB);
            this.Add(this.m_ClipA);

            this.Refresh();
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void OnChangeStart()
        {
            this.BringToFront();
            this.TrackTool.Head.BringToFront();

            if (this.m_Manipulator.Target == this.m_ClipA)
            {
                float clip = this.m_ClipA.transform.position.x;
                this.m_DragOffset = this.m_Manipulator.StartPosition.x - clip;
            }
            
            if (this.m_Manipulator.Target == this.m_ClipB)
            {
                float clip = this.m_ClipB.transform.position.x;
                this.m_DragOffset = this.m_Manipulator.StartPosition.x - clip;
            }
            
            this.OnChange();
        }
        
        private void OnChangeFinish()
        {
            if (Mathf.Abs(this.m_Manipulator.Difference.x) < CLICK_THRESHOLD)
            {
                this.Select();
            }
            
            this.OnChange();
        }

        private void OnChangeMove()
        {
            this.OnChange();
        }
        
        private void OnChange()
        {
            this.OnChangeA();
            this.OnChangeB();

            SerializedObject serializedObject = this.Property.serializedObject;
            SerializationUtils.ApplyUnregisteredSerialization(serializedObject);
            
            this.Refresh();
            this.TrackTool.SequenceTool.DetailsTool.RefreshSign();
        }

        private void OnChangeA()
        {
            float offset = CLIP_WIDTH * 0.5f - this.m_DragOffset;
            float delta = this.m_Manipulator.Target == this.m_ClipA 
                ? this.m_Manipulator.FinishPosition.x + offset
                : 0f;
            
            if (delta == 0f) return;
            
            float deltaRatio = Mathf.Clamp01(delta / this.resolvedStyle.width);
            this.PropertyTime.floatValue = deltaRatio;
        }
        
        private void OnChangeB()
        {
            float offset = CLIP_WIDTH * 0.5f - this.m_DragOffset;
            float delta = this.m_Manipulator.Target == this.m_ClipB 
                ? this.m_Manipulator.FinishPosition.x + offset
                : 0f;
            
            if (delta == 0f) return;

            float time = this.PropertyTime.floatValue;
            float deltaRatio = Mathf.Clamp01(delta / this.resolvedStyle.width);
            this.PropertyDuration.floatValue = deltaRatio - time;
        }
        
        private void OnContextClipA(ContextualMenuPopulateEvent eventContext)
        {
            InputDropdownFloat.Open("Time", this.m_ClipA, result =>
            {
                SerializedObject so = this.PropertyTime.serializedObject; 
                so.Update();
                this.PropertyTime.floatValue = result;
                SerializationUtils.ApplyUnregisteredSerialization(so);
                this.TrackTool.SequenceTool.Refresh();
            }, this.PropertyTime.floatValue);
            
            eventContext.StopPropagation();
        }
        
        private void OnContextClipB(ContextualMenuPopulateEvent eventContext)
        {
            InputDropdownFloat.Open("Duration", this.m_ClipB, result =>
            {
                SerializedObject so = this.PropertyTime.serializedObject; 
                so.Update();
                this.PropertyDuration.floatValue = result;
                SerializationUtils.ApplyUnregisteredSerialization(so);
                this.TrackTool.SequenceTool.Refresh();
            }, this.PropertyDuration.floatValue);
            
            eventContext.StopPropagation();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Refresh()
        {
            this.Property.serializedObject.Update();

            float time = this.PropertyTime.floatValue;
            float duration = this.PropertyDuration.floatValue;
            
            bool aDragged = this.m_Manipulator.Target == this.m_ClipA &&
                            this.m_Manipulator.IsDragging;
            
            bool bDragged = this.m_Manipulator.Target == this.m_ClipB &&
                            this.m_Manipulator.IsDragging;
            
            this.SetClip(time, this.m_ClipA, aDragged);
            this.SetClip(time + duration, this.m_ClipB, bDragged);

            switch (this.TrackTool.Track.TrackType)
            {
                case TrackType.Single:
                    this.m_ClipB.style.display = DisplayStyle.None;
                    this.m_ClipConnectionL.style.display = DisplayStyle.None;
                    this.m_ClipConnectionM.style.display = DisplayStyle.None;
                    this.m_ClipConnectionR.style.display = DisplayStyle.None;
                    break;
                
                case TrackType.Range:
                    this.m_ClipB.style.display = DisplayStyle.Flex;
                    this.m_ClipConnectionL.style.display = DisplayStyle.Flex;
                    this.m_ClipConnectionM.style.display = DisplayStyle.Flex;
                    this.m_ClipConnectionR.style.display = DisplayStyle.Flex;
                    this.RefreshConnections();
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public void Select()
        {
            this.EventSelect?.Invoke(
                this.TrackTool.TrackIndex,
                this.ClipIndex
            );
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void SetClip(float value, Image clip, bool isDragging)
        {
            const float offset = CLIP_WIDTH * 0.5f;

            float x = Mathf.Lerp(0f, this.TrackTool.Width, value);
            float y = !float.IsNaN(clip.transform.position.y) ? clip.transform.position.y : 0f;
            
            Vector3 position = new Vector3(x - offset, y, 0);
            clip.transform.position = position;

            bool isTrack = this.TrackTool.SequenceTool.SelectedTrack == this.TrackTool.TrackIndex;
            
            if (isDragging) clip.image = ICON_CLIP_DRAG.Texture;
            else clip.image =  isTrack && this.TrackTool.SelectedClip == this.ClipIndex
                ? this.m_IconClipSelect.Texture
                : this.m_IconClipNormal.Texture;
        }

        private void RefreshConnections()
        {
            const float offset = CLIP_WIDTH * 0.5f;
            
            Vector3 clipA = this.m_ClipA.transform.position;
            Vector3 clipB = this.m_ClipB.transform.position;

            this.m_ClipConnectionL.transform.position = Vector3.zero;
            this.m_ClipConnectionL.style.width = clipA.x + offset;

            this.m_ClipConnectionM.transform.position = clipA + Vector3.right * offset;
            this.m_ClipConnectionM.style.width = clipB.x - clipA.x;
            
            this.m_ClipConnectionR.transform.position = clipB + Vector3.right * offset;
            this.m_ClipConnectionR.style.width = this.TrackTool.Width - clipB.x - offset;
        }
    }
}