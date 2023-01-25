using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Equip
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private bool m_IsEquippable = false;
        [SerializeField] private GameObject m_Prefab;

        [SerializeField] private ItemConditions m_ConditionsEquip = new ItemConditions();
        [SerializeField] private ItemInstructions m_InstructionsOnEquip = new ItemInstructions();
        [SerializeField] private ItemInstructions m_InstructionsOnUnequip = new ItemInstructions();
        
        [SerializeField] private bool m_ExecuteFromParent = false;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private CopyRunnerConditionList m_TemplateConditionsEquip;
        [NonSerialized] private CopyRunnerInstructionList m_TemplateInstructionsOnEquip;
        [NonSerialized] private CopyRunnerInstructionList m_TemplateInstructionsOnUnequip;

        // PROPERTIES: ----------------------------------------------------------------------------

        public bool IsEquippable => this.m_IsEquippable;
        public GameObject Prefab => this.m_Prefab;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Equip()
        {
            this.m_TemplateConditionsEquip = null;
            this.m_TemplateInstructionsOnEquip = null;
            this.m_TemplateInstructionsOnUnequip = null;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------ 
        
        public static bool RunCanEquip(Item item, Args args)
        {
            if (item.Equip.m_ExecuteFromParent && item.Parent != null)
            {
                bool parentCanEquip = RunCanEquip(item.Parent, args);
                if (!parentCanEquip) return false;
            }
            
            bool conditions = item.Equip
                .ConditionsEquip(args.Self.transform)
                .GetRunner<ConditionList>()
                .Check(args);

            return conditions;
        }
        
        public static async Task RunOnEquip(Item item, Args args)
        {
            try
            {
                if (item.Equip.m_ExecuteFromParent && item.Parent != null)
                {
                    await RunOnEquip(item.Parent, args);
                }

                await item.Equip
                    .InstructionsOnEquip(args.Self.transform)
                    .GetRunner<InstructionList>()
                    .Run(args);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString(), args.Self);
            }
        }
        
        public static async Task RunOnUnequip(Item item, Args args)
        {
            try
            {
                if (item.Equip.m_ExecuteFromParent && item.Parent != null)
                {
                    await RunOnUnequip(item.Parent, args);
                }

                await item.Equip
                    .InstructionsOnUnequip(args.Self.transform)
                    .GetRunner<InstructionList>()
                    .Run(args);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString(), args.Self);
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private CopyRunnerConditionList ConditionsEquip(Transform parent)
        {
            if (this.m_TemplateConditionsEquip == null)
            {
                this.m_TemplateConditionsEquip = CopyRunnerConditionList
                    .CreateTemplate<CopyRunnerConditionList>(
                        this.m_ConditionsEquip.List
                    );
            }
                
            return this.m_TemplateConditionsEquip.CreateRunner<CopyRunnerConditionList>(
                Vector3.zero, 
                Quaternion.identity, 
                parent
            );
        }
        
        private CopyRunnerInstructionList InstructionsOnEquip(Transform parent)
        {
            if (this.m_TemplateInstructionsOnEquip == null)
            {
                this.m_TemplateInstructionsOnEquip = CopyRunnerInstructionList
                    .CreateTemplate<CopyRunnerInstructionList>(
                        this.m_InstructionsOnEquip.List
                    );
            }
                
            return this.m_TemplateInstructionsOnEquip.CreateRunner<CopyRunnerInstructionList>(
                Vector3.zero, 
                Quaternion.identity, 
                parent
            );
        }
        
        private CopyRunnerInstructionList InstructionsOnUnequip(Transform parent)
        {
            if (this.m_TemplateInstructionsOnUnequip == null)
            {
                this.m_TemplateInstructionsOnUnequip = CopyRunnerInstructionList
                    .CreateTemplate<CopyRunnerInstructionList>(
                        this.m_InstructionsOnUnequip.List
                    );
            }
                
            return this.m_TemplateInstructionsOnUnequip.CreateRunner<CopyRunnerInstructionList>(
                Vector3.zero, 
                Quaternion.identity, 
                parent
            );
        }
    }
}