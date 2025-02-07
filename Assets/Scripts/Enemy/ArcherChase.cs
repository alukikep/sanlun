using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherChase : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    public Transform player; // 玩家对象的Transform组件
    public float cooldown;//发射子弹间隔时间
    private float cooldownTimer;//记录冷却时间
    public GameObject bullet;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        cooldownTimer = 1;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0 && player != null)
        {
            animator.Play("Attack");
        }
    }

    private void Shoot()
    {
        GameObject b1 = Instantiate(bullet, transform.position, Quaternion.identity);
        Vector3 direction = player.position - transform.position;//计算Archer到玩家的向量
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//计算角度（以度为单位）
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        b1.transform.rotation = rotation;
        b1.GetComponent<ChaseBullet>().angle = angle;
        cooldownTimer = cooldown;
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
