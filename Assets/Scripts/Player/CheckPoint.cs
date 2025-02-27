using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    public float triggerDistance = 3f; // 玩家靠近触发的距离
    public Animator animator;
    private Player player;
    private bool playerInRange; // 玩家是否在存档点附近
    public Sprite CheckpointPicture;
    public GameObject savePanel;
    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // 更新存档点状态
    private void Update()
    {
        // 检测玩家是否进入触发范围
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= triggerDistance)
        {
            playerInRange = true;
            animator.SetBool("IsIdle", true); // 播放存档点的空闲动画
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