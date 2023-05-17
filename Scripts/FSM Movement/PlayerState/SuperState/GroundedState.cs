using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;

    private bool JumpInput;

    public GroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {

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
        yInput = player.InputHandler.NormalizedInputY;
        JumpInput = player.InputHandler.JumpInput;

        if (JumpInput) {
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }
        yInput = player.InputHandler.NormalizedInputY;
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    public override void DoChecks() {
        base.DoChecks();
    }
}
