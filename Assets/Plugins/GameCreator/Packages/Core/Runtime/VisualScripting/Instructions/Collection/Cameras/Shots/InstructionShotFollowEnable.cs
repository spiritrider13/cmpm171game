using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Enable Follow")]
    [Category("Cameras/Shots/Follow/Enable Follow")]
    [Description("Toggles the active state of a Camera Shot's Follow system")]

    [Parameter("Active", "The next state")]
    [Keywords("Cameras", "Disable", "Activate", "Deactivate", "Bool", "Toggle", "Off", "On")]

    [Serializable]
    public class InstructionShotFollowEnable : TInstructionShotFollow
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetBool m_Active = new PropertyGetBool(true);

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Set {this.m_Shot}[Follow] to {this.m_Active}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            ShotSystemFollow shotSystem = this.GetShotSystem<ShotSystemFollow>(args);
            
            if (shotSystem == null) return DefaultResult;
            shotSystem.Active = this.m_Active.Get(args);
            
            return DefaultResult;
        }
    }
}