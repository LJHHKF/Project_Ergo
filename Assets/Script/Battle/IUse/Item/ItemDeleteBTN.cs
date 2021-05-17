using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeleteBTN : MonoBehaviour
{
    [SerializeField] private GameObject selectEf;
    private bool isSelected = false;

    private void Start()
    {
        isSelected = false;
        selectEf.SetActive(false);
    }

    public void BTNClicked()
    {
        if (isSelected)
        {
            isSelected = false;
            selectEf.SetActive(false);
        }
        else
        {
            isSelected = true;
            selectEf.SetActive(true);
        }
    }

    public bool GetSelected()
    {
        return isSelected;
    }
}
