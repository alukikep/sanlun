using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    public float triggerDistance = 3f; // ��ҿ��������ľ���
    public Animator animator;
    private Player player;
    private bool playerInRange; // ����Ƿ��ڴ浵�㸽��

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // ���´浵��״̬
    private void Update()
    {
        // �������Ƿ���봥����Χ
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= triggerDistance)
        {
            playerInRange = true;
            animator.SetBool("IsIdle", true); // ���Ŵ浵��Ŀ��ж���
        }
        else
        {
            playerInRange = false;
            animator.SetBool("IsIdle", false);
        }

        if (playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveGame();
                animator.SetTrigger("IsActive");
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadGame();
                animator.SetTrigger("IsActive");
            }
        }
    }

    // ������Ϸ����
    private void SaveGame()
    {
        Player.Instance.SavePlayer();
        Debug.Log("��Ϸ�浵�ɹ���");
    }
    public void LoadGame()
    {
        Player.Instance.LoadPlayer(); // ������ҵĶ�������
        Debug.Log("��Ϸ�Ѷ�ȡ");
    }
    
}