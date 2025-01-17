using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Familiar : MonoBehaviour
{

    private Rigidbody2D rb;
    public float attackRange;
    public float attackInterval;
    public float detectionRadius;
    public float farmiliarBulletSpeed;
    private float attackTimer;
    private Transform playerPosition;
    private Transform targetEnemy;
    public GameObject familiarBulletPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer -= Time.deltaTime;
        getCurrentEnemyTarget();
        attack();

    }
    private void getCurrentEnemyTarget()//检测最近的在Enemy层的敌人
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, LayerMask.GetMask("Enemy"));
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach(Collider2D enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }
        if(closestEnemy!=null&&closestDistance<=attackRange)
        {
            targetEnemy = closestEnemy;
        }
    }
    private void attack()//对最近的敌人进行攻击
    {
        if(targetEnemy!=null&&attackTimer<=0)
        {
            GameObject farmiliarBullet = Instantiate(familiarBulletPrefab, transform.position, Quaternion.identity);
            Vector2 direction = (targetEnemy.position - transform.position).normalized;
            Rigidbody2D rb = farmiliarBullet.GetComponent<Rigidbody2D>();
            rb.velocity = direction * farmiliarBulletSpeed;
            attackTimer = attackInterval;
        }
        
    }
}
