using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : AbilityState
{
    public PlayerJump(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {

    }

    public override void Enter() {
        base.Enter();
        player.SetVelocityY(playerData.JumpVelocity);
        isAbilityDone = true;
    }
}
