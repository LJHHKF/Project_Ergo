using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TopHpText : MonoBehaviour
{
    private TextMeshProUGUI m_text;
    private int max;
    private int cur;

    private Character m_char;
    private CStatManager m_statM;
    // Start is called before the first frame update
    void Start()
    {
        m_text = gameObject.GetComponent<TextMeshProUGUI>();
        if(SceneManager.GetActiveScene().name == "Battle")
        {
            m_char = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
            max = m_char.GetFullHealth();
            cur = m_char.health;
            m_char.onHPDamage += UpdateText;
        }
        else
        {
            m_statM = GameObject.FindGameObjectWithTag("InfoM").GetComponent<CStatManager>();
            max = m_statM.GetCalcFullHealth();
            cur = m_statM.health;
        }
        m_text.text = $"{cur} / {max}";

    }

    private void UpdateText()
    {
        max = m_char.GetFullHealth();
        cur = m_char.health;
        m_text.text = $"{cur} / {max}";
    }
}
