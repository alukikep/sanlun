using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retreat : Grounded
{
    private float Timer;
    private float time=0.1f;
    public Retreat(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        Timer = time;
        base.Enter();
        isRetreat = true;
       
        if(player.faceRight==true)
        {
            rb.velocity = new Vector2(-20, 0);
        }
        else
        {
            rb.velocity = new Vector2(20, 0);
        }
    }

    public override void Exit()
    {
        base.Exit();
        isRetreat = false;
    }

    public override void Update()
    {
        base.Update();
        Timer-=Time.deltaTime;
        if(Timer<0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
