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
        Collider2D[] Enemies = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius,LayerMask.GetMask("Enemy"));

        foreach(var hit in Enemies)
        {
            if(hit.GetComponent<EnemyHealth>() != null&&!hit.isTrigger)
            {
                if (player.blockBonus == 1)
                {
                    hit.GetComponent<EnemyHealth>().GetDamage(player.ATK);
                }
                if(player.blockBonus == 2)
                {
                    hit.GetComponent<EnemyHealth>().GetDamage(player.ATK*player.blockBonus);
                    player.blockBonus = 1;
                }
            }
        }
    }

    public void AttackEnd()
    {
        player.isAttack=false;      
    }

    public void BlockEnd()
    {
        player.isBlock=false;
    }

    public void BlockSuccessEnd()
    {
        player.isBlock = false;     
        player.blockSuc=false;
        player.blockBonus = 2;
    }
}
