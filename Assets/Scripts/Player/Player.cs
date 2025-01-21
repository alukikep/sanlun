using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speedRate; // ����ϵ��
    public float jumpForce; // ��Ծ�߶�
    public bool isBat;
    public bool isMouse;
    public bool faceRight;
    public float jumpDir;
    private bool highJump;

    [Header("Attack")]
    public Transform attackCheck;
    public float attackRadius;
    public bool isAttack;

    private Rigidbody2D rigidbody2D;
    private Animator animation;
    private BoxCollider2D boxCollider2D;
    private float xSpeed;
    private int jumpNumber = 0; // 0,1,2�ֱ��ʾ��Ծ��0��1��2�Σ����ƶ�����

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animation = GetComponentInChildren<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        isBat = false;
        faceRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        xSpeed = Input.GetAxisRaw("Horizontal");
        // ��סshift�ٶȷ���
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            xSpeed *= 2;
        }

        if (isAttack == false)
        {
            rigidbody2D.velocity = new Vector2(xSpeed * speedRate, rigidbody2D.velocity.y);
        }
        else
        {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }

        // ��Ծ
        if (Input.GetButtonDown("Jump") && jumpNumber < 2)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
            jumpNumber++;
        }

        if (Input.GetKeyDown(KeyCode.Q) && jumpNumber != 0)//�ڿ��а�Q����������������)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce * 3);
            highJump = true;
            jumpNumber = 2;
        }
        Flip();
        AnimatorControllers();
        BatTranform();
        MouseTransform();
        Attack();
    }

    private void BatTranform()//��R�������𣨿���ʱ��
    {
        //������̬�����裩
        if (Input.GetKeyDown(KeyCode.R) && isBat == false && isMouse == false)
        {
            rigidbody2D.drag = 10;
            isBat = true;
            boxCollider2D.size = new Vector2(0.4f, 0.8f);
        }
        //��r��������ʹ�����ٶȱ����Ҳ�������Ծ
        else if (Input.GetKeyDown(KeyCode.R) && isBat == true && isMouse == false)
        {
            rigidbody2D.drag = 1;
            isBat = false;
            boxCollider2D.size = new Vector2(0.4f, 1.7f);
        }
        //�ɿ�r��ԭ
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J) && isMouse == false && isBat == false && jumpNumber == 0 && isAttack == false)
        {
            isAttack = true;
        }
    }

    private void MouseTransform()//��E��������
    {
        if (Input.GetKeyDown(KeyCode.E) && isMouse == false)
        {
            isMouse = true;
            jumpForce = 8;
            boxCollider2D.size = new Vector2(0.4f, 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.E) && isMouse == true)
        {
            isMouse = false;
            jumpForce = 14;
            boxCollider2D.size = new Vector2(0.4f, 1.7f);
        }
    }

    // ��������ǩΪ��ground���ĵ���Ӵ���������Ծ����
    //OnCollisionEnter2D���������߶�����trigger�����������������Ҫ�ĳ�OnTriggerEnter2D
    //���޸�Ϊtrigger��⣬Ϊ��һЩ�����νӵ���ȷ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            jumpNumber = 0;
            highJump = false;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
           jumpNumber = 1;
        }
    }

    private void AnimatorControllers()//���ƶ���
    {
        bool isMoving = rigidbody2D.velocity.x != 0;
        bool isGround = jumpNumber == 0;
        if (rigidbody2D.velocity.y > 0)
        {
            jumpDir = 1;
        }
        if (rigidbody2D.velocity.y < 0)
        {
            jumpDir = -1;
        }
        animation.SetBool("isMoving", isMoving);
        animation.SetBool("isBat", isBat);
        animation.SetBool("isGround", isGround);
        animation.SetFloat("jump", jumpDir);
        animation.SetBool("highJump", highJump);
        animation.SetBool("isMouse", isMouse);
        animation.SetBool("isAttack", isAttack);
    }

    private void Flip()//����ת��
    {
        if (rigidbody2D.velocity.x > 0 && faceRight == false)
        {
            transform.Rotate(0, 180, 0);
            faceRight = true;
        }
        if (rigidbody2D.velocity.x < 0 && faceRight == true)
        {
            transform.Rotate(0, 180, 0);
            faceRight = false;
        }
    }

   
}