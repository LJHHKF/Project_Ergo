using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class StatusInfoUI : MonoBehaviour
{
    [Header("Object Registration")]
    [SerializeField] private UnitUI m_unitUI;
    [SerializeField] private Enemy_Base m_enemy;
    [Header("Default Setting")]
    [SerializeField] private int abcond_index;
    [SerializeField] private bool isMonsterAct;

    private GameObject target;
    private Text txt_header;
    private Text txt_body;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("UIManager").GetComponent<BattleUIManager>().GetStatusDetailArea();
        txt_header = target.transform.Find("InfoTextArea").Find("Header").GetComponent<Text>();
        txt_body = target.transform.Find("InfoTextArea").Find("Body").GetComponent<Text>();
    }

    public void OnPointerDown()
    {
        target.SetActive(true);
        target.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        if (isMonsterAct)
            m_enemy.GetReadyActText(ref txt_header, ref txt_body);
        else
        {
            int id, piled;
            StringBuilder m_sb = new StringBuilder();
            m_unitUI.GetAbcondIDAndPiled(abcond_index, out id, out piled);

            if (piled == 0)
                piled += 1;

            m_sb.Append($"{ AbCondInfoManager.instance.GetAbCond_Name(id)} X {piled}");
            txt_header.text = m_sb.ToString();

            m_sb.Clear();
            m_sb.Append(AbCondInfoManager.instance.GetAbCond_text(id));
            m_sb.Replace("n", (piled).ToString());
            txt_body.text = m_sb.ToString();
        }
    }

    public void OnPointerUp()
    {
        target.SetActive(false);
    }
}
