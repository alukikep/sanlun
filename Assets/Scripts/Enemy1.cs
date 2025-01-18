using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    public Transform player;

    public float stopDistanceX;//敌人移动到距主角一定距离停下
    [SerializeField] private float moveSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float directionX = player.position.x - transform.position.x;
        if (Mathf.Abs(directionX) > stopDistanceX) {
            float moveDirection = directionX > 0 ? 1 : -1;
            rigidbody2D.velocity = new Vector2(moveDirection * moveSpeed, rigidbody2D.velocity.y);
        }
        else
        {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }
        
    }
}
