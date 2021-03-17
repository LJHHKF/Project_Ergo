using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbCondInfoManager : MonoBehaviour
{
    public static AbCondInfoManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<AbCondInfoManager>();
            }
            return m_instance;
        }
    }
    private static AbCondInfoManager m_instance;

    public Sprite[] conditions_img;
    [TextArea]
    public string[] conditions_text; // 효과치가 계산되어 들어갈 자리엔 '(효과치)'라고 입력할 것.
    public int[] conditions_OnePower; // 1중첩당 효과 증감 수치

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public Sprite GetAbCond_Img(int id)
    {
        return conditions_img[id];
    }
}
