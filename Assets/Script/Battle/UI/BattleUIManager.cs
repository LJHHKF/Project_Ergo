using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BattleUIManager : MonoBehaviour
{
    [Header("UI Object registration")]
    [SerializeField] private GameObject forDice;
    [SerializeField] private GameObject btn_TurnEnd;
    [SerializeField] private GameObject panel_reward;
    [SerializeField] private GameObject dice_Result;
    [SerializeField] private GameObject card_enlarge;
    [SerializeField] private GameObject cardsListWindow;
    [SerializeField] private CardListManager cardListM;
    [SerializeField] private GameObject statusDetailArea;

    public bool isDiceOn
    {
        get { return _isDiceOn; }
        private set { _isDiceOn = value; }
    }
    private bool _isDiceOn = false;

    // Start is called before the first frame update
    void Start()
    {
        forDice.SetActive(false);
        panel_reward.SetActive(false);
        dice_Result.SetActive(false);
        card_enlarge.SetActive(false);
        cardsListWindow.SetActive(false);
        statusDetailArea.SetActive(false);
        isDiceOn = false;
        TurnManager.instance.firstTurn += Event_FirstTurn;
        TurnManager.instance.turnStart += Event_TurnStart;
        TurnManager.instance.playerTurnEnd += Event_PlayerTurnEnd;
        TurnManager.instance.battleEnd += Event_BattleEnd;

        GameMaster.instance.OnBattleStageStart();
    }

    private void OnDestroy()
    {
        TurnManager.instance.firstTurn -= Event_FirstTurn;
        TurnManager.instance.turnStart -= Event_TurnStart;
        TurnManager.instance.playerTurnEnd -= Event_PlayerTurnEnd;
        TurnManager.instance.battleEnd -= Event_BattleEnd;
    }

    private void Event_FirstTurn()
    {
        btn_TurnEnd.SetActive(true);
    }

    private void Event_TurnStart()
    {
        btn_TurnEnd.SetActive(true);
    }

    private void Event_PlayerTurnEnd()
    {
        btn_TurnEnd.SetActive(false);
    }

    private void Event_BattleEnd()
    {
        btn_TurnEnd.SetActive(false);
        panel_reward.SetActive(true);
    }

    public void OnDiceSysetm()
    {
        if (!isDiceOn)
        {
            forDice.SetActive(true);
            isDiceOn = true;
        }
    }

    public void OffDiceSystem()
    {
        forDice.SetActive(false);
        StartCoroutine(DelayedisDiceOnFalse(1.0f));
    }

    public void OnDiceRes()
    {
        dice_Result.SetActive(true);
        StartCoroutine(DelayedUnActive(dice_Result, 1.0f));
    }

    IEnumerator DelayedisDiceOnFalse(float sec)
    {
        yield return new WaitForSeconds(sec);
        isDiceOn = false;
        yield break;
    }

    IEnumerator DelayedUnActive(GameObject _target, float sec)
    {
        //일반 변수는 ref로 못 가져옴. 그러나 애초에 클래스형의 자료들은 일종의 ref 개념을 포함하고 있다고 알며, 잘 동작함. 
        yield return new WaitForSeconds(sec);
        _target.SetActive(false);
        yield break;
    }

    public void OnEnlargeCard(Card_Base _target)
    {
        card_enlarge.SetActive(true);
        card_enlarge.GetComponent<Card_UI>().SetTargetCard(_target, false);
    }

    public void OffEnlargeCard()
    {
        card_enlarge.SetActive(false);
    }

    public void OnCardListWindow(bool _isDeck)
    {
        cardsListWindow.SetActive(true);
        cardListM.InputList(_isDeck);
    }

    public void OffCardListWindow()
    {
        cardsListWindow.SetActive(false);
    }

    public void BtnTurnEnd()
    {
        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
        TurnManager.instance.OnPlayerTurnEnd();
    }

    public GameObject GetStatusDetailArea()
    {
        return statusDetailArea;
    }
}
