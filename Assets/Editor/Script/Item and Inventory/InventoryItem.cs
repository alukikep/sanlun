using System;

[Serializable]

public class InventoryItem 
{
    public ItemData data;
    public int stackSize;

    //为每一个“物品”创建一个独立的“库存”用来统计堆叠数
    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }
    public void AddStack()=>stackSize++;
    public void RemoveStack()=>stackSize--;
}
