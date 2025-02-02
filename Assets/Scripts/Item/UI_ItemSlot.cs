using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UI_ItemSlot : MonoBehaviour
{
    [SerializeField]private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;
    public ItemData itemData;

    public void UpdateSlot(InventoryItem _newItem)
    {
        item=_newItem;

        itemImage.color = Color.white;
        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }
    
    //点击使用道具
    public void OnClick()
    {
        if (itemData != null)
        {
            Inventory.Instance.UseItem(itemData);
        }
    }
}
