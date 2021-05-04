using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class RestSceneManager : MonoBehaviour
{
    public UnityEvent ev_otherSelect;
    public event Func<bool> ev_DeleteConfirm;

    [SerializeField] private GameObject cardsListWindow;
    [SerializeField] private CardListManager listM;
    [SerializeField] private GameObject selectedCardWindow;
    [SerializeField] private GameObject warningWindow;
    [SerializeField] private Text warningBodyText;

    private void Start()
    {
        selectedCardWindow.SetActive(false);
        cardsListWindow.SetActive(false);
        warningWindow.SetActive(false);
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
        if (ev_DeleteConfirm != null)
        {
            if (!ev_DeleteConfirm.Invoke())
            {
                string message = "삭제 요건을 만족시키지 못했습니다.\n예시상황) 덱의 카드 수가 10장 이하인 상태에서 카드 삭제 시도";
                WarningWindowOpen(message);
            }
            else
            {
                LoadManager.instance.LoadNextStage();
            }
        }
        else
        {
            string message = "등록된 삭제 행동이 없어서 확인이 취소됬습니다.";
            WarningWindowOpen(message);
        }
    }

    public void BtnDiscardClose()
    {
        cardsListWindow.SetActive(false);
    }

    public void BtnWarningClose()
    {
        warningWindow.SetActive(false);
    }

    private void WarningWindowOpen(string _message)
    {
        warningWindow.SetActive(true);
        warningBodyText.text = _message;
    }
}
