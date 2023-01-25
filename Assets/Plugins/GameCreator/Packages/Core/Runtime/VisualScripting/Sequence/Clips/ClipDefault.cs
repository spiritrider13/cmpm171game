using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Serializable]
    public class ClipDefault : Clip
    {
        public const string NAME_INSTRUCTIONS = nameof(m_Instructions);
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private InstructionList m_Instructions = new InstructionList();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private CopyRunnerInstructionList TemplateInstructions;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public ClipDefault() : base(default)
        { }
        
        public ClipDefault(float time) : base(time)
        { }

        public ClipDefault(InstructionList instructions, float time) : base(time)
        {
            this.m_Instructions = instructions;
        }

        // VIRTUAL METHODS: -----------------------------------------------------------------------

        protected override void OnStart(ITrack track, Args args)
        {
            base.OnStart(track, args);
            this.Run(args);
        }

        // METHODS: -------------------------------------------------------------------------------
        
        private void Run(Args args)
        {
            if (this.TemplateInstructions == null)
            {
                this.TemplateInstructions = CopyRunnerInstructionList
                    .CreateTemplate<CopyRunnerInstructionList>(this.m_Instructions);
            }

            CopyRunnerInstructionList copy = this.TemplateInstructions
                .CreateRunner<CopyRunnerInstructionList>(
                    args.Self != null ? args.Self.transform.position : Vector3.zero, 
                    args.Self != null ? args.Self.transform.rotation : Quaternion.identity, 
                    null
                );
            
            _ = copy.GetRunner<InstructionList>().Run(args.Clone);
        }
    }
}