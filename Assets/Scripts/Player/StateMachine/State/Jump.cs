using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : Air
{
    public Jump(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();


        if (player.jumpNumber==0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
