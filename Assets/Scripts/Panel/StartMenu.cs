using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public static StartMenu Instance;
    private GameObject startMenu;

    void Awake()
    {
        startMenu = gameObject;
        // ȷ��ֻ��һ��ʵ������
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // �����ظ���ʵ��
            return;
        }

        // ���õ�ǰʵ��
        Instance = this;

        // ��ֹ�ڳ����л�ʱ����
        DontDestroyOnLoad(gameObject);

        // ע�᳡����������¼�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ÿ�γ����������ʱ���õķ���
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartMenu")
        {
            startMenu.SetActive(true);
        }
        else
        {
            startMenu.SetActive(false);
        }
    }
}
