using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public Animator anim { get; private set; }
   public PlayerStateMachine StateMachine {  get; private set; }

    public Idle idleState { get; private set; }
    public Move moveState { get; private set; }

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        idleState = new Idle(this, StateMachine, "Idle");
        moveState = new Move(this, StateMachine, "Move");
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();

        StateMachine.Initialize(idleState);
    }

    private void Update()
    {
        StateMachine.currentState.Update();
    }
}
