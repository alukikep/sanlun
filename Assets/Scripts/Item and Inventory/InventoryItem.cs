using System;

[Serializable]

public class InventoryItem 
{
    public ItemData data;
    public int stackSize;

    //Ϊÿһ������Ʒ������һ�������ġ���桱����ͳ�ƶѵ���
    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }
    public void AddStack()=>stackSize++;
    public void RemoveStack()=>stackSize--;
}
