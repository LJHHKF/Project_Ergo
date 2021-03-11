using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceUIManager : MonoBehaviour
{
    [Header("Object registration")]
    public Image stopPoint;
    public Image sp_forwardS;
    public Image sp_backwardS;
    public Image sp_subFill;
    public Image sp_subFill_backward;
    public Image cp_forwardS;
    public Image cp_backwardS;
    public Image cp_subFill;
    public BattleUIManager battleUIManager;
    
    [Header("Stat Setting")]
    public float spGap = 0.05f;
    public float cpGap = 0.1f;
    public float rollPower = 0.3f;

    private bool isHolded = false;
    private float minCP;
    private DiceSystemManager m_DiceManager;
    private bool isThrowed;

    private void OnEnable()
    {
        stopPoint.fillAmount = 0.0f;
        minCP = Random.Range(0.0f, 1.0f);
        if (minCP > 1.0f - cpGap)
        {
            cp_forwardS.fillAmount = minCP;
            cp_backwardS.fillAmount = 0.0f;
            cp_subFill.fillAmount = cpGap - (1.0f - minCP);
        }
        else
        {
            cp_forwardS.fillAmount = minCP;
            cp_backwardS.fillAmount = 1.0f - (minCP + cpGap);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_DiceManager = GameObject.FindGameObjectWithTag("DiceBox").GetComponent<DiceSystemManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isHolded)
        {
            stopPoint.fillAmount += rollPower * Time.deltaTime;
            if (stopPoint.fillAmount < spGap)
            {
                sp_forwardS.fillAmount = 0.0f;
                sp_backwardS.fillAmount = 1.0f - stopPoint.fillAmount;
                sp_subFill.fillAmount = 0.0f;
                sp_subFill_backward.fillAmount = spGap - stopPoint.fillAmount;
            }
            else if (stopPoint.fillAmount > 1.0f - spGap)
            {
                sp_forwardS.fillAmount = stopPoint.fillAmount - spGap;
                sp_backwardS.fillAmount = 0.0f;
                sp_subFill.fillAmount = spGap - (1.0f - stopPoint.fillAmount);
                sp_subFill_backward.fillAmount = 0.0f;
            }
            else
            {
                sp_forwardS.fillAmount = stopPoint.fillAmount - spGap;
                sp_backwardS.fillAmount = 1.0f - stopPoint.fillAmount;
                sp_subFill.fillAmount = 0.0f;
                sp_subFill_backward.fillAmount = 0.0f;
            }
            
            if(stopPoint.fillAmount >= 0.99f)
            {
                stopPoint.fillAmount = 0.0f;
            }
        }
    }

    public void BtnHolded()
    {
        isHolded = true;
    }

    public void BtnUnHolded()
    {
        isHolded = false;
        m_DiceManager.ActiveDice(out isThrowed);
        if(!isThrowed)
        {
            Debug.LogError("주사위를 굴리지 못했습니다.");
            battleUIManager.OffDiceSystem();
        }
    }
}
