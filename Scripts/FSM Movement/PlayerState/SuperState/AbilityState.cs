using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityState : PlayerState
{
    protected bool isAbilityDone;

    private bool IsGrounded;

    public AbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {

    }

    public override void Enter() {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        if(isAbilityDone) {
            if(IsGrounded && player.CurrentVelocity.y < 0.01f) {
                stateMachine.ChangeState(player.IdleState);
            }
            else {
                stateMachine.ChangeState(player.InAirState);
            }
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
