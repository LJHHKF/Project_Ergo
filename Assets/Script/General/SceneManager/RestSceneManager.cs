using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BtnRest()
    {
        int fullHealth = CStatManager.instance.fullHealth_pure + CStatManager.instance.endurance;
        int restoreValue = Mathf.RoundToInt(fullHealth * 0.3f);
            
        if (CStatManager.instance.health + restoreValue >= fullHealth)
        {
            restoreValue = (fullHealth) - CStatManager.instance.health;
            CStatManager.instance.HealthPointUpdate(CStatManager.instance.health + restoreValue);
        }
        else
        {
            CStatManager.instance.HealthPointUpdate(CStatManager.instance.health + restoreValue);
        }

        LoadManager.instance.LoadNextStage();
    }

    public void BtnDiscard()
    {

    }
}
