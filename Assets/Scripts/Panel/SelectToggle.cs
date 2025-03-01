using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class SelectToggle : MonoBehaviour
{
    public GameObject slot;
    public GameObject SaveButton;
    public GameObject LoadButton;
    public SaveLoadPanel saveLoadPanel;
    public UnityEngine.UI.Toggle toggle;

    private void Start()
    {
        saveLoadPanel= gameObject.GetComponentInParent<SaveLoadPanel>();
        // 添加监听器，当 Toggle 状态改变时触发回调
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }
    
    private void OnToggleValueChanged(bool isOn)
    {
        // 根据 Toggle 的状态设置子物体的显示状态
        slot.SetActive(isOn);
        if (SceneManager.GetActiveScene().name != "StartMenu"&&!Player.Instance.pauseMenu.activeSelf&&!Player.Instance.DieMenu.activeSelf)
        {
            SaveButton.SetActive(isOn);
        }
        LoadButton.SetActive(isOn);
    }
}
