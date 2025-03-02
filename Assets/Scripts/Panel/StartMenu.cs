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
        // 确保只有一个实例存在
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 销毁重复的实例
            return;
        }

        // 设置当前实例
        Instance = this;

        // 防止在场景切换时销毁
        DontDestroyOnLoad(gameObject);

        // 注册场景加载完成事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 每次场景加载完成时调用的方法
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
