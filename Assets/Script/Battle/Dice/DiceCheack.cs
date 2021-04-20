using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheack : MonoBehaviour
{
    [SerializeField] private DiceManager p_diceManager;
    [SerializeField] private int m_number;

    private void Start()
    {
        if (m_number <= 0 || m_number > 6)
            Debug.LogError("수가 잘못 입력된 주사위 면이 있음." + m_number);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DiceChecker")
        {
            p_diceManager.resNum = m_number;
        }
    }
}
