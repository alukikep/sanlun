using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
   protected PlayerStateMachine stateMachine;
    protected PlayerTest player;

    private string animBoolName;

    public PlayerState(PlayerTest _player,PlayerStateMachine _stateMachine,string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
    }
    public virtual void Update()
    {
        Debug.Log("i am in" + animBoolName);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
}
