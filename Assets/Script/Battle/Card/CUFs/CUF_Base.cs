using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUF_Base : MonoBehaviour
{

    public Card_Base myCard;
    public bool isOnlyFixed = false;
    public bool isOnlyDiceValue = false;
    public bool isSecondDmgFormula = false;
    protected int fixP = 1;
    protected float flucPRate = 1.0f;
    protected GameObject target;
    protected int dv = 0;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        myCard.GetCardUseInfo(out fixP, out flucPRate);
    }

    public virtual void Use(int diceValue)
    {

    }

    public virtual void ReUse()
    {

    }
}
