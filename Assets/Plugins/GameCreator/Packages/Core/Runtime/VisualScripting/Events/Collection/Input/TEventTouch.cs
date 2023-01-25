using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace GameCreator.Runtime.VisualScripting
{
    [Parameter(
        "Min Distance", 
        "If set to None, the touch input acts globally. If set to Game Object, the event " +
        "only fires if the target object is within a certain radius"
    )]
    
    [Keywords("Finger", "Press", "Click")]
    
    [Serializable]
    public abstract class TEventTouch : Event
    {
        private static readonly List<RaycastResult> HITS = new List<RaycastResult>();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private CompareMinDistanceOrNone m_MinDistance = new CompareMinDistanceOrNone();
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        protected internal override void OnUpdate(Trigger trigger)
        {
            base.OnUpdate(trigger);
            
            if (!this.InteractionSuccessful(trigger)) return;
            if (IsPointerOverUI()) return;
            if (!this.m_MinDistance.Match(trigger.transform, new Args(this.Self))) return;
            
            _ = this.m_Trigger.Execute(this.Self);
        }
        
        // ABSTRACT METHODS: ----------------------------------------------------------------------

        protected abstract bool InteractionSuccessful(Trigger trigger);
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected bool WasTouchedThisFrame
        {
            get
            {
                TouchControl touch = Touchscreen.current?.primaryTouch;
                return touch != null && touch.phase.ReadValue() == TouchPhase.Began;
            }
        }
        
        protected bool WasReleasedThisFrame
        {
            get
            {
                TouchControl touch = Touchscreen.current?.primaryTouch;
                return touch != null && touch.phase.ReadValue() == TouchPhase.Ended;
            }
        }
        
        protected bool IsPressed
        {
            get
            {
                TouchControl touch = Touchscreen.current?.primaryTouch;
                return touch != null && touch.IsPressed();
            }
        }
        
        protected int TapCount
        {
            get
            {
                TouchControl touch = Touchscreen.current?.primaryTouch;
                return touch != null ? touch.tapCount.ReadValue() : 0;
            }
        }

        // PRIVATE STATIC METHODS: ----------------------------------------------------------------

        private static bool IsPointerOverUI()
        {
            if (EventSystem.current == null) return false;
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = Touchscreen.current.primaryTouch.position.ReadValue()
            };
            
            EventSystem.current.RaycastAll(eventDataCurrentPosition, HITS);
            HITS.Sort(CompareHitDistance);

            return HITS.Count != 0 && HITS[0].gameObject.layer == UIUtils.LAYER_UI;
        }

        private static int CompareHitDistance(RaycastResult x, RaycastResult y)
        {
            return x.distance.CompareTo(y.distance);
        }

        // GIZMOS: --------------------------------------------------------------------------------

        protected internal override void OnDrawGizmosSelected(Trigger trigger)
        {
            base.OnDrawGizmosSelected(trigger);
            this.m_MinDistance.OnDrawGizmos(
                trigger.transform,
                new Args(trigger.gameObject)
            );
        }
    }
}