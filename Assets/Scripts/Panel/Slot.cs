using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Slot : MonoBehaviour
{
    public GameObject parentPanel;
    private TextMeshProUGUI sceneName;
    private void Start()
    {
        //��ȡ������
        Transform parentTransform = transform.parent;
        parentPanel=transform.parent.gameObject;
        // ��ȡ��ǰ�����������
        sceneName = GetComponentInChildren<TextMeshProUGUI>();
        // ��ȡ��ǰ����������
        string currentSceneName = SceneManager.GetActiveScene().name;
        sceneName.text = currentSceneName;
    }
    public void SaveGame()
    {
        Player.Instance.SavePlayer();
    }
    public void LoadGame()
    {
        parentPanel.SetActive(false);
        Time.timeScale = 1;
        Player.Instance.enabled = true;
        Player.Instance.LoadPlayer();
    }
}
