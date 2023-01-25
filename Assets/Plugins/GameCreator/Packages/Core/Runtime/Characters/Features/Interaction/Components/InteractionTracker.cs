using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    
    [Serializable]
    public class InteractionTracker : MonoBehaviour, IInteractive
    {
        private const HideFlags FLAGS = HideFlags.HideAndDontSave | HideFlags.HideInInspector;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Vector3 m_LastPosition;
        
        [NonSerialized] private bool m_IsInteracting;
        [NonSerialized] private Character m_Character;

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<Character, IInteractive> EventInteract;
        public event Action<Character, IInteractive> EventStop;
        
        // INITIALIZE METHODS: --------------------------------------------------------------------
        
        public static InteractionTracker Require(GameObject target)
        {
            InteractionTracker tracker = target.Get<InteractionTracker>();
            return tracker != null ? tracker : target.Add<InteractionTracker>();
        }

        private void Awake()
        {
            this.hideFlags = FLAGS;
        }

        private void OnEnable()
        {
            this.m_LastPosition = this.transform.position;
            SpatialHashInteractions.Get.Insert(this);
        }

        private void OnDisable()
        {
            SpatialHashInteractions.Get.Remove(this);
        }

        // SPATIAL HASH INTERFACE: ----------------------------------------------------------------

        int ISpatialHash.UniqueCode => this.GetInstanceID();

        Vector3 ISpatialHash.Position => this.transform.position;

        // INTERACTIVE INTERFACE: -----------------------------------------------------------------
        
        GameObject IInteractive.Instance => this.gameObject;
        
        bool IInteractive.IsInteracting => this.m_IsInteracting;
        
        void IInteractive.Interact(Character character)
        {
            if (this.m_IsInteracting) return;
            
            this.m_IsInteracting = true;
            this.m_Character = character;
            
            this.EventInteract?.Invoke(character, this);
        }

        void IInteractive.Stop()
        {
            if (!this.m_IsInteracting) return;
            
            this.m_IsInteracting = false;
            this.EventStop?.Invoke(this.m_Character, this);
        }
    }
}