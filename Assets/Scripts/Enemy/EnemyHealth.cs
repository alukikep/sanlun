using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    public float health;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Sprite initialSprite;
    private Color originColor;
    [SerializeField]private float flashTime;
    public GameObject hurtEffect;
    public GameObject dieEffect;

    public GameObject potion;
    private GameObject audio;
    AudioController audioController;

    public delegate void DeathEventHandler();
    public event DeathEventHandler OnDeath;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        initialSprite = spriteRenderer.sprite;
        originColor = spriteRenderer.color;
        audio = GameObject.FindGameObjectWithTag("Audio");
        audioController = audio.GetComponent<AudioController>();
    }

    private void Update()
    {
        Die();
    }
    public void GetDamage(float pATK)
    {
        
        audioController.PlaySfx(audioController.enemyHurt);
        Instantiate(hurtEffect, transform.position, Quaternion.identity);
        health = health - pATK;
        Flash(flashTime);

    }

    private void Die()
    {
        if (health <= 0 && OnDeath != null)
        {
            int num = Random.Range(0, 100);
            if(num<5&&num>=0)
            {
                GameObject PPotion = Instantiate(potion, transform.position, Quaternion.identity);
            }
           
            audioController.PlaySfx(audioController.enemyDie);
            Instantiate(dieEffect, transform.position, Quaternion.identity);
            SetSprite();
            DestroyEnemy();
        }

    }
    private void DestroyEnemy()
    {
        OnDeath();
    }
    private void SetSprite()
    {
        spriteRenderer.sprite = initialSprite;
        
    }

    private void Flash(float time)
    {
        spriteRenderer.color = Color.red;
        Invoke("ResetColor", time);
    }

    private void ResetColor()
    {
        spriteRenderer.color= originColor;
    }

}
