using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.Events;
using static UnityEditorInternal.VersionControl.ListControl;

public class Slot : MonoBehaviour
{
    public GameObject slot;
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
        Player.Instance.enabled = true;
    }
}
