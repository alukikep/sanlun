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
            DontDestroyOnLoad(gameObject); // ȷ���ڳ����л�ʱ������
        }
        else
        {
            Destroy(gameObject); // �����ظ���ʵ��
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
            // �ո�������£�����ִ���κβ���
            return;
        }

    }
    public void UpdateSlotUI()
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
        // ʹ�� itemType �����ֵ����Ƿ��Ѵ�����ͬ����Ʒ
        foreach (var kvp in inventoryDictionary)
        {
            if (kvp.Key.itemType == _item.itemType) // �Ƚ� itemType
            {
                kvp.Value.AddStack(); // ���Ӷѵ�����
                UpdateSlotUI();
                return;
            }
        }

        // ���û���ҵ��������µ���Ʒ
        InventoryItem newItem = new InventoryItem(_item);
        InventoryItems.Add(newItem);
        inventoryDictionary.Add(_item, newItem);
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
                // ʹ�� itemType �����ֵ����Ƿ��Ѵ�����ͬ����Ʒ
                if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
                {
                    if (value.data.itemType == item.itemType) // �Ƚ� itemType
                    {
                        value.stackSize = itemData.quantity; // ���¶ѵ�����
                    }
                }
                else
                {
                    AddItem(item);
                    inventoryDictionary[item].stackSize = itemData.quantity;
                }
            }
        }
        UpdateSlotUI(); // ���� UI
    }
    // ���������������Ʒ��
    public void ClearInventory()
    {
        // �����Ʒ�б���ֵ�
        InventoryItems.Clear();
        inventoryDictionary.Clear();

        // ���� UI�����������Ʒ��
        UpdateSlotUI();
    }
}
