using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    public float triggerDistance = 3f; // ��ҿ��������ľ���
    public Animator animator;
    private Player player;
    private bool playerInRange; // ����Ƿ��ڴ浵�㸽��
    public Sprite CheckpointPicture;
    public GameObject savePanel;
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

        TogglePanel();
    }
    private void TogglePanel()
    {
        if (playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Time.timeScale = 0;
                Player.Instance.CheckpointPicture = CheckpointPicture;
                Player.Instance.enabled = false;
                savePanel.SetActive(true);
            }
        }
    }
}