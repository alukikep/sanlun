using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
   protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;

    protected float xInput;
    private string animBoolName;
    public bool isAttack;
    public bool isRetreat;

    public PlayerState(Player _player,PlayerStateMachine _stateMachine,string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb=player.rigidbody2D;
    }
    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        player.anim.SetFloat("yVelocity",rb.velocity.y);
        if (isRetreat == false)
        {
            player.SetVelocity(xInput * player.speedRate, rb.velocity.y);
        }
        player.xSpeed = xInput;

        if (xInput > 0)
        {
            player.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        if (xInput < 0)
        {
            player.transform.rotation = new Quaternion(0, 180, 0, 0);
        }

    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
}
