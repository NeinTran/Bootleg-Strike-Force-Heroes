using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAir : PlayerState
{
    private bool IsGrounded;
    private int xInput;

    public PlayerInAir(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {

    }

    public override void Enter() {
        base.Enter();
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        xInput = player.InputHandler.NormalizedInputX;
        if(IsGrounded && player.CurrentVelocity.y < 0.01f) {
            stateMachine.ChangeState(player.LandState);
        } else {
            player.CheckIfShouldFlip(xInput);
            player.SetVelocityX(playerData.movementVelocity * xInput);

            player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    public override void DoChecks() {
        base.DoChecks();
        IsGrounded = player.CheckIfTouchingGround();
    }
}
    
