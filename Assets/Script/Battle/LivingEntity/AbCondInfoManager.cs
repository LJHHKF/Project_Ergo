using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    [Serializable]
    struct AbCondition
    {
        public Sprite img;
        public string name;
        [TextArea]
        public string infoText; // 효과치가 계산되어 들어갈 자리엔 '(효과치)'라고 입력할 것.
        public int onePower;
    }

    [SerializeField] private AbCondition[] abConditions;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        m_instance = null;
    }

    public Sprite GetAbCond_Img(int id)
    {
        return m_instance.abConditions[id].img;
    }

    public int GetAbCond_OnePower(int id)
    {
        return m_instance.abConditions[id].onePower;
    }

    public void GetAbCond_text(int id, out string name, out string infoText)
    {
        name = m_instance.abConditions[id].name;
        infoText = m_instance.abConditions[id].infoText;
    }

    public int GetAbCondListLength()
    {
        return abConditions.Length;
    }
}
