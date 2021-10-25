using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceUIManager : MonoBehaviour
{
    [Header("Object registration")]
    [SerializeField] private Image stopPoint;
    [SerializeField] private Image sp_forwardS;
    [SerializeField] private Image sp_backwardS;
    [SerializeField] private Image cp_forwardS;
    [SerializeField] private Image cp_backwardS;
    [SerializeField] private BattleUIManager battleUIManager;
    [SerializeField] private DiceSystemManager m_DiceManager;

    //SP: StopPoint, CP: Catch Point

    [Header("Stat Setting")]
    [SerializeField] private float spGap = 0.05f;
    [SerializeField] private float cpGap = 0.1f;
    [SerializeField] private float gaugePower = 0.3f;

    private bool isHolded = false;
    private bool reversed = false;
    private float minCP;
    
    //private bool isThrowed;

    private void OnEnable()
    {
        stopPoint.fillAmount = 0.0f;
        minCP = Random.Range(0.0f, 1.0f - cpGap);
        cp_forwardS.fillAmount = minCP;
        cp_backwardS.fillAmount = 1.0f - (minCP + cpGap);
        sp_forwardS.fillAmount = stopPoint.fillAmount;
        sp_backwardS.fillAmount = 1.0f - (stopPoint.fillAmount + spGap);
    }

    // Update is called once per frame
    void Update()
    {
        if(isHolded)
        {
            if (!reversed)
            {
                stopPoint.fillAmount += gaugePower * Time.deltaTime;
            }
            else if(reversed)
            {
                stopPoint.fillAmount -= gaugePower * Time.deltaTime;
            }

            sp_forwardS.fillAmount = stopPoint.fillAmount;
            sp_backwardS.fillAmount = 1.0f - (stopPoint.fillAmount + spGap);

            if (!reversed)
            {
                if (stopPoint.fillAmount > 1.0f - spGap)
                {
                    reversed = true;
                }
            }
            else if (reversed)
            {
                if (stopPoint.fillAmount == 0.0f)
                {
                    reversed = false;
                }
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
        if(CheckCP())
        {
            float rand = Random.Range(0, 9);
            if(rand <= 2f) //0, 1, 2
            {
                m_DiceManager.OnSixDice();
            }
        }
        bool isThrowed;
        m_DiceManager.ActiveDice(out isThrowed);
        if(!isThrowed)
        {
            Debug.LogError("주사위를 굴리지 못했습니다.");
            battleUIManager.OffDiceSystem();
        }
    }

    private bool CheckCP()
    {
        float sp_min1, sp_max1;
        sp_min1 = stopPoint.fillAmount;
        sp_max1 = stopPoint.fillAmount + spGap;
        if(SubCheckCP(sp_max1) || SubCheckCP(sp_min1))
        {
            return true;
        }
        return false;
    }

    private bool SubCheckCP(float chk)
    {
        if (chk > minCP && chk < (minCP + cpGap))
            return true;
        else
            return false;
    }
}
