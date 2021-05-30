using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CUF_Base : MonoBehaviour
{

    [SerializeField] protected Card_Base myCard;
    [SerializeField] protected bool isOnlyFixed = false;
    [SerializeField] protected bool isOnlyDiceValue = false;
    [SerializeField] protected bool isSecondDmgFormula = false;
    [SerializeField] protected bool isUseFixRate = false;
    [SerializeField] protected float fixPRate = 0.5f;
    [SerializeField] protected float affectDelay = 2.0f;
    protected bool isUseRepeat = false;
    //[SerializeField] protected int repeatNum = 1;
    //[SerializeField] protected float repeatDelay = 0.5f;
    protected int fixP = 1;
    protected float flucPRate = 1.0f;
    protected GameObject target;
    protected int dv = 0;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        myCard.GetCardUseInfo(out fixP, out flucPRate);
        if (isUseFixRate)
            fixP = Mathf.RoundToInt(fixP * fixPRate);

        if (GetComponent<CUF_Repeat>() != null)
        {
            isUseRepeat = true;
            myCard.use += SetDV;
        }
        else
            isUseRepeat = false;

        myCard.ev_setDelay += SetAffectDelay;
    }

    public virtual void Use(int diceValue)
    {

    }

    public virtual void ReUse()
    {

    }

    protected void SetAffectDelay(float _delay)
    {
        affectDelay = _delay;
    }

    protected void SetDV(int _dicevalue)
    {
        dv = _dicevalue;
    }

    protected IEnumerator delayedAffect(Action _action)
    {
        yield return new WaitForSeconds(affectDelay);
        _action.Invoke();
        myCard.OnEffect();
        yield break;
    }

    protected IEnumerator RepeatAffect(Action _action, int _repeat, float delayTime)
    {
        Debug.Log("불림1");
        for (int i = 0; i < _repeat; i++)
        {
            Debug.Log("불림2");
            StartCoroutine(delayedAffect(_action));
            yield return new WaitForSeconds(delayTime);
        }
        yield break;
    }
}
