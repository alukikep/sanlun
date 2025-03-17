using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy2 : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    public Transform player;

    [SerializeField] private float wanderRadius;// 左右游荡的范围
    [SerializeField] private float moveSpeed;// 左右游荡的速度
    private float leftBoundary;
    private float rightBoundary;
    private bool isMovingRight = true; // 用于记录当前移动方向
    private bool FaceRight;

    public float cooldown;//发射子弹间隔时间
    private float cooldownTimer;//记录冷却时间
    public GameObject bullet;
    private GameObject[] bullets = new GameObject[4];//记录4颗子弹
    private int[] angles = { 45, 135, 225, 315 }; // 预先计算的子弹角度

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        cooldownTimer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) { return; }
        else
        {
            leftBoundary = player.position.x - wanderRadius;
            rightBoundary = player.position.x + wanderRadius;
            // 检查是否到达边界并反转方向
            if (transform.position.x <= leftBoundary)
            {
                isMovingRight = true;
            }
            else if (transform.position.x >= rightBoundary)
            {
                isMovingRight = false;
            }
        }
        // 根据当前方向设置速度
        float direction = isMovingRight ? 1 : -1;
        rigidbody2D.velocity = new Vector2(direction * moveSpeed, 0);

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0)
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = Instantiate(bullet, transform.position, Quaternion.identity);
                bullets[i].GetComponent<Bullet1>().angle = angles[i];
            }
            cooldownTimer = cooldown;
        }

        Flip();
    }

    void Flip()
    {
        if (!FaceRight && rigidbody2D.velocity.x > 0)
        {
            transform.Rotate(0, 180, 0);
            FaceRight = true;
        }
        if (FaceRight && rigidbody2D.velocity.x < 0)
        {
            transform.Rotate(0, 180, 0);
            FaceRight = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }
}
