using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseMinXSet : MonoBehaviour
{
    // ���� Start�� �߾�����, ������ �������� ���� ���� ���������� �������� ������ ���� Enable�� �ϴ°� ���ڴ� �; ����.
    void OnEnable()
    {
        GameObject.FindGameObjectWithTag("ItemSlot").GetComponent<ItemSlot>().SetUseMinX(gameObject.transform.position.x);
    }
}
