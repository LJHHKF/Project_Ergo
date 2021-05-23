using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class Card_UI : MonoBehaviour
{

    private int cardID = 0;
    private string cardName;
    private int cost = 1;
    private int fixP = 1;
    private float flucPRate = 1.0f;
    private Sprite cardImage;
    private Sprite cardOuterImage;
    private string cardText;

    [SerializeField] private TextMeshProUGUI text_cost;
    [SerializeField] private TextMeshProUGUI text_plain;
    [SerializeField] private TextMeshProUGUI text_name;
    [SerializeField] private Image image_card;
    [SerializeField] private Image image_cardOuter;

    private Card_Base target_Card;

    public void SetTargetCard(Card_Base _target, bool _isShop)
    {
        target_Card = _target;
        if (_isShop)
            InitSetting_Shop();
        else
            InitSetting_inGame();
    }

    public Card_Base GetTargetCard()
    {
        return target_Card;
    }

    private void InitSetting_Shop()
    {
        target_Card.CopyUIInfo(out cardID,out cardName ,out cost, out fixP, out flucPRate, out cardImage, out cardOuterImage, out cardText);
        text_cost.text = cost.ToString();
        text_name.text = cardName;
        image_card.sprite = cardImage;
        image_cardOuter.sprite = cardOuterImage;

        StringBuilder sb = new StringBuilder(cardText);
        sb.Replace("()", $"({fixP} + 스탯보정치)");
        sb.Replace("(변동치)", flucPRate.ToString());
        text_plain.text = sb.ToString();
    }

    private void InitSetting_inGame()
    {
        target_Card.CopyUIInfo(out cardID, out cardName, out cost, out fixP, out flucPRate, out cardImage, out cardOuterImage, out cardText);
        text_cost.text = cost.ToString();
        text_name.text = cardName;
        image_card.sprite = cardImage;
        image_cardOuter.sprite = cardOuterImage;

        text_plain.text = target_Card.GetCurPlainText();
    }

    public void AddToDeckTargetedCard()
    {
        GameObject m_deck = GameObject.FindGameObjectWithTag("CDeck");
        List<GameObject> tempList = new List<GameObject>();
        CardPack.instance.AddCard_Object(cardID, m_deck.transform, ref tempList);
        if (tempList.Count > 0)
            DeckManager.instance.AddToDeck_NonList(tempList[0]);
    }

    public void AddToCardPackTargetedCard()
    {
        CardPack.instance.AddCard_OnlyHadData(cardID);
    }

    public void SetImagesAlpha(float _alpha)
    {
        text_cost.color = new Color(text_cost.color.r, text_cost.color.g, text_cost.color.b, _alpha);
        text_name.color = new Color(text_name.color.r, text_name.color.g, text_name.color.b, _alpha);
        text_plain.color = new Color(text_plain.color.r, text_plain.color.g, text_plain.color.b, _alpha);
        image_card.color = new Color(image_card.color.r, image_card.color.g, image_card.color.b, _alpha);
        image_cardOuter.color = new Color(image_cardOuter.color.r, image_cardOuter.color.g, image_cardOuter.color.b, _alpha);
    }

    public void UnDoTransparent()
    {
        text_cost.color = new Color(text_cost.color.r, text_cost.color.g, text_cost.color.b, 1.0f);
        text_name.color = new Color(text_name.color.r, text_name.color.g, text_name.color.b, 1.0f);
        text_plain.color = new Color(text_plain.color.r, text_plain.color.g, text_plain.color.b, 1.0f);
        image_card.color = new Color(image_card.color.r, image_card.color.g, image_card.color.b, 1.0f);
        image_cardOuter.color = new Color(image_cardOuter.color.r, image_cardOuter.color.g, image_cardOuter.color.b, 1.0f);
    }

    public string GetCardName()
    {
        return cardName;
    }

    public string GetCardText()
    {
        return cardText;
    }

    public int GetCardID()
    {
        return cardID;
    }
}
