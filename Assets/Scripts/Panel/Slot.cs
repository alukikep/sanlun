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
        // ��ȡ��ǰ�����������
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
        // ����ļ��Ƿ����
        if (!System.IO.File.Exists(savePath))
        {
            return;
        }
        // ��ȡ�浵����
        string jsonData = System.IO.File.ReadAllText(savePath);
        if (string.IsNullOrEmpty(jsonData))
        {
            return;
        }
        PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(jsonData);
        // ��� data.currentCheckpointImage �Ƿ�Ϊ null
        if (data.currentCheckpointImage != null)
        {
            image.sprite = data.currentCheckpointImage;
        }
        // ����ͼƬ��ɫ
        image.color = Color.white;
    }
}
