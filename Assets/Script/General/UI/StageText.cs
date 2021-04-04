using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageText : MonoBehaviour
{
    [SerializeField] private Text m_text;
    private int cur;
    private int max;
    // Start is called before the first frame update
    void Start()
    {

        cur = StageManager.instance.curStage;
        max = StageManager.instance.GetCurStageMax();

        m_text.text = $"{cur}/{max}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
