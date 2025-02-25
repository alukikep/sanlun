using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Grounded
{
    public Block(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.blockCoolTimer=player.blockCoolTime;
        player.isBlock=true;
        player.audioController.PlaySfx(player.audioController.blockStart);
    }

    public override void Exit()
    {
        base.Exit();
        player.isBlock = false;
    }

    public override void Update()
    {
        base.Update();
    }
}
