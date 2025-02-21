using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttack : PlayerState
{
    public AirAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.audioController.PlaySfx(player.audioController.attack);
        isAttack =true;
    }

    public override void Exit()
    {
        base.Exit();
        isAttack=false;
    }

    public override void Update()
    {
        base.Update();
        if(player.jumpNumber==2)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
