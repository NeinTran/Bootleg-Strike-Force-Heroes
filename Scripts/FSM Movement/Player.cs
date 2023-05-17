using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdle IdleState { get; private set; }
    public PlayerMove MoveState { get; private set; }
    public PlayerJump JumpState { get; private set; }
    public PlayerInAir InAirState { get; private set; }
    public PlayerLand LandState { get; private set; }
    #endregion

    #region Components
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    [SerializeField] private Transform groundCheck;

    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }

    [SerializeField] private PlayerData playerData;

    private Vector2 workSpace;

    private void Awake() {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdle(this, StateMachine, playerData, "Idle");
        MoveState = new PlayerMove(this, StateMachine, playerData, "Move");
        JumpState = new PlayerJump(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAir(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLand(this, StateMachine, playerData, "Land");
    }

    private void Start() {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
        StateMachine.Initialize(IdleState);
        FacingDirection = 1;
    }

    private void Update() {
        CurrentVelocity = rb.velocity;
        StateMachine.currentState.LogicUpdate();
    }

    private void FixedUpdate() {
        StateMachine.currentState.PhysicsUpdate();
    }

    public void SetVelocityX(float velocity) {
        workSpace.Set(velocity, CurrentVelocity.y);
        rb.velocity = workSpace;
        CurrentVelocity = workSpace;
    }

    public void SetVelocityY(float velocity) {
        workSpace.Set(CurrentVelocity.x, velocity);
        rb.velocity = workSpace;
        CurrentVelocity = workSpace;
    }

    public bool CheckIfTouchingGround() {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.WhatIsGround);
    }

    public void CheckIfShouldFlip(int xInput) {
        if (xInput != 0 && xInput != FacingDirection) {
            Flip();
        }
    }

    private void Flip() {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void AnimationTrigger() {
        StateMachine.currentState.AnimationTrigger();
    }

    private void AnimationFinishTrigger() {
        StateMachine.currentState.AnimationFinishTrigger();
    }

}
