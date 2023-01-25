using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Enable Noise")]
    [Category("Cameras/Shots/Noise/Enable Noise")]
    [Description("Toggles the active state of a Camera Shot's Noise system")]

    [Parameter("Active", "The next state")]
    [Keywords("Cameras", "Disable", "Activate", "Deactivate", "Bool", "Toggle", "Off", "On")]

    [Serializable]
    public class InstructionShotNoiseEnable : TInstructionShotNoise
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetBool m_Active = new PropertyGetBool(true);

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Set {this.m_Shot}[Noise] to {this.m_Active}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            ShotSystemNoise shotSystem = this.GetShotSystem<ShotSystemNoise>(args);
            
            if (shotSystem == null) return DefaultResult;
            shotSystem.Active = this.m_Active.Get(args);
            
            return DefaultResult;
        }
    }
}