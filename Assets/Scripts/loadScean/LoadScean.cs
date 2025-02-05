using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScean : MonoBehaviour
{
    [Header("场景设置")]
    [SerializeField] private string targetSceneName; // 目标场景名称（需与构建设置中的名称一致）
    [SerializeField] private Vector3 playerSpawnPosition; // 玩家在新场景的生成位置（可选）

    private bool isTransitioning = false;

    // 当玩家进入触发区域时立即加载新场景
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            isTransitioning = true;
            LoadTargetScene();
        }
    }

    private void LoadTargetScene()
    {
       
         StartCoroutine(LoadAsyncScene());
    }

    // 异步加载版本（可选）
    private System.Collections.IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        // 设置玩家位置（需确保场景加载完成后执行）
        SetPlayerPosition();
    }

    private void SetPlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = playerSpawnPosition;
        }
    }


}
