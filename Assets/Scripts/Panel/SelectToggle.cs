using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class SelectToggle : MonoBehaviour
{
    public GameObject slot;
    public int slotIndex;
    public GameObject SaveButton;
    public GameObject LoadButton;
    public SaveLoadPanel saveLoadPanel;
    public UnityEngine.UI.Toggle toggle;
    private bool hasSaveSlot;

    private void Start()
    {
        saveLoadPanel= gameObject.GetComponentInParent<SaveLoadPanel>();
        // ��Ӽ��������� Toggle ״̬�ı�ʱ�����ص�
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
        CheckSaveSlot();
    }
    
    private void OnToggleValueChanged(bool isOn)
    {
        // ���� Toggle ��״̬�������������ʾ״̬
        slot.SetActive(isOn);
        if (SceneManager.GetActiveScene().name != "StartMenu"&&!Player.Instance.pauseMenu.activeSelf&&!Player.Instance.DieMenu.activeSelf)
        {
            SaveButton.SetActive(isOn);
        }
        if(hasSaveSlot)
        LoadButton.SetActive(isOn);
    }
    public void CheckSaveSlot()
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
        hasSaveSlot = true;
    }
}
