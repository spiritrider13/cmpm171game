using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class EquipmentRuntimeSlot
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private bool m_Override;
        [SerializeField] private Bone m_OverrideBone = new Bone(HumanBodyBones.Hips);
        
        [SerializeField] private IdString rootRuntimeItemIDEquipped = IdString.EMPTY;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool IsEquipped => !string.IsNullOrEmpty(this.rootRuntimeItemIDEquipped.String);
        public IdString RootRuntimeItemIDEquipped => this.rootRuntimeItemIDEquipped;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        internal void Equip(Bag bag, IBone boneSource, RuntimeItem runtimeItem)
        {
            this.rootRuntimeItemIDEquipped = runtimeItem.RuntimeID;
            GameObject prefab = runtimeItem.Item.Equip.Prefab;
            
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;

            if (prefab == null) return;
            
            Character character = bag.Wearer.Get<Character>();
            IBone bone = this.m_Override ? this.m_OverrideBone : boneSource;
                
            if (character != null)
            {
                runtimeItem.PropInstance = character.Props.Attach(
                    bone, prefab, 
                    position, rotation
                );
            }
            else
            {
                Transform boneTransform = this.GetWearerBone(bag.Wearer, bone);
                if (boneTransform != null)
                {
                    if (runtimeItem.PropInstance != null)
                    {
                        UnityEngine.Object.Destroy(runtimeItem.PropInstance);
                    }

                    runtimeItem.PropInstance = UnityEngine.Object.Instantiate(
                        prefab, position, rotation, boneTransform
                    );
                }
            }

            if (runtimeItem.PropInstance == null) return;
            
            Prop prop = runtimeItem.PropInstance.Get<Prop>();
            if (prop != null) prop.Setup(runtimeItem);
        }
        
        internal void Unequip(Bag bag)
        {
            RuntimeItem runtimeItem = bag.Content.GetRuntimeItem(this.rootRuntimeItemIDEquipped);
            Character character = bag.Wearer.Get<Character>();

            if (runtimeItem != null)
            {
                if (character != null)
                {
                    character.Props.Remove(runtimeItem.Item.Equip.Prefab);
                }
                else
                {
                    if (runtimeItem.PropInstance != null)
                    {
                        UnityEngine.Object.Destroy(runtimeItem.PropInstance);
                    }
                }   
            }

            this.rootRuntimeItemIDEquipped = IdString.EMPTY;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private Transform GetWearerBone(GameObject wearer, IBone bone)
        {
            if (wearer == null) return null;
            
            Character character = wearer.Get<Character>();
            Animator animator = character != null 
                ? character.Animim.Animator 
                : wearer.Get<Animator>();
        
            return animator != null 
                ? bone.GetTransform(animator) 
                : wearer.transform;
        }
    }
}