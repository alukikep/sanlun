using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.Events;

public class Slot : MonoBehaviour
{
    public GameObject slot;
    public int slotIndex;
    public GameObject savePanel;
    private TextMeshProUGUI sceneName;
    public Image image;
    public SaveLoadButton saveLoadButton;
    private void Start()
    {
        slot = gameObject;
        image = GetComponent<Image>();
        // 获取当前物体的子物体
        sceneName = GetComponentInChildren<TextMeshProUGUI>();
    }
    void Update()
    {
        if (gameObject.activeSelf)
        {
            if (saveLoadButton != null && saveLoadButton.recordImage != null)
            {
                image = saveLoadButton.recordImage;
            }
        }
    }

    public void SaveGame()
    {
        image.color = Color.white;
        string currentSceneName = SceneManager.GetActiveScene().name;
        sceneName.text = currentSceneName;
        image.sprite = Player.Instance.CheckpointPicture;
    }
    public void LoadGame()
    {
        slot.SetActive(false);
        savePanel.SetActive(false);
        Time.timeScale = 1;
        if (Player.Instance != null)
            Player.Instance.enabled = true;
    }
    public void LoadImage(int slotIndex)
    {
        string savePath = SaveManager.Instance.GetSavePath(slotIndex);
        // 检查文件是否存在
        if (!System.IO.File.Exists(savePath))
        {
            return;
        }
        // 读取存档数据
        string jsonData = System.IO.File.ReadAllText(savePath);
        if (string.IsNullOrEmpty(jsonData))
        {
            return;
        }
        PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(jsonData);
        // 检查 data.currentCheckpointImage 是否为 null
        if (data.currentCheckpointImage != null)
        {
            image.sprite = data.currentCheckpointImage;
        }
        // 设置图片颜色
        image.color = Color.white;
    }
}
