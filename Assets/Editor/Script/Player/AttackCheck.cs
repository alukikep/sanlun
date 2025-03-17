using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackCheck : MonoBehaviour
{
    public Player player;
    public PlayerState state;
    
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

        Collider2D[] Enemies = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius, LayerMask.GetMask("Enemy"));
        Collider2D[] bricks = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius, LayerMask.GetMask("DestructibleBricks"));
        foreach (var hit in Enemies)
        {
            if(hit.GetComponent<EnemyHealth>() != null&&!hit.isTrigger&&hit.GetComponent<EnemyHealth>().health>0)
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
        foreach (var hit in bricks)
        {
            if(hit.GetComponent<DestructibleBricks>() != null&&!hit.isTrigger)
            {
                if (player.blockBonus == 1)
                {
                    hit.GetComponent<DestructibleBricks>().GetDamage(player.ATK);
                }
                if(player.blockBonus == 2)
                {
                    hit.GetComponent<DestructibleBricks>().GetDamage(player.ATK*player.blockBonus);
                    player.blockBonus = 1;
                }
            }
        }
       
           
        
    }

    public void GroundAttackEnd()
    {
        player.StateMachine.ChangeState(player.idleState);
    }

    public void AirAttackEnd()
    {
        player.StateMachine.ChangeState(player.fallState);
    }

    public void BlockEnd()
    {
        player.StateMachine.ChangeState(player.idleState);
    }

    public void BlockSuccessEnd()
    {
        player.StateMachine.ChangeState(player.idleState);
        player.blockBonus = 1;
    }
}
