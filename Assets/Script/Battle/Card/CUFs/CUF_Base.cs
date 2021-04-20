using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUF_Base : MonoBehaviour
{

    [SerializeField] protected Card_Base myCard;
    [SerializeField] protected bool isOnlyFixed = false;
    [SerializeField] protected bool isOnlyDiceValue = false;
    [SerializeField] protected bool isSecondDmgFormula = false;
    [SerializeField] protected bool isUseFixRate = false;
    [SerializeField] protected float fixPRate = 0.5f;
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
    }

    public virtual void Use(int diceValue)
    {

    }

    public virtual void ReUse()
    {

    }
}
