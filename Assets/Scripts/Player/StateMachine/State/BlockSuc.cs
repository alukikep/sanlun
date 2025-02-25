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
        player.audioController.PlaySfx(player.audioController.block);
        GameObject BlockEffect = MonoBehaviour.Instantiate(player.blockEffect, player.transform.position, Quaternion.identity);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
