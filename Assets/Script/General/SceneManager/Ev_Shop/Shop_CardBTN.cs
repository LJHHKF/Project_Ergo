using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_CardBTN : IShopBTN
{
    [Header("Card Setting")]
    [SerializeField] private Card_UI m_Card;

    private Card_Base m_target;
    //private int index;

    // Start is called before the first frame update
    void Start()
    {
        shopManager.SetCardTarget(out m_target, out price/*, out index*/);
        stock = 1;

        m_Card.SetTargetCard(m_target, true);
        txt_Price.text = price.ToString();
        txt_Stock.text = stock.ToString();

        detailManager = detailWindow.GetComponent<Shop_DetailWindow>();
    }

    public void BTNClicked()
    {
        detailWindow.SetActive(true);
        detailManager.SetTarget_card(m_Card, this);
    }
}
