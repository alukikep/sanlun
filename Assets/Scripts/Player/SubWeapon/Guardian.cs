using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardian : MonoBehaviour
{
    public float damage;
    public float speed;
    public float radius;
    public float destroyTime;
    public float totalAngle;
    public float neededMana;
    public float rotateSpeed;
    private float rotationAmount;
    private float initialAngle;
    public Transform attackCheck;
    public float attackRadius;
    private Transform Player;
    private Rigidbody2D rb;
    private void Start()
    {
        Player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        Vector2 direction = (transform.position - Player.transform.position);
        initialAngle = Mathf.Atan2(direction.y, direction.x);
    }
    private void Update()
    {
        rotate();
        AttackTrigger();
        destroyTime -= Time.deltaTime;
       
        if (destroyTime <= 0)
        {
            Destroy(gameObject);
        }
       

    }
    public void rotate()
    {
        float angle = Time.deltaTime * speed; 
        totalAngle += angle; 
        float x = Player.position.x+radius*Mathf.Cos(initialAngle+totalAngle);
        float y = Player.position.y+radius*Mathf.Sin(initialAngle+totalAngle);
        transform.position = new Vector3(x, y, transform.position.z);
        rotationAmount = (rotateSpeed * Time.deltaTime) % 360;
        transform.Rotate(transform.rotation.x, transform.rotation.y, rotationAmount);

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
