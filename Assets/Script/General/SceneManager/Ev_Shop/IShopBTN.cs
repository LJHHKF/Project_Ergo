using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IShopBTN : MonoBehaviour
{
    [Header("Base Setting")]
    [SerializeField] protected ShopSceneManager shopManager;
    [SerializeField] protected GameObject detailWindow;
    [Space]
    [SerializeField] protected Text txt_Price;
    [SerializeField] protected Text txt_Stock;

    protected int price;
    protected int stock = 1;
    protected Shop_DetailWindow detailManager;
    
    public int GetStock()
    {
        return stock;
    }

    public int GetPrice()
    {
        return price;
    }

    public void DownStock()
    {
        stock -= 1;
        txt_Stock.text = stock.ToString();
    }
}
