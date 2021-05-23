using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBTN : MonoBehaviour
{
    [SerializeField] private int index = 0;
    [SerializeField] private Image img;
    [SerializeField] private Sprite img_null;
    [SerializeField] private ItemDeleteBTN deleteBTN;

    private bool isSet = false;
    

    // Start is called before the first frame update
    void Start()
    {
        ImageSetting();
    }

    private void OnEnable()
    {
        ItemSlot.instance.ev_listChange += ImageSetting; // Enable, Disable 관리가 안전. Disable 상태에서 Destroy 안 불린다고 함.
    }

    private void OnDisable()
    {
        ItemSlot.instance.ev_listChange -= ImageSetting;
    }

    private void ImageSetting()
    {
        if (index < ItemSlot.instance.GetSlotCount())
        {
            img.sprite = ItemSlot.instance.GetItemImg(index);
            isSet = true;
        }
        else
        {
            img.sprite = img_null;
            isSet = false;
        }
    }

    public void BTN_Set()
    {
        if (isSet)
        {
            SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
            //if (deleteBTN.GetSelected())
            //{
            //    ItemSlot.instance.DeleteItem(index);
            //    deleteBTN.BTNClicked();
            //}
            //else
            ItemSlot.instance.ItemSelected(index, gameObject.transform);
        }
    }
}
