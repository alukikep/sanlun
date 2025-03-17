using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliarBullet : MonoBehaviour
{
    public Transform attackCheck;
    public float damage;
    public float attackRadius;
    public float destroyTime;
    private Player playerScript;
    private GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        damage = playerScript.ATK;
        AttackTrigger();
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0)
        {
            Destroy(gameObject);
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
