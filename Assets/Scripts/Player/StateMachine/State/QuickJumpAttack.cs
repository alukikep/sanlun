using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickJumpAttack : Air
{
    public QuickJumpAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.audioController.PlaySfx(player.audioController.attack);
        isAttack = true;
        player.attackTimer = player.attackTime/5;
    }

    public override void Exit()
    {
        base.Exit();
        isAttack = false;
    }

    public override void Update()
    {
        base.Update();
        if (player.jumpNumber == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
