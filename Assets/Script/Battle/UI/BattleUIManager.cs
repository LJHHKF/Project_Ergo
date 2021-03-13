using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [Header("Object registration")]
    public GameObject forDice;
    private RectTransform diceRect;

    private Camera myMainCam;

    // Start is called before the first frame update
    void Start()
    {
        diceRect = forDice.GetComponent<RectTransform>();
        forDice.SetActive(false);

        myMainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDiceSysetm(Vector2 cardPos)
    {
        forDice.SetActive(true);
        cardPos = myMainCam.WorldToScreenPoint(cardPos);
        diceRect.position = cardPos;
    }

    public void OffDiceSystem()
    {
        forDice.SetActive(false);
    }
}
