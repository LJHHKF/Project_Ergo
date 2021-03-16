using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [Header("UI Object registration")]
    public GameObject forDice;
    public GameObject btn_TurnEnd;

    [Header("Managers registartion")]
    public TurnManager turnManager;

    private bool isDiceOn = false;

    // Start is called before the first frame update
    void Start()
    {
        forDice.SetActive(false);

        turnManager.firstTurn += () => SetBtnTurnEActive(true);
        turnManager.turnStart += () => SetBtnTurnEActive(true);
        turnManager.playerTurnEnd += () => SetBtnTurnEActive(false);
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

    public void SetBtnTurnEActive(bool isActive)
    {
        btn_TurnEnd.SetActive(isActive);
    }

    IEnumerator DelayedFalse(float sec)
    {
        yield return new WaitForSeconds(sec);
        isDiceOn = false;
        yield break;
    }
}
