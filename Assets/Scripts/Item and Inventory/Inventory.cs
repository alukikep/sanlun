using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<InventoryItem> InventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;


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
        InventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        itemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 空格键被按下，但不执行任何操作
            return;
        }

    }
    public void UpdateSlotUI()
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
                case ItemData.ItemType.HealingPotion:
                    Player.Instance.Heal(_item.healAmount);
                    break;
                case ItemData.ItemType.HealthMaxPotion:
                    Player.Instance.IncreaseMaxHealth(_item.increaseHealthMax);
                    break;
                case ItemData.ItemType.EnternalAttackPotion:
                    Player.Instance.IncreaseEnternalAttack(_item.increaseAttack);
                    break;
                case ItemData.ItemType.TemporaryAttackPotion:
                    Player.Instance.IncreaseTemporaryAttack(_item.increaseAttack,_item.Duration);
                    break;
                case ItemData.ItemType.ManaCapacityPotion:
                    Player.Instance.IncreaseMaxMana(_item.increaseManaMax);
                    break;
                case ItemData.ItemType.ManaRestorePotion:
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
            if (item != null)
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
    // 新增方法：清空物品栏
    public void ClearInventory()
    {
        // 清空物品列表和字典
        InventoryItems.Clear();
        inventoryDictionary.Clear();

        // 更新 UI，清空所有物品槽
        UpdateSlotUI();
    }
}
