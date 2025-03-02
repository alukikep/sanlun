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
    private GameObject canvas;
    public GameObject savePanel;
    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        canvas = GameObject.Find("Canvas");
        savePanel =FindChildByName(canvas,"SavePanel");
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
            if (Input.GetKeyDown(KeyCode.F)&&!Player.Instance.isBat&&!Player.Instance.isMouse)
            {
                Time.timeScale = 0;
                Player.Instance.CheckpointPicture = CheckpointPicture;
                Player.Instance.enabled = false;
                savePanel.SetActive(true);
            }
        }
    }
    private GameObject FindChildByName(GameObject parent, string childName)
    {
        // 遍历父物体的所有子物体
        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.name == childName)
            {
                return child.gameObject; // 找到目标子物体
            }

            // 如果子物体有子物体，递归查找
            if (child.childCount > 0)
            {
                GameObject found = FindChildByName(child.gameObject, childName);
                if (found != null)
                {
                    return found;
                }
            }
        }

        // 如果没有找到，返回 null
        return null;
    }
}