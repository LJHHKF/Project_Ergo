using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RewardUIManager : MonoBehaviour
{
    [SerializeField] private GameObject itemBtn;
    [SerializeField] private int itemOnPer_int = 30; // max 100
    [SerializeField] private Card_UI cardUIManager;

    [SerializeField] private GameObject[] selectEfs;
    private bool[] isSelected;

    private event Action m_onDisable;
    private int selectedNum = -1;

    private void Awake()
    {
        isSelected = new bool[selectEfs.Length];
        for (int i = 0; i < isSelected.Length; i++)
            isSelected[i] = false;
    }

    private void OnEnable()
    {
        selectedNum = -1;
        int rand = UnityEngine.Random.Range(0, 99);
        if (rand < itemOnPer_int)
            itemBtn.SetActive(true);
        else
            itemBtn.SetActive(false);

        CardPack.instance.ResetCanList();
        Card_Base m_card = CardPack.instance.GetRandomCard_isntConfirm().GetComponent<Card_Base>();
        cardUIManager.SetTargetCard(m_card);
        m_onDisable += () => CardPack.instance.TempHadCntUpDown(m_card.GetCardID(), false);
        Debug.Log("CardID:" + m_card.GetCardID());

        for (int i = 0; i < selectEfs.Length; i++)
            selectEfs[i].SetActive(false);
    }

    private void OnDisable()
    {
        if (m_onDisable != null)
        {
            m_onDisable();
        }
    }

    public void BtnConfirm()
    {
        if(selectedNum != -1)
        {
            switch(selectedNum)
            {
                case 0:
                    cardUIManager.AddToDeckTargetedCard();
                    break;
            }
        }
        StartCoroutine(DeleayedNextStage());
    }

    public void BtnDiscard()
    {
        LoadManager.instance.LoadNextStage();
    }

    private void BtnSelectClick(int index)
    {
        // 0: card, 1 : sout, 2: item
        if (isSelected[index] == false)
        {
            selectedNum = index;
            selectEfs[index].SetActive(true);
            isSelected[index] = true;
        }
        else
        {
            selectedNum = -1;
            selectEfs[index].SetActive(false);
            isSelected[index] = false;
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
