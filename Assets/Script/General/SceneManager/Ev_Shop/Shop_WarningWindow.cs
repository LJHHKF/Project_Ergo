using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_WarningWindow : MonoBehaviour
{
    [Header("Object Registration")]
    [SerializeField] private Shop_DetailWindow detailWindow;
    [Space]
    [SerializeField] private Text txt_Body;

    [Header("Pos Object Registration")]
    [SerializeField] private Transform t_left;
    [SerializeField] private Transform t_right;
    [SerializeField] private Transform t_top;
    [SerializeField] private Transform t_down;

    [Header("Text Registartion")]
    [TextArea] [SerializeField] private string lessSoul;
    [TextArea] [SerializeField] private string lessStock;
    [TextArea] [SerializeField] private string lessInventory;

    private void OnEnable()
    {
        detailWindow.SetHadWarning(true);
    }

    private void OnDisable()
    {
        detailWindow.SetHadWarning(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            if (t_left.position.x > mousePos.x
                || t_right.position.x < mousePos.x
                || t_top.position.y < mousePos.y
                || t_down.position.y > mousePos.y)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void SetWarning_LessSoul()
    {
        txt_Body.text = lessSoul;
    }

    public void SetWarning_LessStock()
    {
        txt_Body.text = lessStock;
    }

    public void SetWarning_LessInventory()
    {
        txt_Body.text = lessInventory;
    }
}
