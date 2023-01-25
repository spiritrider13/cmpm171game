using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Enable First Person")]
    [Category("Cameras/Shots/First Person/Enable First Person")]
    [Description("Toggles the active state of a Camera Shot's First Person system")]

    [Parameter("Active", "The next state")]
    [Keywords("Cameras", "Disable", "Activate", "Deactivate", "Bool", "Toggle", "Off", "On")]

    [Serializable]
    public class InstructionShotFirstPersonEnable : TInstructionShotFirstPerson
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetBool m_Active = new PropertyGetBool(true);

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Set {this.m_Shot}[First Person] to {this.m_Active}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            ShotSystemFirstPerson shotSystem = this.GetShotSystem<ShotSystemFirstPerson>(args);
            
            if (shotSystem == null) return DefaultResult;
            shotSystem.Active = this.m_Active.Get(args);
            
            return DefaultResult;
        }
    }
}