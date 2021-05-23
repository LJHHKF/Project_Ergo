using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseMinXSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("ItemSlot").GetComponent<ItemSlot>().SetUseMinX(gameObject.transform.position.x);
    }
}
