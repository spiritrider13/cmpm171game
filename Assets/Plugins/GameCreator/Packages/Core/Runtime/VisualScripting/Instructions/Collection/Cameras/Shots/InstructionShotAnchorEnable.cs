using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Enable Anchor")]
    [Category("Cameras/Shots/Anchor/Enable Anchor")]
    [Description("Toggles the active state of a Camera Shot's Anchor system")]

    [Parameter("Active", "The next state")]
    [Keywords("Cameras", "Disable", "Activate", "Deactivate", "Bool", "Toggle", "Off", "On")]

    [Serializable]
    public class InstructionShotAnchorEnable : TInstructionShotAnchor
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetBool m_Active = new PropertyGetBool(true);

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Set {this.m_Shot}[Anchor] to {this.m_Active}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            ShotSystemAnchor shotSystem = this.GetShotSystem<ShotSystemAnchor>(args);
            
            if (shotSystem == null) return DefaultResult;
            shotSystem.Active = this.m_Active.Get(args);
            
            return DefaultResult;
        }
    }
}