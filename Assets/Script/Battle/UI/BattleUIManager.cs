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

    private void Event_FirstTurn(object _o, EventArgs _e)
    {
        btn_TurnEnd.SetActive(true);
    }

    private void Event_TurnStart(object _o, EventArgs _e)
    {
        btn_TurnEnd.SetActive(true);
    }

    private void Event_PlayerTurnEnd(object _o, EventArgs _e)
    {
        btn_TurnEnd.SetActive(false);
    }

    private void Event_BattleEnd(object _o, EventArgs _e)
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
        StartCoroutine(DelayedFalse(1.0f));
    }

    IEnumerator DelayedFalse(float sec)
    {
        yield return new WaitForSeconds(sec);
        isDiceOn = false;
        yield break;
    }

    public void BtnTurnEnd()
    {
        TurnManager.instance.OnPlayerTurnEnd();
    }
}
