using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : PlayerState
{
    public bool isBat=false;
    public Air(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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


        if (Input.GetKeyDown(KeyCode.B)&&isBat==false)
        {
            stateMachine.ChangeState(player.batState);
            
        }
        else if(Input.GetKeyDown(KeyCode.B)&&isBat==true)
        {
            stateMachine.ChangeState(player.fallState);
            
        }

        if(Input.GetKeyDown(KeyCode.J)&&isBat==false&&isAttack==false)
        {
            stateMachine.ChangeState(player.airAttackState);
        }

        
    }
}
