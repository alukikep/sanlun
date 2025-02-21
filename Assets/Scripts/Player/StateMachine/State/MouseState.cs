using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseState : Grounded
{
    public MouseState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.jumpForce = 8;
        player.transform.position = player.transform.position - new Vector3(0, 0.8f, 0);
        player.capsuleCollider2D.size = new Vector2(0.5f, 0.5f);
        isMouse=true;
    }

    public override void Exit()
    {
        base.Exit();
        player.transform.position = player.transform.position + new Vector3(0, 0.7f, 0);
        player.jumpForce = 14;
        player.capsuleCollider2D.size = new Vector2(0.4f, 1.7f);
        isMouse=false;
 
    }

    public override void Update()
    {
        base.Update();
    }
}
