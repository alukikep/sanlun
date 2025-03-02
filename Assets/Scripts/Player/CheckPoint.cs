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
    private GameObject canvas;
    public GameObject savePanel;
    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        canvas = GameObject.Find("Canvas");
        savePanel =FindChildByName(canvas,"SavePanel");
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
        // ���������������������
        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.name == childName)
            {
                return child.gameObject; // �ҵ�Ŀ��������
            }

            // ����������������壬�ݹ����
            if (child.childCount > 0)
            {
                GameObject found = FindChildByName(child.gameObject, childName);
                if (found != null)
                {
                    return found;
                }
            }
        }

        // ���û���ҵ������� null
        return null;
    }
}