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
