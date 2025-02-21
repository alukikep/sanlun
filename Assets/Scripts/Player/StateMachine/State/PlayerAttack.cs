using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerAttack : Grounded
{
    public PlayerAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.audioController.PlaySfx(player.audioController.attack);
        isAttack = true;
    }

    public override void Exit()
    {
        base.Exit();
        isAttack=false;
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(0, 0);      
    }
}
