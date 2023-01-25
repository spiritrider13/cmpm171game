using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Serializable]
    public class SequenceTester : Sequence
    {
        [SerializeField] private TimeMode m_TimeMode;
        [SerializeField] private float m_Duration = 1f;

        public override float Duration => this.m_Duration;
        public override TimeMode TimeMode => this.m_TimeMode;
        
        public SequenceTester(Track[] tracks) : base(tracks)
        { }
        
        public async Task Run(Args args)
        {
            await this.DoRun(args);
        }

        protected virtual void Cancel(Args args)
        {
            this.DoCancel(args);
        }
    }
}