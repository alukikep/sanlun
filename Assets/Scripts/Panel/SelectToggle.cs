using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
        // ��Ӽ��������� Toggle ״̬�ı�ʱ�����ص�
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }
    
    private void OnToggleValueChanged(bool isOn)
    {
        // ���� Toggle ��״̬�������������ʾ״̬
        slot.SetActive(isOn);
        SaveButton.SetActive(isOn);
        LoadButton.SetActive(isOn);
    }
}
