using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RestSceneManager : MonoBehaviour
{
    public event Action ev_otherSelect;
    public event Action ev_DeleteConfirm;

    [SerializeField] private GameObject cardsListWindow;
    [SerializeField] private CardListManager listM;
    [SerializeField] private GameObject selectedCardWindow;

    private void Start()
    {
        selectedCardWindow.SetActive(false);
        cardsListWindow.SetActive(false);
    }

    public void BtnRest()
    {
        int fullHealth = CStatManager.instance.fullHealth_pure + CStatManager.instance.endurance;
        int restoreValue = Mathf.RoundToInt(fullHealth * 0.3f);
            
        if (CStatManager.instance.health + restoreValue >= fullHealth)
        {
            restoreValue = (fullHealth) - CStatManager.instance.health;
            CStatManager.instance.HealthPointUpdate(CStatManager.instance.health + restoreValue);
        }
        else
        {
            CStatManager.instance.HealthPointUpdate(CStatManager.instance.health + restoreValue);
        }

        LoadManager.instance.LoadNextStage();
    }

    public void OnEventOtherSelect()
    {
        ev_otherSelect?.Invoke();
    }

    public void BtnDiscard()
    {
        cardsListWindow.SetActive(true);
        listM.InputList_RestV();
    }

    public void SetSelected(int _index)
    {
        if (!selectedCardWindow.activeSelf)
            selectedCardWindow.SetActive(true);
        selectedCardWindow.transform.Find("Card_UI").GetComponent<Card_UI>().SetTargetCard(listM.GetInputInfo(_index), false);
    }

    public void UnSetSelected()
    {
        selectedCardWindow.SetActive(false);
    }

    public void BtnDiscardConfirm()
    {
        ev_DeleteConfirm?.Invoke();
        LoadManager.instance.LoadNextStage();
    }

    public void BtnDiscardClose()
    {
        cardsListWindow.SetActive(false);
    }
}
