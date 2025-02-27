using UnityEngine;
using UnityEngine.UI;

public class SaveLoadButton : MonoBehaviour
{
    public int slotIndex; // ´æµµ²ÛË÷Òý
    public bool isSaveButton; // ÊÇ·ñÎª´æµµ°´Å¥
    private Button button;
    private SelectToggle selectToggle;
    public UnityEngine.UI.Image recordImage;
    void Start()
    {
        selectToggle = GetComponentInParent<SelectToggle>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        if (isSaveButton)
        {
            recordImage.sprite=Player.Instance.CheckpointPicture;
            SaveManager.Instance.SaveGame(slotIndex);
        }
        else
        {
            SaveManager.Instance.LoadGame(slotIndex);
        }
    }
}