using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public int maxSaveSlots;
    public List<SaveSlot> saveSlots = new List<SaveSlot>();
    public SaveSlot selectedSlot;
    public GameObject inventory;
    public GameObject player;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        LoadAllSaves();
    }
//    void OnEnable()
//    {
//        // 在编辑器中注册退出事件
//        if (Application.isEditor)
//        {
//#if UNITY_EDITOR
//            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
//#endif
//        }
//    }

//    void OnDisable()
//    {
//        // 在编辑器中注销退出事件
//        if (Application.isEditor)
//        {
//#if UNITY_EDITOR
//            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
//#endif
//        }
//    }

//#if UNITY_EDITOR
//    private void OnPlayModeStateChanged(PlayModeStateChange state)
//    {
//        // 检测是否从运行模式切换到编辑模式
//        if (state == PlayModeStateChange.ExitingPlayMode)
//        {
//            ClearSaveFiles();
//        }
//    }

//#endif
    void ClearSaveFiles()
    {
        for (int i = 0; i < maxSaveSlots; i++)
        {
            string savePath = GetSavePath(i);
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                Debug.Log($"Deleted save file: {savePath}");
            }
        }
    }

    public void SaveGame(int slotIndex)
    {
        // 创建存档路径
        string savePath = GetSavePath(slotIndex);

        // 获取玩家数据
        PlayerSaveData data = new PlayerSaveData
        {
            health = Player.Instance.health,
            currentMana = Player.Instance.currentMana,
            position = Player.Instance.transform.position,
            isDoubleJumpEnabled = Player.Instance.isdoubleJumpEnabled,
            isHighJumpEnabled = Player.Instance.ishighJumpEnabled,
            isBatTransformEnabled = Player.Instance.isbatTransformEnabled,
            isRatTransformEnabled = Player.Instance.isratTransformEnabled,
            isAxeEnabled = Player.Instance.isAxeEnabled,
            isGuardianEnabled = Player.Instance.isGuardianEnabled,
            isTimeSlowEnabled = Player.Instance.isTimeSlowEnabled,
            attack = Player.Instance.ATK,
            currentSceneName = SceneManager.GetActiveScene().name,
            currentCheckpointImage=Player.Instance.CheckpointPicture,
            collectedPotions = Player.Instance.GetCollectedPotions().ToList(),
            inventoryItems = Inventory.Instance.InventoryItems.Select(item => new InventoryItemData
            {
                itemName = item.data.itemName,
                iconPath = item.data.icon.name, // 如果需要保存图标路径
                quantity = item.stackSize
            }).ToList()
        };

        // 序列化数据
        string jsonData = JsonUtility.ToJson(data);

        // 保存到文件
        System.IO.File.WriteAllText(savePath, jsonData);

        // 更新存档列表
        saveSlots[slotIndex] = new SaveSlot($"Save_{slotIndex + 1}", savePath, System.DateTime.Now);
    }

    public void LoadGame(int slotIndex)
    {
        player.SetActive(true);
        inventory.SetActive(true);
        string savePath = GetSavePath(slotIndex);
        // 读取存档数据
        if (savePath == null) return;
        string jsonData = System.IO.File.ReadAllText(savePath);
        PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(jsonData);
        Inventory.Instance.ClearInventory();
        // 切换场景
        SceneManager.LoadScene(data.currentSceneName);
        // 还原玩家数据
        Player.Instance.health = data.health;
        Player.Instance.currentMana = data.currentMana;
        Player.Instance.transform.position = data.position;
        Player.Instance.isdoubleJumpEnabled = data.isDoubleJumpEnabled;
        Player.Instance.ishighJumpEnabled = data.isHighJumpEnabled;
        Player.Instance.isbatTransformEnabled = data.isBatTransformEnabled;
        Player.Instance.isratTransformEnabled = data.isRatTransformEnabled;
        Player.Instance.isAxeEnabled = data.isAxeEnabled;
        Player.Instance.isGuardianEnabled = data.isGuardianEnabled;
        Player.Instance.isTimeSlowEnabled = data.isTimeSlowEnabled;
        Player.Instance.ATK = data.attack;
        // 加载背包数据
        Inventory.Instance.LoadInventory(data.inventoryItems);

        StartCoroutine(Player.Instance.UpdateVirtualCameraAfterLoad());
        GameObject[] allObj = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObj)
        {
            if (obj.name == "Player")
            {
                obj.SetActive(true);
                GameObject PauMenu = GameObject.Find("PauseMenu");
                if (PauMenu != null)
                {

                    PauMenu.SetActive(false);
                    Time.timeScale = 1f;
                }
            }
        }
    }
    public string GetSavePath(int slotIndex)
    {
        return Application.persistentDataPath + $"Save_{slotIndex + 1}.save";
    }

    // 加载所有存档
    void LoadAllSaves()
    {
        saveSlots.Clear();

        for (int i = 0; i < maxSaveSlots; i++)
        {
            string savePath = GetSavePath(i);
            if (System.IO.File.Exists(savePath))
            {
                saveSlots.Add(new SaveSlot($"Save_{i + 1}", savePath, System.IO.File.GetLastWriteTime(savePath)));
            }
            else
            {
                saveSlots.Add(new SaveSlot($"Save_{i + 1}", savePath, System.DateTime.MinValue));
            }
        }
    }
}

[System.Serializable]
public class SaveSlot
{
    public string name;
    public string path;
    public System.DateTime date;

    public SaveSlot(string name, string path, System.DateTime date)
    {
        this.name = name;
        this.path = path;
        this.date = date;
    }
}