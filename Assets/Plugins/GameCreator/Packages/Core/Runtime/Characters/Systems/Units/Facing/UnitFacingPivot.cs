﻿using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    [Title("Pivot")]
    [Image(typeof(IconRotationYaw), ColorTheme.Type.Green)]
    
    [Category("Pivot")]
    [Description("Rotates the Character towards the direction it moves")]

    [Serializable]
    public class UnitFacingPivot : TUnitFacing
    {
        private enum DirectionFrom
        {
            MotionDirection,
            DriverDirection
        }

        [SerializeField] private DirectionFrom m_DirectionFrom = DirectionFrom.MotionDirection;
        
        protected override Vector3 GetDefaultDirection()
        {
            Vector3 driverDirection = Vector3.Scale(
                this.m_DirectionFrom switch
                {
                    DirectionFrom.MotionDirection => this.Character.Motion.MoveDirection,
                    DirectionFrom.DriverDirection => this.Character.Driver.WorldMoveDirection,
                    _ => throw new ArgumentOutOfRangeException()
                },
                Vector3Plane.NormalUp
            );
            
            return this.DecideDirection(driverDirection);
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => "Pivot";
    }
}