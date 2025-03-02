using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDevil : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    public Transform player; // ��Ҷ����Transform���
    public float cooldown;//�����ӵ����ʱ��
    private float cooldownTimer;//��¼��ȴʱ��
    public GameObject iceBullet;
    private Animator animator;
    public float retreatDistance = 5f;

    [SerializeField] private float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        cooldownTimer = 1;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            rigidbody2D.velocity= Vector2.zero;
            return;
        }
        Vector2 dirToPlayer = (player.position - transform.position).normalized;
        if (dirToPlayer.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        HandleMovement(dirToPlayer);

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0 && player != null)
        {
            animator.Play("Attack");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float ATK = 40;
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().GetDamage(ATK);
        }
    }
    private void HandleMovement(Vector2 vector2)
    {
        float currentDistance = Vector2.Distance(transform.position, player.position);

        // �����Ҿ���С�� retreatDistance��Զ�����
        if (currentDistance < retreatDistance)
        {
            rigidbody2D.velocity = -vector2 * moveSpeed; // Զ�����
        }
        else if(currentDistance > retreatDistance+1)
        {
            rigidbody2D.velocity = vector2*moveSpeed; // ֹͣ�ƶ�
        }
        else 
        {
            rigidbody2D.velocity = Vector2.zero;
        }

    }

    private void Attack()
    {
        if (player != null)
        {
            cooldownTimer = cooldown;
            Vector3 height = new Vector3(0, 4, 0);
            GameObject IceBullet = Instantiate(iceBullet, player.position + height, Quaternion.identity);
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
