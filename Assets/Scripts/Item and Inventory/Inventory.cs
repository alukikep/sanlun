using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<InventoryItem> InventoryItems = new List<InventoryItem>();
    public Dictionary<ItemData, InventoryItem> inventoryDictionary = new Dictionary<ItemData, InventoryItem>();


    [Header("Inventory UI")]

    [SerializeField]private Transform inventorySlotParent;

    private UI_ItemSlot[] itemSlot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 确保在场景切换时不销毁
        }
        else
        {
            Destroy(gameObject); // 销毁重复的实例
        }
        
    }
    private void Start()
    {
        itemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }
    public void UpdateSlotUI()
    {
        if(itemSlot == null)return;
        if (itemSlot.Length > 0)
        {
            for (int i = 0; i < itemSlot.Length; i++)
            {
                if (i < InventoryItems.Count)
                {
                    // 如果物品槽有物品，更新物品信息
                    itemSlot[i].UpdateSlot(InventoryItems[i]);
                }
                else
                {
                    // 如果物品槽没有物品，清空显示
                    itemSlot[i].UpdateSlot(null);
                }
            }
        }
    }
    public void AddItem(ItemData _item)
    {
        // 使用 itemType 查找字典中是否已存在相同的物品
        foreach (var kvp in inventoryDictionary)
        {
            if (kvp.Key.itemType == _item.itemType) // 比较 itemType
            {
                kvp.Value.AddStack(); // 增加堆叠数量
                UpdateSlotUI();
                return;
            }
        }

        // 如果没有找到，创建新的物品
        InventoryItem newItem = new InventoryItem(_item);
        InventoryItems.Add(newItem);
        inventoryDictionary.Add(_item, newItem);
        UpdateSlotUI();
    }
    public void RemoveItem(ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item,out InventoryItem value))
        {
            //如果物品数量为0，在背包中删除这个物品
            if (value.stackSize <= 1)
            {
                InventoryItems.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
                value.RemoveStack();
        }
       
        UpdateSlotUI();
    }
    // 使用物品
    public void UseItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            switch (_item.itemType)
            {
                case ItemData.E_ItemType.HealingPotion:
                    Player.Instance.Heal(_item.healAmount);
                    break;
                case ItemData.E_ItemType.HealthMaxPotion:
                    Player.Instance.IncreaseMaxHealth(_item.increaseHealthMax);
                    break;
                case ItemData.E_ItemType.EnternalAttackPotion:
                    Player.Instance.IncreaseEnternalAttack(_item.increaseAttack);
                    break;
                case ItemData.E_ItemType.TemporaryAttackPotion:
                    Player.Instance.IncreaseTemporaryAttack(_item.increaseAttack,_item.Duration);
                    break;
                case ItemData.E_ItemType.ManaCapacityPotion:
                    Player.Instance.IncreaseMaxMana(_item.increaseManaMax);
                    break;
                case ItemData.E_ItemType.ManaRestorePotion:
                    Player.Instance.IncreaseMana(_item.increaseMana);
                    break;
            }
            RemoveItem(_item);
        }
    }
    public void LoadInventory(List<InventoryItemData> inventoryData)
    {
        foreach (var itemData in inventoryData)
        {
            ItemData item = Resources.Load<ItemData>("Items/" + itemData.itemName);
            if (item != null&&inventoryDictionary!=null)
            {
                // 使用 itemType 查找字典中是否已存在相同的物品
                if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
                {
                    if (value.data.itemType == item.itemType) // 比较 itemType
                    {
                        value.stackSize = itemData.quantity; // 更新堆叠数量
                    }
                }
                else
                {
                    AddItem(item);
                    inventoryDictionary[item].stackSize = itemData.quantity;
                }
            }
        }
        UpdateSlotUI(); // 更新 UI
    }
    public void ClearInventory()
    {
        // 清空物品列表和字典
        InventoryItems.Clear();
        if (inventoryDictionary != null)
        inventoryDictionary.Clear();
        UpdateSlotUI();
    }
}
