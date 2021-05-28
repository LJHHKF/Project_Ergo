using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseMinXSet : MonoBehaviour
{
    // 원래 Start에 했었으나, 간간이 오류내서 조금 성능 낮아지더라도 안정적인 구동을 위해 Enable로 하는게 낫겠다 싶어서 수정.
    void OnEnable()
    {
        GameObject.FindGameObjectWithTag("ItemSlot").GetComponent<ItemSlot>().SetUseMinX(gameObject.transform.position.x);
    }
}
