using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : PlayerState
{
    public bool isMouse=false;
    public Grounded(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.jumpNumber = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();



        if (Input.GetKeyDown(KeyCode.J) && isMouse == false && isAttack == false && player.attackTimer < 0 && Time.timeScale == 1)
        {
            stateMachine.ChangeState(player.attackState);
        }
        else if (Input.GetKeyDown(KeyCode.J) && isMouse == false && isAttack == false && player.attackTimer < 0 && Time.timeScale == 0.2f)
        {
            stateMachine.ChangeState(player.quickAttackState);
        }

            if (Input.GetKeyDown(KeyCode.Space)&&player.jumpNumber<player.jumpLimit&&isMouse==false)
        {
            player.audioController.PlaySfx(player.audioController.jump);
            player.jumpNumber--;
            stateMachine.ChangeState(player.jumpState);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && player.jumpNumber <player.jumpLimit && isMouse == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
            player.jumpNumber++;
        }

        if (Input.GetKeyDown(KeyCode.V)&&isMouse==false&&player.isratTransformEnabled==true)
        {
            stateMachine.ChangeState(player.mouseState);  
        }
        else if(Input.GetKeyDown(KeyCode.V) && isMouse == true&&player.CanRestore()&&player.isratTransformEnabled==true)
        {
                stateMachine.ChangeState(player.idleState);        
        }

        if(Input.GetKeyDown(KeyCode.K)&&player.blockCoolTimer<0)
        {
            stateMachine.ChangeState(player.blockState);
        }

        if(Input.GetKeyDown(KeyCode.Q)&&isMouse==false)
        {
            stateMachine.ChangeState(player.retreatState);
        }
    }
}
