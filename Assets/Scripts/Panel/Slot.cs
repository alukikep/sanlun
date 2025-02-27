using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GameObject slot;
    public GameObject parentPanel;
    private TextMeshProUGUI sceneName;
    public Image image;
    private void Start()
    {
        image = GetComponent<Image>(); 
        //��ȡ������
        Transform parentTransform = transform.parent;
        parentPanel=transform.parent.gameObject;
        // ��ȡ��ǰ�����������
        sceneName = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SaveGame()
    {
        image.color = Color.white;
        string currentSceneName = SceneManager.GetActiveScene().name;
        sceneName.text = currentSceneName;
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
    public void Activate()
    {
        slot.SetActive(true);
    }
}
