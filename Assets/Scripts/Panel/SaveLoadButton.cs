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
        gameObject.SetActive(false);
        if (button==null)return;
        if (isSaveButton)
        {
            if (Player.Instance != null && Player.Instance.CheckpointPicture != null)
            {
                if (recordImage != null)
                recordImage.sprite = Player.Instance.CheckpointPicture;
            }
            SaveManager.Instance.SaveGame(slotIndex);
        }
        else
        {
            SaveManager.Instance.LoadGame(slotIndex);
            save.Instance.ActivateInventory();
            Player.Instance.pauseMenu.SetActive(false);
            Player.Instance.DieMenu.SetActive(false);
        }
    }
}