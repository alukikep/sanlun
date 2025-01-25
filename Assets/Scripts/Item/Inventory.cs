using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<InventoryItem> InventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    private void Awake()
    {
        //避免重复
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        InventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
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
    }
}
