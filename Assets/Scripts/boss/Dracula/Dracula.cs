using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Dracula : MonoBehaviour
{
    public Vector2 areaCenter;
    public Transform player;
    public Vector2 areaSize;
    public bool faceRight;
    private Rigidbody2D rb;
    [SerializeField] private float rushSpeed;
    private Animator animator;
    private EnemyHealth enemyHealth;
    [Header("子弹")]
    public GameObject fireBall;
    public GameObject hugeFireBall;
    public GameObject lightRain;

    [Header("火球间距")]
    [SerializeField]private float littleSpacing;
    [SerializeField] private float hugeSpacing;
    [SerializeField]private float lightRainSpacing;

    [Header("间歇时间")]
    [SerializeField] private float attackEnd;
    [SerializeField] private float transEnd;

    private AudioController _audioController;
    private bool BGM;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        _audioController = GameObject.Find("Player").GetComponentInChildren<AudioController>();
    }
    private void Update()
    {
        Flip();

        if(Input.GetKeyDown(KeyCode.O))
        {
            FireBall();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            HugeFireBalluUP();
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            HugeFireBallDown();
        }
        if(Input.GetKeyDown (KeyCode.L))
        {
            LightRain();
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            Rush();
        }

        if (player != null && BGM == false)
        {
            _audioController.BGM.clip = _audioController.Dracula;
            _audioController.BGM.Play();
            BGM = true;
        }

        if (enemyHealth.health <= 0)
        {
            _audioController.BGM.clip = _audioController.StageClear;
            _audioController.BGM.PlayOneShot(_audioController.StageClear);
        }
    }

    private void Attack()
    {
        int attackNum = Random.Range(0, 4);
        switch (attackNum)
        { 
            case 0:
                {
                    animator.Play("FireBall");
                    break;
                }

            case 1:
                {
                    animator.Play("HugeFireBall");
                    break;
                }
            case 2:
                {
                    animator.Play("LightRain");
                    break;
                }
            case 3:
                {
                    animator.Play("Rush");
                    break;
                }
        }
    }

    private void AttackEnd()
    {
        Invoke("Trans", attackEnd);

    }

    private void TransEnd()
    {
        Invoke("Attack", transEnd);
    }

    private void Trans()
    {
        animator.Play("Teleportation");
    }


    public void TeleportToRandomPosition()
    {
        float randomX = Random.Range(areaCenter.x - areaSize.x / 2, areaCenter.x + areaSize.x / 2);
        float randomZ = Random.Range(areaCenter.y - areaSize.y / 2, areaCenter.y + areaSize.y / 2);
        float currentY = transform.position.y;
        Vector3 randomPosition = new Vector3(randomX, currentY, randomZ);
        transform.position = randomPosition;
    }

    public void FireBall()
    {
        if (player == null || fireBall == null)
        {
            Debug.Log("Player or fireball prefab is not assigned!");
            return;
        }
        Vector3 space = new Vector3(2,littleSpacing,0);
        Vector3 middleSpace = new Vector3(2, 0, 0);
        Vector3 direction = new Vector3(faceRight?1:-1,0,0);
        Vector3 up = transform.position +space;
        Vector3 middle  = transform.position +middleSpace;
        Vector3 down = transform.position -space+2*middleSpace;

        GameObject fireball1 = Instantiate(fireBall, up,Quaternion.Euler(direction));
        GameObject fireball2 = Instantiate(fireBall, middle, Quaternion.Euler(direction));
        GameObject fireball3 = Instantiate(fireBall, down, Quaternion.Euler(direction));

    }

    public void HugeFireBalluUP()
    {
        if (player == null || hugeFireBall == null)
        {
            Debug.Log("Player or fireball prefab is not assigned!");
            return;
        }
        Vector3 space = new Vector3(0, hugeSpacing, 0);
        Vector3 direction = new Vector3(faceRight ? 1 : -1, 0, 0);
        Vector3 up = transform.position+space;

        GameObject HugeFireBall= Instantiate(hugeFireBall, up, Quaternion.Euler(direction));

    }

    public void HugeFireBallDown()
    {

        if (player == null || hugeFireBall == null)
        {
            Debug.Log("Player or fireball prefab is not assigned!");
            return;
        }
        Vector3 space = new Vector3(0, -hugeSpacing, 0);
        Vector3 direction = new Vector3(faceRight ? 1 : -1, 0, 0);
        Vector3 down = transform.position + space;

        GameObject HugeFireBall = Instantiate(hugeFireBall, down, Quaternion.Euler(direction));
    }

    public void LightRain ()
    {
        if (player == null || lightRain == null)
        {
            Debug.Log("Player or fireball prefab is not assigned!");
            return;
        }
        Vector3 space = new Vector3 (lightRainSpacing, 0, 0);
        Vector3 height = new Vector3(0, 3, 0);
        Vector3 startPos = transform.position-8*space+height;
        for (int i = 0; i < 16; i++)
        {
            Vector3 setPos = startPos + space * i;
            GameObject LightRain = Instantiate(lightRain, setPos,Quaternion.identity);
        }

    }

    public void Rush()
    {
        rb.velocity = new Vector2(rushSpeed * (faceRight ? 1 : -1), 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float ATK = 100;
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().GetDamage(ATK);
        }
    }

    private void Flip()
    {
        if (player == null)
        {
            return;
        }
        else
        {
            if (player.position.x > transform.position.x && faceRight == false)
            {
                transform.Rotate(0, 180, 0);
                faceRight = true;
            }
            if (player.position.x < transform.position.x && faceRight == true)
            {
                transform.Rotate(0, 180, 0);
                faceRight = false;
            }
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
