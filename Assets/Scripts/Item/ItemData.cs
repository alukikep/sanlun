using UnityEngine;

//���Ҽ��д���һ���µĲ˵�
[CreateAssetMenu(fileName ="New Item Data",menuName ="Data/Item")]
//�ɽű���������һ�����������������ڱ���������ݣ����������ʵ��
public class ItemData : ScriptableObject 
{
    public string itemName;
    public Sprite icon;
}
