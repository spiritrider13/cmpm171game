using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Crafting
    {
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnEnterPlayMode]
        private static void InitializeOnEnterPlayMode()
        {
            EventCraft = null;
            EventDismantle = null;

            LastItemCrafted = null;
            LastItemDismantled = null;
        }
        
        #endif

        public static RuntimeItem LastItemCrafted = null;
        public static RuntimeItem LastItemDismantled = null;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Ingredient[] m_Ingredients = new Ingredient[0];

        [SerializeField] private bool m_CanCraft;
        [SerializeField] private ItemConditions m_ConditionsCraft = new ItemConditions();
        [SerializeField] private ItemInstructions m_InstructionsOnCraft = new ItemInstructions();

        [SerializeField] private bool m_CanDismantle;
        [SerializeField] private ItemConditions m_ConditionsDismantle = new ItemConditions();
        [SerializeField] private ItemInstructions m_InstructionsOnDismantle = new ItemInstructions();
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private CopyRunnerConditionList m_TemplateConditionsCraft;
        private CopyRunnerConditionList m_TemplateConditionsDismantle;

        private CopyRunnerInstructionList m_TemplateInstructionsOnCraft;
        private CopyRunnerInstructionList m_TemplateInstructionsOnDismantle;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Ingredient[] Ingredients => this.m_Ingredients;

        public bool AllowToCraft => this.m_CanCraft;
        public bool AllowToDismantle => this.m_CanDismantle;
        
        // EVENTS: --------------------------------------------------------------------------------

        public static event Action EventCraft;
        public static event Action EventDismantle;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Crafting()
        {
            this.m_TemplateConditionsCraft = null;
            this.m_TemplateConditionsDismantle = null;
            this.m_TemplateInstructionsOnCraft = null;
            this.m_TemplateInstructionsOnDismantle = null;
        }
        
        // CRAFT METHODS: -------------------------------------------------------------------------
        
        public static bool CanCraft(Item item, Bag inputBag, Bag outputBag)
        {
            if (item == null || inputBag == null || outputBag == null) return false;
            if (!item.Crafting.m_CanCraft) return false;
            
            bool canCraft = ConditionsCraft(item, outputBag)
                .GetRunner<ConditionList>()
                .Check(new Args(inputBag.gameObject, outputBag.gameObject));
            
            if (!canCraft) return false;
            
            int ingredientsLength = item.Crafting.Ingredients.Length;
            for (int i = 0; i < ingredientsLength; ++i)
            {
                Ingredient ingredient = item.Crafting.Ingredients[i];
                if (inputBag.Content.ContainsType(ingredient.Item, ingredient.Amount)) continue;
                
                return false;
            }

            return outputBag.Content.CanAddType(item, true);
        }
        
        public static RuntimeItem Craft(Item item, Bag inputBag, Bag outputBag)
        {
            if (!CanCraft(item, inputBag, outputBag)) return null;

            int ingredientsLength = item.Crafting.Ingredients.Length;
            List<RuntimeItem> removeRuntimeItemList = new List<RuntimeItem>();
            
            for (int i = 0; i < ingredientsLength; ++i)
            {
                Ingredient ingredient = item.Crafting.Ingredients[i];
                for (int j = 0; j < ingredient.Amount; ++j)
                {
                    RuntimeItem removeRuntimeItem = inputBag.Content.RemoveType(ingredient.Item);
                    if (removeRuntimeItem != null)
                    {
                        removeRuntimeItemList.Add(removeRuntimeItem);
                        continue;
                    }

                    foreach (RuntimeItem restoreRuntimeItem in removeRuntimeItemList)
                    {
                        inputBag.Content.Add(restoreRuntimeItem, true);
                    }
                    
                    return null;
                }
            }

            RuntimeItem craftRuntimeItem = outputBag.Content.AddType(item, true);
            if (craftRuntimeItem == null)
            {
                foreach (RuntimeItem restoreRuntimeItem in removeRuntimeItemList)
                {
                    inputBag.Content.Add(restoreRuntimeItem, true);
                }

                return null;
            }

            LastItemCrafted = craftRuntimeItem;

            try
            {
                _ = InstructionsOnCraft(item, outputBag)
                    .GetRunner<InstructionList>()
                    .Run(new Args(inputBag.gameObject, outputBag.gameObject));
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString(), inputBag.gameObject);
            }

            EventCraft?.Invoke();
            return craftRuntimeItem;
        }

        // DISMANTLE METHODS: ---------------------------------------------------------------------

        public static bool CanDismantle(Item item, Bag inputBag, Bag outputBag)
        {
            if (item == null || inputBag == null || outputBag == null) return false;
            if (!inputBag.Content.ContainsType(item, 1)) return false;
            if (!item.Crafting.m_CanDismantle) return false;
            
            bool canDismantle = ConditionsDismantle(item, outputBag)
                .GetRunner<ConditionList>()
                .Check(new Args(inputBag.gameObject, outputBag.gameObject));

            return canDismantle;
        }
        
        public static RuntimeItem[] Dismantle(Item item, Bag inputBag, Bag outputBag, float chance)
        {
            if (!CanDismantle(item, inputBag, outputBag)) return null;
            
            RuntimeItem removeRuntimeItem = inputBag.Content.RemoveType(item);
            return removeRuntimeItem != null 
                ? ProcessDismantle(removeRuntimeItem, inputBag, outputBag, chance) 
                : null;
        }
        
        public static RuntimeItem[] Dismantle(RuntimeItem runtimeItem, Bag inputBag, Bag outputBag, float chance)
        {
            if (!CanDismantle(runtimeItem.Item, inputBag, outputBag)) return null;
            
            RuntimeItem removeRuntimeItem = inputBag.Content.Remove(runtimeItem);
            return ProcessDismantle(removeRuntimeItem, inputBag, outputBag, chance);
        }
        
        private static RuntimeItem[] ProcessDismantle(RuntimeItem removedItem, Bag inputBag, Bag outputBag, float chance)
        {
            if (removedItem?.Item == null) return null;
            
            int ingredientsLength = removedItem.Item.Crafting.Ingredients.Length;
            List<RuntimeItem> dismantleRuntimeItems = new List<RuntimeItem>();
            
            for (int i = 0; i < ingredientsLength; ++i)
            {
                Ingredient ingredient = removedItem.Item.Crafting.Ingredients[i];
                for (int j = 0; j < ingredient.Amount; ++j)
                {
                    float randomChance = UnityEngine.Random.value;
                    if (randomChance > chance) continue;
                    
                    RuntimeItem dismantleRuntimeItem = outputBag
                        .Content.AddType(ingredient.Item, true);
                    
                    if (dismantleRuntimeItem != null)
                    {
                        dismantleRuntimeItems.Add(dismantleRuntimeItem);
                    }
                }
            }
            
            LastItemDismantled = removedItem;

            try
            {
                _ = InstructionsOnDismantle(removedItem.Item, outputBag)
                    .GetRunner<InstructionList>()
                    .Run(new Args(inputBag.gameObject, outputBag.gameObject));
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString(), inputBag.gameObject);
            }

            EventDismantle?.Invoke();
            return dismantleRuntimeItems.ToArray();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        private static CopyRunnerConditionList ConditionsCraft(Item item, Bag bag)
        {
            if (item.Crafting.m_TemplateConditionsCraft == null)
            {
                item.Crafting.m_TemplateConditionsCraft = CopyRunnerConditionList
                    .CreateTemplate<CopyRunnerConditionList>(
                        item.Crafting.m_ConditionsCraft.List
                    );
            }
                
            return item.Crafting.m_TemplateConditionsCraft
                .CreateRunner<CopyRunnerConditionList>(
                    Vector3.zero, Quaternion.identity, 
                    bag.Wearer != null ? bag.Wearer.transform : bag.transform
            );
        }
        
        private static CopyRunnerInstructionList InstructionsOnCraft(Item item, Bag bag)
        {
            if (item.Crafting.m_TemplateInstructionsOnCraft == null)
            {
                item.Crafting.m_TemplateInstructionsOnCraft = CopyRunnerInstructionList
                    .CreateTemplate<CopyRunnerInstructionList>(
                        item.Crafting.m_InstructionsOnCraft.List
                    );
            }
                
            return item.Crafting.m_TemplateInstructionsOnCraft
                .CreateRunner<CopyRunnerInstructionList>(
                    Vector3.zero, Quaternion.identity, 
                    bag.Wearer != null ? bag.Wearer.transform : bag.transform
                );
        }
        
        private static CopyRunnerConditionList ConditionsDismantle(Item item, Bag bag)
        {
            if (item.Crafting.m_TemplateConditionsDismantle == null)
            {
                item.Crafting.m_TemplateConditionsDismantle = CopyRunnerConditionList
                    .CreateTemplate<CopyRunnerConditionList>(
                        item.Crafting.m_ConditionsDismantle.List
                    );
            }
                
            return item.Crafting.m_TemplateConditionsDismantle
                .CreateRunner<CopyRunnerConditionList>(
                    Vector3.zero, Quaternion.identity, 
                    bag.Wearer != null ? bag.Wearer.transform : bag.transform
                );
        }
        
        private static CopyRunnerInstructionList InstructionsOnDismantle(Item item, Bag bag)
        {
            if (item.Crafting.m_TemplateInstructionsOnDismantle == null)
            {
                item.Crafting.m_TemplateInstructionsOnDismantle = CopyRunnerInstructionList
                    .CreateTemplate<CopyRunnerInstructionList>(
                        item.Crafting.m_InstructionsOnDismantle.List
                    );
            }
                
            return item.Crafting.m_TemplateInstructionsOnDismantle
                .CreateRunner<CopyRunnerInstructionList>(
                    Vector3.zero, Quaternion.identity, 
                    bag.Wearer != null ? bag.Wearer.transform : bag.transform
                );
        }
    }
}