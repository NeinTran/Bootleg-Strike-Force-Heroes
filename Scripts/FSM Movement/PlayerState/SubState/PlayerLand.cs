using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLand : GroundedState
{
    public PlayerLand(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if(xInput != 0) {
            stateMachine.ChangeState(player.MoveState);
        } else if (isAnimationFinished) {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
