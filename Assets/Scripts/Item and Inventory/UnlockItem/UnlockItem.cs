using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockItem : MonoBehaviour
{
    public Ability abilityToUnlock;
    private bool isProcessed=false;

    //�Ӵ���Һ��Ի�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&&isProcessed==false)
        {
            isProcessed=true;
            Destroy(gameObject,0.1f);
        }

    }
}
