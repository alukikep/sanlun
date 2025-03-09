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

    public GameObject axePrefab;
    public GameObject guardianPrefab;
    public GameObject familiarPrefab;
    public GameObject timeSlowPrefab;
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Start()
    {
        Player1 = GameObject.Find("Player");
        PlayerRB = Player1.GetComponent<Rigidbody2D>();
        StartCoroutine(Player.Instance.DestroyCollectedPotions());

        if (Player.Instance == null)
        {
            Debug.LogError("Player.Instance is null. Please ensure Player object exists in the scene.");
            return;
        }

        // 初始化副武器 Prefab
        //if (Player.Instance.isAxeEnabled && Player.Instance.axe == null)
        //{
        //    Player.Instance.axe = Instantiate(axePrefab, Player.Instance.transform.position, Quaternion.identity);
        //}

        //if (Player.Instance.isGuardianEnabled && Player.Instance.guardian == null)
        //{
        //    Player.Instance.guardian = Instantiate(guardianPrefab, Player.Instance.transform.position, Quaternion.identity);
        //}

        //if (Player.Instance.isFamiliarEnabled && Player.Instance.familiar == null)
        //{
        //    Player.Instance.familiar = Instantiate(familiarPrefab, Player.Instance.transform.position, Quaternion.identity);
        //}

        //if (Player.Instance.isTimeSlowEnabled && Player.Instance.TimeSlow == null)
        //{
        //    Player.Instance.TimeSlow = Instantiate(timeSlowPrefab, Player.Instance.transform.position, Quaternion.identity);
        //}
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
                // 等待场景加载完成
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject Player1 = GameObject.Find("Player");
        GameObject spawnpoint = GameObject.Find(targetSpawnPointName); 
        if(spawnpoint!=null)
        {
            transform.position = spawnpoint.transform.position;
        }
        
        CinemachineVirtualCamera virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
        virtualCam.Follow = Player1.transform;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}