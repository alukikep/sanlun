using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Grounded
{
    public Move(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        

        if(xInput==0)
            stateMachine.ChangeState(player.idleState);
    }
}
