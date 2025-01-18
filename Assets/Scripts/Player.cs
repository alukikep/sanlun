using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speedRate; // ����ϵ��
    public float jumpForce; // ��Ծ�߶�
    public bool isBat;
    public bool faceRight;
    public float jumpDir;
    private bool highJump;

    private Rigidbody2D rigidbody2D;
    private Animator animation;
    private float xSpeed;
    private int jumpNumber = 0; // 0,1,2�ֱ��ʾ��Ծ��0��1��2�Σ����ƶ�����

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animation=GetComponentInChildren<Animator>();
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
        rigidbody2D.velocity = new Vector2(xSpeed * speedRate, rigidbody2D.velocity.y);

        // ��Ծ
        if (Input.GetButtonDown("Jump") && jumpNumber < 2)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
            jumpNumber++;
        }

        if(Input.GetKeyDown(KeyCode.Q)&&jumpNumber!=0)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce * 3);
            highJump=true;
            jumpNumber = 2;
        }
        Flip();
        AnimatorControllers();
        BatTranform();
    }

    private void BatTranform()
    {
        //������̬�����裩
        if (Input.GetKey(KeyCode.R) && isBat == false)
        {
            rigidbody2D.drag = 10;
            isBat = true;
        }
        //������סr��������ʹ�����ٶȱ����Ҳ�������Ծ
        if (Input.GetKeyUp(KeyCode.R) && isBat == true)
        {
            rigidbody2D.drag = 1;
            isBat = false;
        }
        //�ɿ�r��ԭ
    }

    // ��������ǩΪ��ground���ĵ���Ӵ���������Ծ����
    //OnCollisionEnter2D���������߶�����trigger�����������������Ҫ�ĳ�OnTriggerEnter2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpNumber = 0;
            highJump = false;
        }
    }

    private void AnimatorControllers()//���ƶ���
    {
        bool isMoving = rigidbody2D.velocity.x != 0;
        bool isGround = jumpNumber == 0;
        if(rigidbody2D.velocity.y>0)
        {
            jumpDir = 1;
        }
        if(rigidbody2D.velocity.y<0)
        {
            jumpDir = -1;
        }
        animation.SetBool("isMoving", isMoving);
        animation.SetBool("isBat",isBat);
        animation.SetBool("isGround",isGround);
        animation.SetFloat("jump",jumpDir );
        animation.SetBool("highJump",highJump);
    }

    private void Flip()//����ת��
    {
        if(rigidbody2D.velocity.x > 0&&faceRight==false)
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