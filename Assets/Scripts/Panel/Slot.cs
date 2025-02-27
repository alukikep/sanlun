using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GameObject parentPanel;
    private TextMeshProUGUI sceneName;
    public Image image;
    private void Start()
    {
        image = GetComponent<Image>(); 
        //获取父物体
        Transform parentTransform = transform.parent;
        parentPanel=transform.parent.gameObject;
        // 获取当前物体的子物体
        sceneName = GetComponentInChildren<TextMeshProUGUI>();
        // 获取当前场景的名称
        string currentSceneName = SceneManager.GetActiveScene().name;
        sceneName.text = currentSceneName;
    }
    public void SaveGame()
    {
        image.sprite=Player.Instance.CheckpointPicture;
        Player.Instance.SavePlayer();
    }
    public void LoadGame()
    {
        parentPanel.SetActive(false);
        Time.timeScale = 1;
        Player.Instance.enabled = true;
        Player.Instance.LoadPlayer();
    }
}
