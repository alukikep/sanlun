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

       

        if (Input.GetKeyDown(KeyCode.B)&&isBat==false&&player.isbatTransformEnabled==true)
        {
            stateMachine.ChangeState(player.batState);
            
        }
        else if(Input.GetKeyDown(KeyCode.B)&&isBat==true&&player.isbatTransformEnabled==true)
        {
            stateMachine.ChangeState(player.fallState);
            
        }

        if(Input.GetKeyDown(KeyCode.J)&&isBat==false&&isAttack==false&&player.attackTimer<0&&Time.timeScale==1)
        {
            stateMachine.ChangeState(player.airAttackState);
        }
        else if(Input.GetKeyDown(KeyCode.J) && isBat == false && isAttack == false && player.attackTimer < 0 && Time.timeScale == 0.2f)
        {
            stateMachine.ChangeState(player.quickJumpState);
        }

        if(player.jumpNumber==0)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if(Input.GetKeyDown(KeyCode.Q)&&isBat ==false &&isAttack==false&&player.ishighJumpEnabled==true)
        {
            stateMachine.ChangeState(player.highJumpState);
        }

        if (player.jumpNumber < player.jumpLimit && Input.GetKeyDown(KeyCode.Space))
        {
            player.audioController.PlaySfx(player.audioController.jump);
            player.jumpNumber++;
            stateMachine.ChangeState(player.jumpState);
        }
    }
}
