using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace GameCreator.Runtime.Characters.Animim
{
    public class GesturePlayableBehaviour : TAnimimPlayableBehaviour
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] internal int AnimationClipHash { get; private set; }
        [field: NonSerialized] protected override Playable AnimationPlayable { get; }

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public GesturePlayableBehaviour(AnimationClip animationClip, AvatarMask avatarMask,
            BlendMode blendMode, AnimimGraph animimGraph, ConfigGesture config) 
            : base(avatarMask, blendMode, animimGraph, config)
        {
            this.AnimationClipHash = animationClip.GetHashCode();
            this.AnimationPlayable = AnimationClipPlayable.Create(
                animimGraph.Graph, 
                animationClip
            );
        }
        
        public GesturePlayableBehaviour() : base(null, BlendMode.Blend, null, default)
        { }
    }
}