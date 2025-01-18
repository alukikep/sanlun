using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speedRate; // 速率系数
    public float jumpForce; // 跳跃高度
    public bool isBat;
    public bool faceRight;
    public float jumpDir;
    private bool highJump;

    private Rigidbody2D rigidbody2D;
    private Animator animation;
    private float xSpeed;
    private int jumpNumber = 0; // 0,1,2分别表示跳跃了0，1，2次，控制二段跳

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
        //蝙蝠形态（滑翔）
        if (Input.GetKey(KeyCode.R) && isBat == false)
        {
            rigidbody2D.drag = 10;
            isBat = true;
        }
        //持续按住r变身蝙蝠，使下落速度变慢且不能再跳跃
        if (Input.GetKeyUp(KeyCode.R) && isBat == true)
        {
            rigidbody2D.drag = 1;
            isBat = false;
        }
        //松开r还原
    }

    // 当玩家与标签为“ground”的地面接触后，重置跳跃次数
    //OnCollisionEnter2D适用于两者都不是trigger的情况，后续可能需要改成OnTriggerEnter2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
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