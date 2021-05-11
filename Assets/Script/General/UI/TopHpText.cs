using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class TopHpText : MonoBehaviour
{
    private TextMeshProUGUI m_text;
    private int max;
    private int cur;
    private bool isEventAdded = false;

    private Character m_char;
    private CStatManager m_statM;
    // Start is called before the first frame update
    void Start()
    {
        m_text = gameObject.GetComponent<TextMeshProUGUI>();
        if (SceneManager.GetActiveScene().name == "Battle")
        {
            m_char = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
            m_char.onHPDamage += UpdateText;
            TurnManager.instance.firstTurn += firstTurnEvent;
            isEventAdded = true;
        }
        else
        {
            m_statM = GameObject.FindGameObjectWithTag("InfoM").GetComponent<CStatManager>();
            max = m_statM.GetCalcFullHealth();
            cur = m_statM.health;
            m_text.text = $"{cur} / {max}";
        }
        
    }

    private void OnDisable()
    {
        if(isEventAdded)
            TurnManager.instance.firstTurn -= firstTurnEvent;
    }

    private void firstTurnEvent()
    {
        UpdateText(0);
    }

    private void UpdateText(int _dummy)
    {
        cur = m_char.health;
        max = m_char.GetFullHealth();
        m_text.text = $"{cur} / {max}";
    }
}
