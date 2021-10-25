using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUF_Repeat : MonoBehaviour
{
    [SerializeField] private Card_Base myCard;
    [SerializeField] private int maxNum = 2;
    [SerializeField] private CUF_Base repeatTarget;
    [SerializeField] private float timeInterval = 1.0f;
    [SerializeField] private bool isUseFixP = false;
    [SerializeField] private bool isUseFlucP = false;
    [SerializeField] private bool isSecondDmgFormula = false;
    private int fixP = 1;
    private float flucPRate = 1.0f;
    private int dv = 0;

    // Start is called before the first frame update
    private void Start()
    {
        if (isUseFixP || isUseFlucP)
        {
            myCard.GetCardUseInfo(out fixP, out flucPRate);
            myCard.use += GetDVAndUse;
        }
        else
        {
            myCard.sub_use += SubUse;
        }
    }

    private void GetDVAndUse(int diceValue)
    {
        dv = diceValue;

        int dmg;
        if (isUseFixP && isUseFlucP && isSecondDmgFormula)
            dmg = Mathf.RoundToInt((fixP + diceValue) * flucPRate);
        else if(isUseFixP && isUseFlucP)
            dmg = fixP + Mathf.RoundToInt(diceValue * flucPRate);
        else if (isUseFixP)
            dmg = fixP;
        else if (isUseFlucP)
            dmg = Mathf.RoundToInt(diceValue * flucPRate);
        else
            dmg = 0;

        StartCoroutine(DelayedUse(dmg));
    }

    private void SubUse()
    {
        StartCoroutine(DelayedUse(maxNum));
    }

    IEnumerator DelayedUse(int _maxNum)
    {
        int cnt = 0;
        while(cnt < _maxNum)
        {
            yield return new WaitForSeconds(timeInterval);
            repeatTarget.ReUse();
            cnt++;
        }
        yield break;
    }
}
