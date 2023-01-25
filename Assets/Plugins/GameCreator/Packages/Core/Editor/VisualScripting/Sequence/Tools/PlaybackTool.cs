using System;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.VisualScripting
{
    public class PlaybackTool : VisualElement
    {
        private struct Metric
        {
            public float time;
            public string name;
            public VisualElement element;
        }
        
        // CONSTANTS: -----------------------------------------------------------------------------
        
        private const string NAME_PLAYBACK_TRACK = "GC-Sequence-Playback-Track";
        private const string NAME_PLAYBACK_TOOLTIP = "GC-Sequence-Playback-Tooltip";
        private const string NAME_PLAYBACK_HEAD = "GC-Sequence-Playback-Head";

        private const string NAME_PLAYBACK_METRIC_UNIT = "GC-Sequence-Playback-Metric-Unit";
        private const string NAME_PLAYBACK_METRIC_FRAC = "GC-Sequence-Playback-Metric-Fraction";

        private const string KEY_PLAYBACK_VALUE = "gc:sequence:playback-head-value";
        private static readonly IIcon ICON_HEAD_ON = new IconSequenceHead(ColorTheme.Type.TextNormal);
        private static readonly IIcon ICON_HEAD_OFF = new IconSequenceHead(ColorTheme.Type.TextLight);

        private const float TOOLTIP_WIDTH = 35f;
        private const float TOOLTIP_PADDING = 10f;

        private const int INDEX_METRIC_0 = 0;
        private const int INDEX_METRIC_1 = 4;

        // MEMBERS: -------------------------------------------------------------------------------

        private readonly SequenceTool m_SequenceTool;
        private readonly HandleDragManipulator m_Manipulator;
        
        private readonly VisualElement m_PlaybackTrack;
        private readonly Label m_PlaybackTooltip;
        private readonly Image m_PlaybackHead;
        
        private readonly Metric[] m_Metrics = 
        {
            new Metric { time = 0.00f, name = NAME_PLAYBACK_METRIC_UNIT },
            new Metric { time = 0.25f, name = NAME_PLAYBACK_METRIC_UNIT },
            new Metric { time = 0.50f, name = NAME_PLAYBACK_METRIC_UNIT },
            new Metric { time = 0.75f, name = NAME_PLAYBACK_METRIC_UNIT },
            new Metric { time = 1.00f, name = NAME_PLAYBACK_METRIC_UNIT },
            new Metric { time = 0.05f, name = NAME_PLAYBACK_METRIC_FRAC },
            new Metric { time = 0.10f, name = NAME_PLAYBACK_METRIC_FRAC },
            new Metric { time = 0.15f, name = NAME_PLAYBACK_METRIC_FRAC },
            new Metric { time = 0.20f, name = NAME_PLAYBACK_METRIC_FRAC },
            new Metric { time = 0.30f, name = NAME_PLAYBACK_METRIC_FRAC },
            new Metric { time = 0.35f, name = NAME_PLAYBACK_METRIC_FRAC },
            new Metric { time = 0.40f, name = NAME_PLAYBACK_METRIC_FRAC },
            new Metric { time = 0.45f, name = NAME_PLAYBACK_METRIC_FRAC },
            new Metric { time = 0.55f, name = NAME_PLAYBACK_METRIC_FRAC },
            new Metric { time = 0.60f, name = NAME_PLAYBACK_METRIC_FRAC },
            new Metric { time = 0.65f, name = NAME_PLAYBACK_METRIC_FRAC },
            new Metric { time = 0.70f, name = NAME_PLAYBACK_METRIC_FRAC },
            new Metric { time = 0.80f, name = NAME_PLAYBACK_METRIC_FRAC },
            new Metric { time = 0.85f, name = NAME_PLAYBACK_METRIC_FRAC },
            new Metric { time = 0.90f, name = NAME_PLAYBACK_METRIC_FRAC },
            new Metric { time = 0.95f, name = NAME_PLAYBACK_METRIC_FRAC },
        };

        // PROPERTIES: ----------------------------------------------------------------------------

        public float Value
        {
            get => EditorPrefs.GetFloat(KEY_PLAYBACK_VALUE, 0.25f);
            set
            {
                if (Math.Abs(this.Value - value) < float.Epsilon) return;
                EditorPrefs.SetFloat(KEY_PLAYBACK_VALUE, value);
                
                this.RefreshPlaybackHead();
                this.RefreshPlaybackTooltip();
                this.EventChange?.Invoke();
            }
        }
        
        public float MaxFrame { get; set; } = 100;

        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventChange;
        
        public event Action EventDragStart;
        public event Action EventDragFinish;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public PlaybackTool(SequenceTool sequenceTool)
        {
            this.m_SequenceTool = sequenceTool;

            this.m_PlaybackTrack = new VisualElement { name = NAME_PLAYBACK_TRACK };
            
            this.m_PlaybackTooltip = new Label("100")
            {
                name = NAME_PLAYBACK_TOOLTIP,
                pickingMode = PickingMode.Ignore,
                style =
                {
                    width = new Length(TOOLTIP_WIDTH, LengthUnit.Pixel),
                    display = DisplayStyle.None
                }
            };
            
            this.m_PlaybackHead = new Image
            {
                image = ICON_HEAD_OFF.Texture,
                name = NAME_PLAYBACK_HEAD
            };

            this.m_PlaybackTrack.RegisterCallback<GeometryChangedEvent>(this.OnChangeGeometry);

            this.m_SequenceTool.SetupControlL(this);

            this.m_Manipulator = new HandleDragManipulator(
                this.OnChangeFromStart,
                this.OnChangeFromFinish,
                this.OnChangeFromMove
            );
            
            this.m_PlaybackTrack.AddManipulator(this.m_Manipulator);

            for (int i = 0; i < this.m_Metrics.Length; i++)
            {
                this.m_Metrics[i].element = new VisualElement
                {
                    name = this.m_Metrics[i].name,
                    style = { backgroundColor = ColorTheme.Get(ColorTheme.Type.TextNormal) }
                };
                
                this.m_PlaybackTrack.Add(this.m_Metrics[i].element);
            }

            this.m_Metrics[INDEX_METRIC_0].element.visible = this.m_SequenceTool.ShowMetric0;
            this.m_Metrics[INDEX_METRIC_1].element.visible = this.m_SequenceTool.ShowMetric1;

            this.m_PlaybackTrack.Add(this.m_PlaybackTooltip);
            this.m_PlaybackTrack.Add(this.m_PlaybackHead);
            this.Add(this.m_PlaybackTrack);
            
            this.m_SequenceTool.SetupControlR(this);
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void OnChangeFromStart()
        {
            float x = this.m_Manipulator.StartPosition.x;
            this.m_PlaybackHead.image = ICON_HEAD_ON.Texture;
            this.m_PlaybackTooltip.style.display = DisplayStyle.Flex;
            
            this.OnChange(x);
            this.EventDragStart?.Invoke();
        }

        private void OnChangeFromFinish()
        {
            this.m_PlaybackHead.image = ICON_HEAD_OFF.Texture;
            this.m_PlaybackTooltip.style.display = DisplayStyle.None;
            this.EventDragFinish?.Invoke();
        }

        private void OnChangeFromMove()
        {
            float x = this.m_Manipulator.FinishPosition.x;
            this.OnChange(x);
        }

        private void OnChange(float x)
        {
            float contentLength = this.m_PlaybackTrack.resolvedStyle.width;
            if (contentLength <= 0f) return;

            float normalized = x / contentLength;
            this.Value = Mathf.Clamp01(normalized);
        }

        private void OnChangeGeometry(GeometryChangedEvent eventGeometry)
        {
            if (eventGeometry.oldRect == eventGeometry.newRect) return;
            
            this.RefreshPlaybackHead();
            this.RefreshPlaybackTooltip();
            this.RefreshPlaybackMetrics();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshPlaybackHead()
        {
            float playheadOffset = this.m_PlaybackHead.resolvedStyle.width * 0.5f;
            float x = Mathf.Lerp(0f, this.m_PlaybackTrack.resolvedStyle.width, this.Value);
            float y = this.m_PlaybackHead.transform.position.y;
            
            Vector3 position = new Vector3(x - playheadOffset, y, 0);
            this.m_PlaybackHead.transform.position = position;
        }

        private void RefreshPlaybackTooltip()
        {
            float time = this.Value;
            float offset = time < 0.75f 
                ? TOOLTIP_PADDING
                : -1f * (TOOLTIP_WIDTH + TOOLTIP_PADDING);
            
            float x = Mathf.Lerp(0f, this.m_PlaybackTrack.resolvedStyle.width, time);
            float y = this.m_PlaybackTooltip.transform.position.y;
            
            Vector3 position = new Vector3(x + offset, y, 0);
            this.m_PlaybackTooltip.transform.position = position;
            this.m_PlaybackTooltip.text = (time * this.MaxFrame).ToString("0000");
        }

        private void RefreshPlaybackMetrics()
        {
            float size = this.m_PlaybackTrack.resolvedStyle.width - 1;
            for (int i = 0; i < this.m_Metrics.Length; i++)
            {
                float position = size * this.m_Metrics[i].time;
                this.m_Metrics[i].element.transform.position = new Vector3(position, 0);
            }
        }
    }
}