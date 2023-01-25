using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Usage
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private bool m_CanUse = false;
        [SerializeField] private bool m_ConsumeWhenUse = true;
        
        [SerializeField] private ItemConditions m_ConditionsCanUse = new ItemConditions();
        [SerializeField] private ItemInstructions m_InstructionsOnUse = new ItemInstructions();

        [SerializeField] private bool m_ExecuteFromParent = false;

        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private CopyRunnerConditionList m_TemplateConditionsCanUse;
        [NonSerialized] private CopyRunnerInstructionList m_TemplateInstructionsOnUse;

        // PROPERTIES: ----------------------------------------------------------------------------

        public bool AllowUse => this.m_CanUse;
        public bool ConsumeWhenUse => this.m_ConsumeWhenUse;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Usage()
        {
            this.m_TemplateInstructionsOnUse = null;
        }

        // CONDITION METHODS: ---------------------------------------------------------------------
        
        public static bool RunCanUse(Item item, Args args)
        {
            return RunCanUse(item, args, true);
        }
        
        private static bool RunCanUse(Item item, Args args, bool isLeaf)
        {
            if (isLeaf)
            {
                if (args?.Self == null) return false;
                if (!item.Usage.m_CanUse) return false;
            }
            
            if (item.Usage.m_ExecuteFromParent && item.Parent != null)
            {
                bool parentCanUse = RunCanUse(item.Parent, args, false);
                if (!parentCanUse) return false;
            }
            
            bool conditions = item.Usage
                .ConditionsCanUse(args?.Self.transform)
                .GetRunner<ConditionList>()
                .Check(args);

            return conditions;
        }
        
        // USAGE METHODS: -------------------------------------------------------------------------
        
        public static async Task RunOnUse(Item item, Args args)
        {
            try
            {
                if (item.Usage.m_ExecuteFromParent && item.Parent != null)
                {
                    await RunOnUse(item.Parent, args);
                }

                await item.Usage
                    .InstructionsOnUse(args.Self.transform)
                    .GetRunner<InstructionList>()
                    .Run(args);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString(), args.Self);
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private CopyRunnerConditionList ConditionsCanUse(Transform parent)
        {
            if (this.m_TemplateConditionsCanUse == null)
            {
                this.m_TemplateConditionsCanUse = CopyRunnerConditionList
                    .CreateTemplate<CopyRunnerConditionList>(
                        this.m_ConditionsCanUse.List
                    );
            }
                
            return this.m_TemplateConditionsCanUse.CreateRunner<CopyRunnerConditionList>(
                Vector3.zero, 
                Quaternion.identity, 
                parent
            );
        }
        
        private CopyRunnerInstructionList InstructionsOnUse(Transform parent)
        {
            if (this.m_TemplateInstructionsOnUse == null)
            {
                this.m_TemplateInstructionsOnUse = CopyRunnerInstructionList
                    .CreateTemplate<CopyRunnerInstructionList>(
                        this.m_InstructionsOnUse.List
                    );
            }
                
            return this.m_TemplateInstructionsOnUse.CreateRunner<CopyRunnerInstructionList>(
                Vector3.zero, 
                Quaternion.identity, 
                parent
            );
        }
    }
}