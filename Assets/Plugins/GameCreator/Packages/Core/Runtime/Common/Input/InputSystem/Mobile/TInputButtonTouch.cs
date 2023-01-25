using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace GameCreator.Runtime.Common
{
    [Keywords("Finger", "Touch", "Press", "Tap")]
    
    [Serializable]
    public abstract class TInputButtonTouch : TInputButton
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public override bool Active => true;

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
    }
}