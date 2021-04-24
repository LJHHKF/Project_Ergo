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
        DeckManager.instance.AddToDeck_NonList(tempList[0]);
    }

    public void DeleteToDeckTargetedCard()
    {

    }
}
