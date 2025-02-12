using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public float speed;
    public float angle;
    public float damage;
    public float destroyTime;
    public float neededMana;
    public Transform attackCheck;
    public float rotateSpeed;
    private float rotationAmount;
    public float attackRadius;
    private GameObject _player;
    private Player player;
    private SpriteRenderer _spriteRenderer;

    private Rigidbody2D rb;
    private void Start()
    {
        _player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        player = _player.GetComponent<Player>();
        ThrowAxe();
        _spriteRenderer = GetComponent<SpriteRenderer>();
       
       
    }

    private void Update()
    {
        AttackTrigger();
        destroyTime -= Time.deltaTime;
        rotationAmount = (rotateSpeed * Time.deltaTime) % 360;
        if (rb.velocity.x > 0)
        {
            transform.Rotate(transform.rotation.x, transform.rotation.y, -rotationAmount);
            _spriteRenderer.flipX = false;
        }
        else
        {
            transform.Rotate(transform.rotation.x, transform.rotation.y, rotationAmount);
            _spriteRenderer.flipX = true;
        }
        if (destroyTime<=0)
        {
            Destroy(gameObject);
        }
    }
    public void ThrowAxe()
    {
        
        
        if(player.faceRight==true)
        {
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            rb.velocity = direction * speed;
        }
        else 
        {
            Vector2 direction = new Vector2(-Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            rb.velocity = direction * speed; 
        }
        


    }
    private void AttackTrigger()
    {
        Collider2D[] Enemies = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, LayerMask.GetMask("Enemy"));

        foreach (var hit in Enemies)
        {
            if (hit.GetComponent<EnemyHealth>() != null && !hit.isTrigger)
            {
              
                    hit.GetComponent<EnemyHealth>().GetDamage(damage);
                Destroy(gameObject);
                
            }
        }




    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
}
