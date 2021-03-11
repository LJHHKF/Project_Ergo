using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [Header("Object registration")]
    public GameObject forDice;

    // Start is called before the first frame update
    void Start()
    {
        forDice.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDiceSysetm()
    {
        forDice.SetActive(true);
    }

    public void OffDiceSystem()
    {
        forDice.SetActive(false);
    }
}
