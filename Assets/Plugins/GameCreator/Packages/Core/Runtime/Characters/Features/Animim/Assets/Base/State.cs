using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    public abstract class State : ScriptableObject, IState, ISerializationCallbackReceiver
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] protected AvatarMask m_StateMask;

        [SerializeField] private EntryAnimationClip m_Entry = new EntryAnimationClip();
        [SerializeField] private ExitAnimationClip m_Exit = new ExitAnimationClip();

        [SerializeField] private InstructionList m_OnChange = new InstructionList();
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private CopyRunnerInstructionList TemplateOnChange;

        // PROPERTIES: ----------------------------------------------------------------------------

        public abstract RuntimeAnimatorController StateController { get; }
        
        public AvatarMask StateMask => this.m_StateMask;
        public bool HasStateMask => this.m_StateMask != null;

        public AnimationClip EntryClip => this.m_Entry.EntryClip;
        public bool HasEntryClip => this.m_Entry.EntryClip != null;
        public AvatarMask EntryMask => this.m_Entry.EntryMask;
        
        public AnimationClip ExitClip => this.m_Exit.ExitClip;
        public bool HasExitClip => this.m_Exit.ExitClip != null;
        public AvatarMask ExitMask => this.m_Exit.ExitMask;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void RunChange(Args args)
        {
            if (this.TemplateOnChange == null)
            {
                this.TemplateOnChange = CopyRunnerInstructionList
                    .CreateTemplate<CopyRunnerInstructionList>(this.m_OnChange);
            }

            CopyRunnerInstructionList copy = this.TemplateOnChange
                .CreateRunner<CopyRunnerInstructionList>(
                    args.Self != null ? args.Self.transform.position : Vector3.zero, 
                    args.Self != null ? args.Self.transform.rotation : Quaternion.identity, 
                    null
                );
            
            _ = copy.GetRunner<InstructionList>().Run(args.Clone);
        }

        // SERIALIZATION CALLBACKS: ---------------------------------------------------------------
        
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            #if UNITY_EDITOR
            
            if (AssemblyUtils.IsReloading) return;
            this.BeforeSerialize();
            
            #endif
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            #if UNITY_EDITOR
            
            if (AssemblyUtils.IsReloading) return;
            this.AfterSerialize();
            
            #endif
        }

        protected abstract void BeforeSerialize();
        protected abstract void AfterSerialize();
    }
}