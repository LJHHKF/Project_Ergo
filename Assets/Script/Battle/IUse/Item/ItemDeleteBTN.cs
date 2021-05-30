using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeleteBTN : MonoBehaviour
{
    private bool isSelected = false;

    [SerializeField] private Transform t_left;
    [SerializeField] private Transform t_right;
    [SerializeField] private Transform t_top;
    [SerializeField] private Transform t_down;


    private void Start()
    {
        isSelected = false;

        InputSystem.instance.SetDeleteBTNPos(t_left.position.x, t_right.position.x, t_top.position.y, t_down.position.y);
    }

    public bool GetSelected()
    {
        return isSelected;
    }
}
