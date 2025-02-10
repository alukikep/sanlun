using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScean : MonoBehaviour
{
    [Header("场景设置")]
    [SerializeField] private string targetSceneName; //场景名称
    private GameObject DoubleJumpUnlockItem;
    private bool isTransitioning = false;
    private GameObject Player;

    private void Start()
    {
        Player = GameObject.Find("Player");
        DoubleJumpUnlockItem = GameObject.Find("DoubleJumpUnlockItem");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTransitioning)
        {
            
            LoadTargetScene();
           
        }
    }

    private void LoadTargetScene()
    {
       if(targetSceneName== "Sewers")
        {
            if(DoubleJumpUnlockItem!=null)
            {
                Debug.Log("yes");
               
            }
            else
            {
                StartCoroutine(LoadAsyncScene());
                isTransitioning = true;
            }
        }
        else
        {
            StartCoroutine(LoadAsyncScene());
            isTransitioning = true;
        }
    }

    private IEnumerator LoadAsyncScene()
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
       
    }

   


}
