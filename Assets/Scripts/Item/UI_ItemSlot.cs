using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UI_ItemSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
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
            itemData=item.data;
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
        else
        {
            // 如果物品为null（即物品数量为0或物品被移除），清空图标和文本
            itemImage.sprite = null;
            itemImage.color = Color.clear; // 设置为透明，避免显示白底
            itemText.text = "";
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
