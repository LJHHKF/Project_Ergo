using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System;

public class RewardUIManager : MonoBehaviour
{
    [SerializeField] private int itemOnPer_int = 30; // max 100
    [SerializeField] private Card_UI cardUIManager;
    private int card_ID;

    [SerializeField] private GameObject[] btns;
    [SerializeField] private Image soulImage;
    [SerializeField] private Text soulText;
    [SerializeField] private Image itemImage;
    [SerializeField] private Text itemText;
    private bool[] isSelected;
    private int soulReward;

    private event Action m_onDisable;

    private void Awake()
    {
        isSelected = new bool[btns.Length];
    }

    private void OnEnable()
    {
        CardPack.instance.ResetCanList();
        Card_Base m_card = CardPack.instance.GetRandomCard_isntConfirm().GetComponent<Card_Base>();
        cardUIManager.SetTargetCard(m_card, true);
        m_onDisable += () => CardPack.instance.TempHadCntUpDown(m_card.GetCardID(), false);

        StringBuilder m_sb = new StringBuilder();
        m_sb.Append("소울 보상(가구현)\n");
        soulReward = EnemiesManager.instance.GetInitCnt() * 100;
        m_sb.Append(soulReward.ToString());
        soulText.text = m_sb.ToString();

        for (int i = 0; i < isSelected.Length; i++)
        {
            int _i = i;
            isSelected[_i] = true;
        }

        int rand = UnityEngine.Random.Range(0, 99);
        if (rand < itemOnPer_int)
            btns[2].SetActive(true);
        else
        {
            btns[2].SetActive(false);
        }
    }

    private void OnDisable()
    {
        m_onDisable?.Invoke();
    }

    public void BtnConfirm()
    {
        if(isSelected[0])
            cardUIManager.AddToDeckTargetedCard();
        if (isSelected[1])
            PlayerMoneyManager.instance.AcquiredSoul(soulReward);

        StartCoroutine(DeleayedNextStage());
    }

    private void BtnSelectClick(int index)
    {
        // 0: card, 1 : sout, 2: item
        if (!isSelected[index])
        {
            isSelected[index] = true;
            switch(index)
            {
                case 0:
                    cardUIManager.SetImagesAlpha(1.0f);
                    break;
                case 1:
                    soulImage.color = new Color(soulImage.color.r, soulImage.color.g, soulImage.color.b, 1.0f);
                    soulText.color = new Color(soulText.color.r, soulText.color.g, soulText.color.b, 1.0f);
                    break;
                case 2:
                    itemImage.color = new Color(itemImage.color.r, itemImage.color.g, itemImage.color.b, 1.0f);
                    itemText.color = new Color(itemText.color.r, itemText.color.g, itemText.color.b, 1.0f);
                    break;
            }
        }
        else if(isSelected[index])
        {
            isSelected[index] = false;
            switch (index)
            {
                case 0:
                    cardUIManager.SetImagesAlpha(0.5f);
                    break;
                case 1:
                    soulImage.color = new Color(soulImage.color.r, soulImage.color.g, soulImage.color.b, 0.5f);
                    soulText.color = new Color(soulText.color.r, soulText.color.g, soulText.color.b, 0.5f);
                    break;
                case 2:
                    itemImage.color = new Color(itemImage.color.r, itemImage.color.g, itemImage.color.b, 0.5f);
                    itemText.color = new Color(itemText.color.r, itemText.color.g, itemText.color.b, 0.5f);
                    break;
            }
        }
    }

    public void BtnCard()
    {
        BtnSelectClick(0);
    }

    public void BtnSoul()
    {
        BtnSelectClick(1);
    }

    public void BtnItem()
    {
        BtnSelectClick(2);
    }

    IEnumerator DeleayedNextStage()
    {
        yield return new WaitForSeconds(1.0f);
        LoadManager.instance.LoadNextStage();
        yield break;
    }
}
