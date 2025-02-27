using UnityEngine;
using UnityEngine.UI;

public class SaveLoadButton : MonoBehaviour
{
    public int slotIndex; // �浵������
    public bool isSaveButton; // �Ƿ�Ϊ�浵��ť
    private Button button;
    private SelectButton selectButton;

    void Start()
    {
        selectButton = GetComponentInParent<SelectButton>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        if (isSaveButton)
        {
            SaveManager.Instance.SaveGame(slotIndex);
        }
        else
        {
            SaveManager.Instance.LoadGame(slotIndex);
        }
    }
    public void UpdateRecordImage()
    {
        if (isSaveButton)
        {
            selectButton.recordimage.sprite=Player.Instance.CheckpointPicture;
        }
    }
}