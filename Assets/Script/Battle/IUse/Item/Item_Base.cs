using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item;

namespace Item
{
    public enum ItemType
    {
        Throw,
        Drink
    }
}

public abstract class Item_Base : MonoBehaviour, IItem
{
    [Header("Item Base Setting")]
    [SerializeField] protected int itemID = 0;
    [SerializeField] protected string itemName;
    [SerializeField] protected int itemPower = 1;
    [SerializeField] protected bool isNonTarget = false;
    [SerializeField] protected ItemType type;
    [SerializeField] protected Sprite img;
    [TextArea]
    [SerializeField] protected string itemText;

    protected GameObject target;
    protected int slotIndex = 0;
    protected float min_x = 0;
    protected bool ready = false;
    protected bool selected = false;
    protected Transform btn_transform;

    protected virtual void Start()
    {
        min_x = ItemSlot.instance.GetUseMinX();
    }

    public virtual void Use(int _dummy)
    {
        ItemSlot.instance.DeleteItem(slotIndex);
    }

    public abstract void SetTarget(GameObject _target);

    public virtual IUse Selected()
    {
        selected = true;
        return this;
    }

    public void UnSetSelected()
    {
        selected = false;
    }

    public int GetID()
    {
        return itemID;
    }

    public Sprite GetItemImg()
    {
        return img;
    }

    public string GetItemText()
    {
        return itemText;
    }

    public int GetUseType()
    {
        return 1;
    }

    public bool GetIsNonTarget()
    {
        return isNonTarget;
    }

    public bool GetReady()
    {
        return ready;
    }

    public void SetSlotIndex(int _index)
    {
        slotIndex = _index;
    }

    public bool Dragged(Vector2 mousePos, LineDrawer liner)
    {
        liner.SetLine_Canvas(btn_transform, mousePos);

        Vector2 mouseP = Camera.main.WorldToScreenPoint(mousePos);
        if(mouseP.x > min_x)
        {
            ready = true;
            return false;
        }
        else
        {
            ready = false;
            return true;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void SetBtnPos(Transform t_btn)
    {
        btn_transform = t_btn;
    }
}
