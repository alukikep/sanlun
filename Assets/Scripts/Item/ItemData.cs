using UnityEngine;

//在右键中创建一个新的菜单
[CreateAssetMenu(fileName ="New Item Data",menuName ="Data/Item")]
//可脚本化对象是一个数据容器，可用于保存大量数据，独立于类的实例
public class ItemData : ScriptableObject 
{
    public string itemName;
    public Sprite icon;
}
