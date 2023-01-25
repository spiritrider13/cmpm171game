using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    public interface ITrack
    {
        int TrackOrder { get; }
        TrackType TrackType { get; }
        
        TrackAddType AllowAdd { get; }
        TrackRemoveType AllowRemove { get; }
        
        IClip[] Clips { get; }

        Color ColorConnectionLeft   { get; }
        Color ColorConnectionMiddle { get; }
        Color ColorConnectionRight  { get; }
        
        Color ColorClipNormal { get; }
        Color ColorClipSelect { get; }

        // METHODS: -------------------------------------------------------------------------------

        void OnStart(ISequence sequence, Args args);
        void OnComplete(ISequence sequence, Args args);
        void OnCancel(ISequence sequence, Args args);
        void OnUpdate(ISequence sequence, Args args);
    }
}