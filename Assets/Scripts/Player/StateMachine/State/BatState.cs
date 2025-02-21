using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatState : Air
{
    public BatState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.rigidbody2D.drag = 10;
        player.capsuleCollider2D.size = new Vector2(0.8f, 0.8f);
        isBat = true;

    }

    public override void Exit()
    {
        base.Exit();
        player.rigidbody2D.drag = 1;

        player.capsuleCollider2D.size = new Vector2(0.4f, 1.7f);
        isBat = false;
    }

    public override void Update()
    {
        base.Update();

    }
}
