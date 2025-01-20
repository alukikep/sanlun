using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackCheck : MonoBehaviour
{
    public Player player;
    
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(player.attackCheck.position,player.attackRadius);
    }

    private void AttackTrigger()
    {
        Collider2D[] Enemies = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);

        foreach(var hit in Enemies)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }

    public void AttackEnd()
    {
        player.isAttack=false;      
    }
}
