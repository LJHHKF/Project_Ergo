using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [Header("UI Object registration")]
    [SerializeField] private GameObject forDice;
    [SerializeField] private GameObject btn_TurnEnd;
    [SerializeField] private GameObject panel_reward;
    private TurnManager m_TurnM;

    public bool isDiceOn
    {
        get { return _isDiceOn; }
        private set { _isDiceOn = value; }
    }
    private bool _isDiceOn = false;

    // Start is called before the first frame update
    void Start()
    {
        m_TurnM = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
        forDice.SetActive(false);
        panel_reward.SetActive(false);
        isDiceOn = false;
        m_TurnM.firstTurn += () => SetBtnTurnEActive(true);
        m_TurnM.turnStart += () => SetBtnTurnEActive(true);
        m_TurnM.playerTurnEnd += () => SetBtnTurnEActive(false);
        m_TurnM.battleEnd += () => SetBtnTurnEActive(false);
        m_TurnM.battleEnd += () => panel_reward.SetActive(true);
    }

    private void OnDestroy()
    {
        m_TurnM.firstTurn -= () => SetBtnTurnEActive(true);
        m_TurnM.turnStart -= () => SetBtnTurnEActive(true);
        m_TurnM.playerTurnEnd -= () => SetBtnTurnEActive(false);
        m_TurnM.battleEnd -= () => SetBtnTurnEActive(false);
        m_TurnM.battleEnd -= () => panel_reward.SetActive(true);
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

    public void BtnTurnEnd()
    {
        m_TurnM.OnPlayerTurnEnd();
    }
}
