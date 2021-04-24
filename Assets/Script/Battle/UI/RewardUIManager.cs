using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RewardUIManager : MonoBehaviour
{
    [SerializeField] private int itemOnPer_int = 30; // max 100
    [SerializeField] private Card_UI cardUIManager;

    [SerializeField] private GameObject[] btns;
    [SerializeField] private GameObject[] selectEfs;
    private bool[] isSelected;
    private bool[] isDiscarded;

    private event Action m_onDisable;
    //private bool isAlreadySelected = false;

    private void Awake()
    {
        isSelected = new bool[selectEfs.Length];
        isDiscarded = new bool[selectEfs.Length];
        for (int i = 0; i < isSelected.Length; i++)
            isSelected[i] = false;
    }

    private void OnEnable()
    {
        CardPack.instance.ResetCanList();
        Card_Base m_card = CardPack.instance.GetRandomCard_isntConfirm().GetComponent<Card_Base>();
        cardUIManager.SetTargetCard(m_card, true);
        m_onDisable += () => CardPack.instance.TempHadCntUpDown(m_card.GetCardID(), false);

        for (int i = 0; i < selectEfs.Length; i++)
        {
            selectEfs[i].SetActive(false);
            isDiscarded[i] = false;
        }

        int rand = UnityEngine.Random.Range(0, 99);
        if (rand < itemOnPer_int)
            btns[2].SetActive(true);
        else
        {
            btns[2].SetActive(false);
            isDiscarded[2] = true;
        }
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
        if(!isDiscarded[0])
            cardUIManager.AddToDeckTargetedCard();

        StartCoroutine(DeleayedNextStage());
    }

    public void BtnDiscard()
    {
        for(int i = 0; i < isSelected.Length; i++)
        {
            if(isSelected[i])
            {
                isDiscarded[i] = true;
                selectEfs[i].SetActive(false);
                btns[i].SetActive(false);
            }
        }
    }

    private void BtnSelectClick(int index)
    {
        // 0: card, 1 : sout, 2: item
        if (!isSelected[index])
        {
            selectEfs[index].SetActive(true);
            isSelected[index] = true;
        }
        else if(isSelected[index])
        {
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
