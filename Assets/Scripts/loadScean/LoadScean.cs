using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScean : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] private string targetSceneName; // Ŀ�곡�����ƣ����빹�������е�����һ�£�
    [SerializeField] private Vector3 playerSpawnPosition; // ������³���������λ�ã���ѡ��

    private bool isTransitioning = false;

    // ����ҽ��봥������ʱ���������³���
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

    // �첽���ذ汾����ѡ��
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

        // �������λ�ã���ȷ������������ɺ�ִ�У�
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
