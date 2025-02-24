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
        //�����ظ�
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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
                // �����Ʒ������Ʒ��������Ʒ��Ϣ
                itemSlot[i].UpdateSlot(InventoryItems[i]);
            }
            else
            {
                // �����Ʒ��û����Ʒ�������ʾ
                itemSlot[i].UpdateSlot(null);
            }
        }
    }
    public void AddItem(ItemData _item)
    {
        //�ڱ����У���ͬ��Ʒ�ѵ�
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        //����Ʒ�½�
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
            //�����Ʒ����Ϊ0���ڱ�����ɾ�������Ʒ
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
    // ʹ����Ʒ
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
            ItemData item = Resources.Load<ItemData>("Items/" + itemData.itemName); // ��Ҫ����ʵ���������·��
            if (item != null)
            {
                AddItem(item);
                inventoryDictionary[item].stackSize = itemData.quantity;
            }
        }
    }
}
