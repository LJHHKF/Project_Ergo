using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopSoulText : MonoBehaviour
{
    private TextMeshProUGUI m_text;
    // Start is called before the first frame update
    void Start()
    {
        m_text = gameObject.GetComponent<TextMeshProUGUI>();
        UpdateText();
        PlayerMoneyManager.instance.soulChanged += UpdateText;
    }

    private void OnDisable()
    {
        PlayerMoneyManager.instance.soulChanged -= UpdateText;
    }

    private void UpdateText()
    {
        m_text.text = PlayerMoneyManager.instance.soul.ToString();
    }
}
