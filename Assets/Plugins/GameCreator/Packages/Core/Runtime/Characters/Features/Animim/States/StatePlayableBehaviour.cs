using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace GameCreator.Runtime.Characters.Animim
{
    public sealed class StatePlayableBehaviour : TAnimimPlayableBehaviour
    {
        private const float TRANSITION_FAST = 0.15f;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public int Layer { get; }
        [field: NonSerialized] public State State { get; }
        
        [field: NonSerialized] public bool IsExiting { get; private set; }

        [field: NonSerialized] public bool IsEntryClipComplete { get; private set; }
        [field: NonSerialized] public bool IsExitClipComplete { get; private set; }

        protected override Playable AnimationPlayable { get; }

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public StatePlayableBehaviour(AnimationClip animationClip, AvatarMask avatarMask, int layer,
            BlendMode blendMode, AnimimGraph animimGraph, ConfigState config) 
            : base(avatarMask, blendMode, animimGraph, config)
        {
            this.State = null;
            this.Layer = layer;
            this.IsExiting = false;
            this.IsEntryClipComplete = true;
            this.IsExitClipComplete = false;

            this.AnimationPlayable = AnimationClipPlayable.Create(
                animimGraph.Graph, 
                animationClip
            );
        }
        
        public StatePlayableBehaviour(RuntimeAnimatorController rtc, AvatarMask avatarMask, 
            int layer, BlendMode blendMode, AnimimGraph animimGraph, ConfigState config) 
            : base(avatarMask, blendMode, animimGraph, config)
        {
            this.State = null;
            this.Layer = layer;
            this.IsExiting = false;
            this.IsEntryClipComplete = true;
            this.IsExitClipComplete = false;

            this.AnimationPlayable = AnimatorControllerPlayable.Create(
                animimGraph.Graph, 
                rtc
            );
        }
        
        public StatePlayableBehaviour(State state, int layer, BlendMode blendMode, 
            AnimimGraph animimGraph, ConfigState config) 
            : base(state.StateMask, blendMode, animimGraph, config)
        {
            this.IsExiting = false;
            this.IsEntryClipComplete = true;
            this.IsExitClipComplete = false;

            if (state.HasEntryClip)
            {
                this.IsEntryClipComplete = false;
                _ = this.PlayEntryClip(animimGraph, state, config);
                
                this.m_Config.TransitionIn = 0f;
                this.m_Config.DelayIn += config.TransitionIn;
            }
            
            this.State = state;
            this.Layer = layer;

            this.AnimationPlayable = AnimatorControllerPlayable.Create(
                animimGraph.Graph, 
                state.StateController
            );
        }
        
        public StatePlayableBehaviour() : base(null, BlendMode.Blend, null, default)
        { }

        public override void Stop(float delay, float transitionOut)
        {
            this.IsExiting = true;

            if (this.State != null && this.State.HasEntryClip)
            {
                this.m_AnimimGraph.Gestures.Stop(
                    this.State.EntryClip, 
                    delay, transitionOut
                );
            }
            
            if (this.State != null && this.State.HasExitClip)
            {
                _ = this.PlayExitClip(new ConfigGesture(
                    delay, this.State.ExitClip.length, 1f, false,
                    TRANSITION_FAST, transitionOut
                ));

                delay += TRANSITION_FAST + transitionOut;
                transitionOut = 0f;
            }
            
            base.Stop(delay, transitionOut);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private async Task PlayEntryClip(AnimimGraph animimGraph, State state, ConfigState config)
        {
            await animimGraph.Gestures.CrossFade(
                state.EntryClip, state.EntryMask, this.m_BlendMode,
                new ConfigGesture(
                    config.DelayIn, state.EntryClip.length,
                    1f, config.RootMotion,
                    config.TransitionIn, TRANSITION_FAST
                ),
                false
            );

            this.IsEntryClipComplete = true;
        }
        
        private async Task PlayExitClip(ConfigGesture config)
        {
            await this.m_AnimimGraph.Gestures.CrossFade(
                this.State.ExitClip, this.State.ExitMask, this.m_BlendMode,
                config, false
            );

            this.IsExitClipComplete = true;
        }
    }
}