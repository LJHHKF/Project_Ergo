using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatusWindowM : MonoBehaviour
{
    private bool isBattle;
    private Character m_char;
    private CStatManager m_statM;

    [SerializeField] private Text endu_field;
    [SerializeField] private Text str_field;
    [SerializeField] private Text solid_field;
    [SerializeField] private Text int_field;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Battle")
        {
            isBattle = true;
            m_char = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
            m_statM = GameObject.FindGameObjectWithTag("InfoM").GetComponent<CStatManager>();
        }
        else
        {
            isBattle = false;
            m_statM = GameObject.FindGameObjectWithTag("InfoM").GetComponent<CStatManager>();
        }
    }

    private void OnEnable()
    {
        StartCoroutine(delayedEnableSetting());
    }

    IEnumerator delayedEnableSetting()
    {
        yield return new WaitForSeconds(0.2f);
        if (isBattle)
        {
            endu_field.text = $"{m_statM.endurance} + ({m_char.fluc_endurance})";
            str_field.text = $"{m_statM.strength} + ({m_char.fluc_strength})";
            solid_field.text = $"{m_statM.solid} + ({m_char.fluc_solid})";
            int_field.text = $"{m_statM.intelligent} + ({m_char.fluc_intel})";
        }
        else
        {
            endu_field.text = $"{m_statM.endurance}";
            str_field.text = $"{m_statM.strength}";
            solid_field.text = $"{m_statM.solid}";
            int_field.text = $"{m_statM.intelligent}";
        }
        yield break;
    }
}
