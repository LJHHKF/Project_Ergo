using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_ItemBTN : IShopBTN
{
    [Header("Item Setting")]
    [SerializeField] private Image itemImg;

    private IItem m_target;

    // Start is called before the first frame update
    void Start()
    {
        shopManager.SetItemTarget(out m_target, out price);
        stock = 1;

        itemImg.sprite = m_target.GetItemImg();
        txt_Price.text = price.ToString();
        txt_Stock.text = stock.ToString();

        detailManager = detailWindow.GetComponent<Shop_DetailWindow>();
    }

    public void BTNClicked()
    {
        detailWindow.SetActive(true);
        detailManager.SetTarget_Item(m_target, this);
    }
}
