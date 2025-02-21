using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSuc : Block
{
    public BlockSuc(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isBlock = true;
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
