using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    public float health;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void GetDamage(float pATK)
    {
        animator.Play("Hurt");
        health = health - pATK;
    }


}
