using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
using System.Threading.Tasks;

public class LoadScene : MonoBehaviour
{
    [Header("场景设置")]
    [SerializeField] private string targetSceneName; //场景名称
    [SerializeField] private string targetSpawnPointName;
    private GameObject DoubleJumpUnlockItem;
    private bool isTransitioning = false;
    private GameObject Player1;
    private Player playerScripit;
    private Rigidbody2D PlayerRB;
    List<InventoryItemData> inventoryItems;
    public static string TargetSpawnPoint { get; private set; }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Start()
    {
        Player1 = GameObject.Find("Player");
        PlayerRB = Player1.GetComponent<Rigidbody2D>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTransitioning)
        {
            LoadTargetScene();
        }
    }

    private async void LoadTargetScene()
    {
        TargetSpawnPoint = targetSpawnPointName;
        StartCoroutine(LoadAsyncScene());
        isTransitioning = true;
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
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject Player = GameObject.Find("Player");
        GameObject spawnpoint = GameObject.Find(targetSpawnPointName); ;
        transform.position = spawnpoint.transform.position;
        CinemachineVirtualCamera virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
        virtualCam.Follow = Player.transform;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }




}
