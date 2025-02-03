using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour//用来互动之后摧毁特定砖块
{
    public float damage;
    public float speed;
    public float slowPercent;
    public float slowDuration;
    private Player player;
    private GameObject _Player;
    private Rigidbody2D rb;
    private void Start()
    {
        _Player = GameObject.Find("Player");  
        rb = GetComponent<Rigidbody2D>();
        Vector2 direction = (_Player.transform.position-transform.position).normalized;
        rb.velocity = direction * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_Player != null)
        {
            player = collision.GetComponent<Player>();
            player.getSlowed(slowPercent, slowDuration);
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Player>().GetDamage(damage);
            }
        }
       
    }
}
