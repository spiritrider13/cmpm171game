﻿using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Title("Motion Controller")]
    [Image(typeof(IconChip), ColorTheme.Type.Blue)]

    [Category("Motion Controller")]
    [Description(
        "Motion system that defines how the Character responds to external stimulus"
    )]

    [Serializable]
    public class UnitMotionController : TUnitMotion
    {
        // PUBLIC FIELDS: -------------------------------------------------------------------------
        
        [SerializeField] private float m_Speed = 4f;
        [SerializeField] private float m_Rotation = 1800f;

        [SerializeField] private float m_Mass = 80f;
        [SerializeField] private float m_Height = 2.0f;
        [SerializeField] private float m_Radius = 0.2f;

        [SerializeField] private float m_Gravity = -9.81f;
        [SerializeField] private float m_TerminalVelocity = -53f;

        [SerializeField] private MotionAcceleration m_Acceleration;
        [SerializeField] private MotionJump m_Jump;
        
        // INTERFACE PROPERTIES: ------------------------------------------------------------------
        
        public override float JumpForce
        {
            get => this.m_Jump.JumpForce;
            set => this.m_Jump.JumpForce = value;
        }

        public override float LinearSpeed
        {
            get => this.m_Speed;
            set => this.m_Speed = value;
        }

        public override float AngularSpeed
        {
            get => this.m_Rotation;
            set => this.m_Rotation = value;
        }

        public override float Mass
        {
            get => this.m_Mass;
            set => this.m_Mass = value;
        }

        public override float Height
        {
            get => this.m_Height;
            set => this.m_Height = value;
        }

        public override float Radius
        {
            get => this.m_Radius;
            set => this.m_Radius = value;
        }

        public override bool CanJump
        {
            get => this.m_Jump.CanJump && !this.Character.Busy.AreLegsBusy;
            set => this.m_Jump.CanJump = value;
        }

        public override int AirJumps
        {
            get => m_Jump.AirJumps;
            set => m_Jump.AirJumps = value;
        }

        public override float Gravity
        {
            get => this.m_Gravity;
            set => this.m_Gravity = value;
        }

        public override float TerminalVelocity
        {
            get => this.m_TerminalVelocity;
            set => this.m_TerminalVelocity = value;
        }

        public override float JumpCooldown
        {
            get => this.m_Jump.JumpCooldown;
            set => this.m_Jump.JumpCooldown = value;
        }

        public override bool UseAcceleration => this.m_Acceleration.UseAcceleration;

        public override float Acceleration
        {
            get => this.m_Acceleration.Acceleration;
            set => this.m_Acceleration.Acceleration = value;
        }

        public override float Deceleration
        {
            get => this.m_Acceleration.Deceleration;
            set => this.m_Acceleration.Deceleration = value;
        }
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public UnitMotionController() : base()
        {
            this.m_Acceleration = new MotionAcceleration();
            this.m_Jump = new MotionJump();
        }
    }
}
