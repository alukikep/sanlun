using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public static Slot Instance;
    public GameObject slot;
    public GameObject parentPanel;
    private TextMeshProUGUI sceneName;
    public Image image;
    private void Start()
    {
        slot = gameObject;
        image = GetComponent<Image>(); 
        //获取父物体
        Transform parentTransform = transform.parent;
        parentPanel=transform.parent.gameObject;
        // 获取当前物体的子物体
        sceneName = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SaveGame()
    {
        //image.color = Color.white;
        string currentSceneName = SceneManager.GetActiveScene().name;
        //sceneName.text = currentSceneName;
        if (Player.Instance.CheckpointPicture != null)
        {
            image.sprite = Player.Instance.CheckpointPicture;
        }
    }
    public void LoadGame()
    {
        slot.SetActive(false);
        parentPanel.SetActive(false);
        Time.timeScale = 1;
        Player.Instance.enabled = true;
    }
}
