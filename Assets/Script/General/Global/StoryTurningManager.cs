using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTurningManager : MonoBehaviour
{
    private static StoryTurningManager m_instance;
    public static StoryTurningManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<StoryTurningManager>();
            return m_instance;
        }
    }

    public bool isTutorial { get; set; }
    public int battleDamage { get; set; }
    public bool isEliteStage { get; private set; }
    public int index_Elite { get; private set; }
    public bool isBossStage { get; private set; }
    public int index_Boss { get; private set; }
    public bool isShopStage { get; private set; }
    public bool isShop_Bought { get; set; }
    public bool isRestStage { get; private set; }
    public bool isRest_Rest { get; set; }
    public bool isRest_CDelete { get; set; }
    public bool isChapterEnd_1 { get; set; }


    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }
        isTutorial = false;
        battleDamage = 0;
        isEliteStage = false;
        index_Elite = -1;
        isBossStage = false;
        index_Boss = -1;
        isChapterEnd_1 = false;
    }

    public void SetShopStage(bool _value)
    {
        isShopStage = _value;
        isShop_Bought = false;
    }
    public void SetRestStage(bool _value)
    {
        isRestStage = _value;
        isRest_Rest = false;
        isRest_CDelete = false;
    }

    public void SetEliteStage(bool _value)
    {
        isEliteStage = _value;
        index_Elite = -1;
    }

    public void SetElite_Aries()
    {
        index_Elite = 0;
    }

    public void SetElite_Creta()
    {
        index_Elite = 1;
    }

    public void SetElite_JunkGoblin()
    {
        index_Elite = 2;
    }

    public void SetBossStage(bool _value)
    {
        isBossStage = _value;
        index_Boss = -1;
    }

    public void SetBoss_Aries()
    {
        index_Boss = 0;
    }
    public void SetBoss_Creta()
    {
        index_Boss = 1;
    }

    public void SetBoss_JunkGoblin()
    {
        index_Boss = 2;
    }
}
