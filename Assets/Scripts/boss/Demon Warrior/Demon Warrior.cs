using UnityEngine;

public class DemonWarrior : MonoBehaviour
{
    public Animator animator;
    public float attackCooldown = 2.0f;
    private float nextAttackTime = 0.0f;
    public Transform playerPosition;
    public GameObject[] spawnPoints;
    public GameObject impPrefab; // С��ħԤ����
    public GameObject fireballPrefab; // ����Ԥ����
    public GameObject demonPortalPrefab; // ��ħ֮��Ԥ����
    public int maxHealth = 3; // �������ֵ����Ӧ�׶���
    private int currentHealth = 3; // ��ǰ����ֵ
    private int phase = 1;

    void Update()
    {
        if (Time.time > nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            PerformAttack();
        }
    }

    void PerformAttack()
    {
        switch (phase)
        {
            case 1:
                Phase1Attack();
                break;
            case 2:
                Phase2Attack();
                break;
            case 3:
                Phase3Attack();
                break;
        }
    }

    void Phase1Attack()
    {
        int attackType = Random.Range(0, 3);
        switch (attackType)
        {
            case 0:
                SwipeAttack();
                break;
            case 1:
                JumpAttack();
                break;
            case 2:
                SummonImps();
                break;
        }
    }

    void Phase2Attack()
    {
        int attackType = Random.Range(0, 3);
        switch (attackType)
        {
            case 0:
                FireballAttack();
                break;
            case 1:
                SpinAttack();
                break;
            case 2:
                TeleportAttack();
                break;
        }
    }

    void Phase3Attack()
    {
        ScreenAttack();
        SummonDemonicPortal();
        FinalStrike();
    }

    void SwipeAttack()
    {
        animator.SetTrigger("Swipe");
    }

    void JumpAttack()
    {
        animator.SetTrigger("Jump");
    }

    void SummonImps()
    {
        animator.SetTrigger("Summon");
        foreach (var spawnPoint in spawnPoints)
        {
            Instantiate(impPrefab, spawnPoint.transform.position, Quaternion.identity);
        }
    }

    void FireballAttack()
    {
        animator.SetTrigger("Fireball");
        Instantiate(fireballPrefab, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
    }

    void SpinAttack()
    {
        animator.SetTrigger("Spin");
    }

    void TeleportAttack()
    {
        animator.SetTrigger("Teleport");
        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        Instantiate(fireballPrefab, randomPos, Quaternion.identity);
    }

    void ScreenAttack()
    {
        animator.SetTrigger("ScreenAttack");
    }

    void SummonDemonicPortal()
    {
        animator.SetTrigger("SummonPortal");
        Instantiate(demonPortalPrefab, transform.position + new Vector3(0, -2, 0), Quaternion.identity);
    }

    void FinalStrike()
    {
        animator.SetTrigger("FinalStrike");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ��ҽ��빥����Χ����������
            animator.SetTrigger("Attack");
        }
    }

    // �׶�ת��
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            AdvancePhase();
            currentHealth = maxHealth; // ��������ֵ
        }
    }

    private void AdvancePhase()
    {
        phase++;
        if (phase > 3)
        {
            phase = 3; // ��������׶�
        }
        Debug.Log("Phase: " + phase);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPosition = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPosition = null;
        }
    }
}