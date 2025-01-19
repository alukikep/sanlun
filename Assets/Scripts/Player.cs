using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speedRate; // 速率系数
    public float jumpForce; // 跳跃高度
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
    private CapsuleCollider2D capsuleCollider;
    private float xSpeed;
    private int jumpNumber = 0; // 0,1,2分别表示跳跃了0，1，2次，控制二段跳

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animation=GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        isBat = false;
        faceRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        xSpeed = Input.GetAxisRaw("Horizontal");
        // 按住shift速度翻倍
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            xSpeed *= 2;
        }
        rigidbody2D.velocity = new Vector2(xSpeed * speedRate, rigidbody2D.velocity.y);

        // 跳跃
        if (Input.GetButtonDown("Jump") && jumpNumber < 2)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
            jumpNumber++;
        }

        if(Input.GetKeyDown(KeyCode.Q)&&jumpNumber!=0)//在空中按Q高跳（可以无限跳)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce * 3);
            highJump=true;
            jumpNumber = 2;
        }
        Flip();
        AnimatorControllers();
        BatTranform();
        MouseTransform();
        Attack();
    }

    private void BatTranform()//按R变身蝙蝠（空中时）
    {
        //蝙蝠形态（滑翔）
        if (Input.GetKeyDown(KeyCode.R) && isBat == false&&isMouse==false)
        {
            rigidbody2D.drag = 10;
            isBat = true;
        }
        //持续按住r变身蝙蝠，使下落速度变慢且不能再跳跃
        else if (Input.GetKeyDown(KeyCode.R) && isBat == true&&isMouse==false)
        {
            rigidbody2D.drag = 1;
            isBat = false;
        }
        //松开r还原
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J) && isMouse == false && isBat == false && jumpNumber == 0&&isAttack==false)
        {
            isAttack = true;
        }
    }

    private void MouseTransform()//按E变身老鼠
    {
        if(Input.GetKeyDown(KeyCode.E) && isMouse == false)
        {
            isMouse = true;
            jumpForce = 8;
            capsuleCollider.size=new Vector2(0.8f,0.8f);
        }
        else if(Input.GetKeyDown(KeyCode.E)&& isMouse == true)
        {
            isMouse=false;
            jumpForce=10;
            capsuleCollider.size = new Vector2(1,1.9f);
        }
    }

    // 当玩家与标签为“ground”的地面接触后，重置跳跃次数
    //OnCollisionEnter2D适用于两者都不是trigger的情况，后续可能需要改成OnTriggerEnter2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            jumpNumber = 0;
            highJump = false;
        }
    }

    private void AnimatorControllers()//控制动画
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
        animation.SetBool("isMouse", isMouse);
        animation.SetBool("isAttack",isAttack);
    }

    private void Flip()//控制转向
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