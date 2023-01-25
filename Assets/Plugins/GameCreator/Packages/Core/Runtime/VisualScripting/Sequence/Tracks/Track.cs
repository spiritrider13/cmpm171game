using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Serializable]
    public abstract class Track : ITrack
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public virtual int TrackOrder => 1;
        public virtual TrackType TrackType => TrackType.Single;
        
        public virtual TrackAddType AllowAdd => TrackAddType.Allow;
        public virtual TrackRemoveType AllowRemove => TrackRemoveType.Allow;
        
        public abstract IClip[] Clips { get; }

        public virtual Color ColorConnectionLeft => default;
        public virtual Color ColorConnectionMiddle => ColorTheme.Get(ColorTheme.Type.Red);
        public virtual Color ColorConnectionRight => default;

        public virtual Color ColorClipNormal => ColorTheme.Get(ColorTheme.Type.TextLight);
        public virtual Color ColorClipSelect => ColorTheme.Get(ColorTheme.Type.TextNormal);

        // TRACK METHODS: -------------------------------------------------------------------------
        
        void ITrack.OnStart(ISequence sequence, Args args)
        {
            foreach (IClip clip in this.Clips)
            {
                clip?.Reset();
            }
        }

        void ITrack.OnComplete(ISequence sequence, Args args)
        {
            foreach (IClip clip in this.Clips)
            {
                if (!clip.IsComplete)
                {
                    clip.Complete(this, args);
                }
            }
        }

        void ITrack.OnCancel(ISequence sequence, Args args)
        {
            foreach (IClip clip in this.Clips)
            {
                clip?.Cancel(this, args);
            }
        }

        void ITrack.OnUpdate(ISequence sequence, Args args)
        {
            float t = sequence.T;
            foreach (IClip clip in this.Clips)
            {
                if (t >= clip.TimeStart)
                {
                    if (!clip.IsStart)
                    {
                        clip.Start(this, args);
                        clip.Update(this, args, this.T(clip, t));
                    }
                    else if (t <= clip.TimeEnd)
                    {
                        clip.Update(this, args, this.T(clip, t));
                    }
                }
                else
                {
                    if (!clip.IsComplete)
                    {
                        clip.Complete(this, args);
                    }
                }
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private float T(IClip clip, float t)
        {
            if (clip.TimeStart >= clip.TimeEnd) return 1f;
            return (t - clip.TimeStart) / (clip.TimeEnd - clip.TimeStart);
        }
    }
}