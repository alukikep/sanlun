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
    void ClearSaveFiles()
    {
        for (int i = 0; i < maxSaveSlots; i++)
        {
            string savePath = GetSavePath(i);
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
        }
    }

    public void SaveGame(int slotIndex)
    {
        // 创建存档路径
        string savePath = GetSavePath(slotIndex);
        // 获取 CheckpointPicture 的路径
        string checkpointImagePath = "";
        if (Player.Instance.CheckpointPicture != null)
        {
#if UNITY_EDITOR
            string fullPath = AssetDatabase.GetAssetPath(Player.Instance.CheckpointPicture);

            if (fullPath.StartsWith("Assets/Resources/"))
            {
                checkpointImagePath = fullPath.Substring("Assets/Resources/".Length);
            }
            else
            {
                Debug.LogError("CheckpointPicture is not in the Resources folder.");
            }
#endif
        }
        List<string> weaponPrefabPaths = new List<string>
    {
        "Prefabs/SubWeapon/axe.prefab", // 相对于 Resources 文件夹的路径
        "Prefabs/SubWeapon/guardian.prefab",
        "Prefabs/SubWeapon/familiar.prefab",
        "Prefabs/SubWeapon/timeSlow.prefab"
    };
        Player.Instance.health = Player.Instance.maxHealth;
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
            currentCheckpointImage = Player.Instance.CheckpointPicture,
            collectedPotions = Player.Instance.GetCollectedPotions().ToList(),
            inventoryItems = Inventory.Instance.InventoryItems.Select(item => new InventoryItemData
            {
                itemName = item.data.itemName,
                iconPath = item.data.icon.name,
                quantity = item.stackSize
            }).ToList(),
            collectedWeapons = new List<bool>
        {
            Player.Instance.isAxeEnabled,
            Player.Instance.isGuardianEnabled,
            Player.Instance.isTimeSlowEnabled,
            Player.Instance.isFamiliarEnabled
        },
            collectedWeaponsIndex = Player.Instance.collectedWeapons, // 保存副武器名称列表
            checkpointImagePath = checkpointImagePath, // 存储路径
            weaponPrefabPaths = weaponPrefabPaths,
            maxSubWeaponNum = Player.Instance.maxSubWeaponNum // 保存 maxSubWeaponNum
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
        if (Player.Instance != null)
        {
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

            // 加载 CheckpointPicture
            if (!string.IsNullOrEmpty(data.checkpointImagePath))
            {
                // 确保路径格式正确，移除扩展名
                string pathWithoutExtension = Path.ChangeExtension(data.checkpointImagePath, null);
                Sprite checkpointSprite = Resources.Load<Sprite>(pathWithoutExtension);

                if (checkpointSprite != null)
                {
                    Player.Instance.CheckpointPicture = checkpointSprite;
                }
                else
                {
                    Debug.LogError($"CheckpointPicture not found at path: {pathWithoutExtension}");
                }
            }
            Player.Instance.collectedWeapons = data.collectedWeaponsIndex; // 加载副武器名称列表

            // 加载副武器 Prefab 和状态
            if (data.weaponPrefabPaths.Count > 0 && data.collectedWeapons.Count > 0)
            {
                for (int i = 0; i < data.weaponPrefabPaths.Count; i++)
                {
                    string prefabPath = data.weaponPrefabPaths[i];
                    bool isEnabled = data.collectedWeapons[i];

                    // 确保路径是相对于 Resources 文件夹的
                    prefabPath = prefabPath.Replace("Assets/Resources/", "").Replace(".prefab", "");

                    GameObject prefab = Resources.Load<GameObject>(prefabPath);
                    if (prefab != null)
                    {
                        GameObject weaponInstance = Instantiate(prefab, Player.Instance.transform.position, Quaternion.identity);
                        if (i == 0) Player.Instance.axe = weaponInstance;
                        else if (i == 1) Player.Instance.guardian = weaponInstance;
                        else if (i == 2) Player.Instance.familiar = weaponInstance;
                        else if (i == 3) Player.Instance.TimeSlow = weaponInstance;

                        weaponInstance.SetActive(isEnabled);
                    }
                    else
                    {
                        Debug.LogError($"Prefab not found at path: {prefabPath}");
                    }
                }
            }
            Player.Instance.maxSubWeaponNum = data.maxSubWeaponNum;
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