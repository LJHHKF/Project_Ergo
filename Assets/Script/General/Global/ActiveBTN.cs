using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBTN : MonoBehaviour
{
    [SerializeField] private GameObject activeTarget;

    public void ActiveBTNClick()
    {
        if(activeTarget.activeSelf)
        {
            activeTarget.SetActive(false);
        }
        else
        {
            activeTarget.SetActive(true);
        }
    }

}
