using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_DetailWindow : MonoBehaviour
{
    [Header("Object Registration_out")]
    [SerializeField] private ShopSceneManager shopManager;
    [SerializeField] private GameObject warningWindow;
    private Shop_WarningWindow warningManager;
    
    [Header("Object Registration_in")]
    [SerializeField] private Text product_name;
    [SerializeField] private Text product_description;
    [SerializeField] private GameObject itemImg_obj;
    [SerializeField] private Image itemImg;
    [SerializeField] private GameObject card_obj;
    [SerializeField] private Card_UI card_ui;
    [SerializeField] private Image btn_purchase_img;
    [SerializeField] private Text btn_purchase_text;

    [Header("Pos Object Registration")]
    [SerializeField] private Transform t_left;
    [SerializeField] private Transform t_right;
    [SerializeField] private Transform t_top;
    [SerializeField] private Transform t_down;

    private IShopBTN shopBTN;
    private int itemID;
    private bool hadWarning = false;
    private int price;
    private bool can_price = true;
    private bool can_stock = true;
    private bool isCard = false;
    private bool isItem = false;
    private bool isOn = false;

    private void Start()
    {
        //itemImg = itemImg_obj.GetComponent<Image>();
        //card_ui = card_obj.GetComponent<Card_UI>();
        warningManager = warningWindow.GetComponent<Shop_WarningWindow>();
        itemImg_obj.SetActive(false);
        card_obj.SetActive(false);
        warningWindow.SetActive(false);
    }

    private void OnDisable()
    {
        shopBTN = null;
        isOn = false;
    }

    private void Update()
    {
        if (!hadWarning)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Input.mousePosition;
                if (t_left.position.x > mousePos.x
                    || t_right.position.x < mousePos.x
                    || t_top.position.y < mousePos.y
                    || t_down.position.y > mousePos.y)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public void SetTarget_card(Card_UI _target, IShopBTN _btn)
    {
        if (!isOn)
        {
            isOn = true;

            itemImg_obj.SetActive(false);
            isItem = false;
            card_obj.SetActive(true);
            isCard = true;
            itemID = -1;

            card_ui.SetTargetCard(_target.GetTargetCard(), true);


            shopBTN = _btn;
            price = shopBTN.GetPrice();
            if (price > PlayerMoneyManager.instance.soul)
                can_price = false;
            else
                can_price = true;

            if (shopBTN.GetStock() > 0)
                can_stock = true;
            else
                can_stock = false;
            product_name.text = card_ui.GetCardName();
            product_description.text = card_ui.GetCardText();

            SetBTNAlpha();
        }
    }

    public void SetTarget_Item(IItem _target, IShopBTN _btn)
    {
        if (!isOn)
        {
            isOn = true;

            itemImg_obj.SetActive(true);
            isItem = true;
            card_obj.SetActive(false);
            isCard = false;
            itemID = _target.GetID();

            shopBTN = _btn;
            price = shopBTN.GetPrice();
            if (price > PlayerMoneyManager.instance.soul)
                can_price = false;
            else
                can_price = true;

            if (shopBTN.GetStock() > 0)
                can_stock = true;
            else
                can_stock = false;

            itemImg.sprite = _target.GetItemImg();
            product_name.text = _target.GetItemName();
            product_description.text = _target.GetItemText();

            SetBTNAlpha();
        }
    }

    public void SetHadWarning(bool _value)
    {
        hadWarning = _value;
    }

    public void SetBTNAlpha()
    {
        //float r = btn_purchase_img.color.r;
        //float g = btn_purchase_img.color.g;
        //float b = btn_purchase_img.color.b;
        if (!can_stock
            || !can_price
            || (isItem && !ItemSlot.instance.GetCanAdd()))
        {
            btn_purchase_img.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            btn_purchase_text.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            btn_purchase_text.text = "구매 불가";
        }
        else
        {
            btn_purchase_img.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            btn_purchase_img.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            btn_purchase_text.text = "구매";
        }
    }

    public void BTNPurchase()
    {
        if(!can_stock)
        {
            warningWindow.SetActive(true);
            warningManager.SetWarning_LessStock();
        }
        else if(!can_price)
        {
            warningWindow.SetActive(true);
            warningManager.SetWarning_LessSoul();
        }
        else if(isItem && !ItemSlot.instance.GetCanAdd())
        {
            warningWindow.SetActive(true);
            warningManager.SetWarning_LessInventory();
        }
        else
        {
            if (PlayerMoneyManager.instance.UseSoul(price))
            {
                
                if (isCard)
                {
                    card_ui.AddToCardPackTargetedCard();
                }
                else if (isItem)
                {
                    ItemSlot.instance.AddItem(itemID);
                }
                shopBTN.DownStock();
                shopManager.Bought();
                gameObject.SetActive(false);
            }
        }
    }
}
