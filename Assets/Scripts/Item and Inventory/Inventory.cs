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

        itemSlot=inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }
    private void UpdateSlotUI()
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
        //在背包中，相同物品堆叠
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        //新物品新建
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            InventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
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
                case ItemData.ItemType.SpeedPotion:
                    Player.Instance.IncreaseSpeedRate(_item.speedEnhanceRate, _item.Duration);
                    break;
            }
            RemoveItem(_item);
        }
    }
    public void LoadInventory(List<InventoryItemData> inventoryData)
    {
        foreach (var itemData in inventoryData)
        {
            ItemData item = Resources.Load<ItemData>("Items/" + itemData.itemName); // 需要根据实际情况调整路径
            if (item != null)
            {
                AddItem(item);
                inventoryDictionary[item].stackSize = itemData.quantity;
            }
        }
    }
    public void SaveInventory()
    {
        List<InventoryItemData> inventoryItems = new List<InventoryItemData>();
        foreach (var item in InventoryItems)
        {
            InventoryItemData itemData = new InventoryItemData
            {
                itemName = item.data.itemName,
                iconPath = item.data.icon.name,
                quantity = item.stackSize
            };
            inventoryItems.Add(itemData);
        }

        string json = JsonUtility.ToJson(inventoryItems);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/inventoryData.json", json);
    }

    public void LoadInventory()
    {
        string path = Application.persistentDataPath + "/inventoryData.json";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            List<InventoryItemData> inventoryData = JsonUtility.FromJson<List<InventoryItemData>>(json);

            foreach (var itemData in inventoryData)
            {
                ItemData item = Resources.Load<ItemData>("Items/" + itemData.itemName);
                if (item != null)
                {
                    AddItem(item);
                    inventoryDictionary[item].stackSize = itemData.quantity;
                }
            }
        }
        UpdateSlotUI();
    }
}
