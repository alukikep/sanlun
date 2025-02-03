using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    public float health;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Sprite initialSprite;

    public delegate void DeathEventHandler();
    public event DeathEventHandler OnDeath;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        initialSprite = spriteRenderer.sprite;
    }

    private void Update()
    {
        Die();
    }
    public void GetDamage(float pATK)
    {
        
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        if(animator!=null)
        {
            animator.Play("Hurt");
        }
      
        health = health - pATK;
       
        
    }

    private void Die()
    {
        if (health <= 0 && OnDeath != null)
            animator.Play("Die");
    }
    private void DestroyEnemy()
    {
        OnDeath();
    }
    private void SetSprite()
    {
        spriteRenderer.sprite = initialSprite;
        
    }
}
