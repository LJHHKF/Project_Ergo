using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using myBGM;

namespace myBGM
{
    public enum BGMList
    {
        entrance,
        battle,
        shop,
        rest,
        story
    }
}

public class BGMManager : MonoBehaviour
{
    private static BGMManager m_instance;
    public static BGMManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<BGMManager>();
            return m_instance;
        }
    }

    [Serializable]
    public struct EffectBGM
    {
        public AudioSource bgm;
        public float playTime;
    }

    [Header("Loop BGMs")]
    [SerializeField] private AudioSource bgm_entrance;
    [SerializeField] private AudioSource bgm_battle;
    [SerializeField] private AudioSource bgm_shop;
    [SerializeField] private AudioSource bgm_rest;
    [SerializeField] private AudioSource bgm_story;

    [Header("NonLoop BGMs")]
    [SerializeField] private EffectBGM bgm_ef_battleWin;
    [SerializeField] private EffectBGM bgm_ef_dead;
    [SerializeField] private EffectBGM bgm_ef_trap_Arrow;
    [SerializeField] private EffectBGM bgm_ef_trap_Stone;

    private AudioSource curBGM;
    private BGMList? curBGM_name;
    private float delayTime = 0.0f;

    private Queue<Action> changeQueue = new Queue<Action>();

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        bgm_entrance.Stop();
        bgm_battle.Stop();
        bgm_shop.Stop();
        bgm_rest.Stop();
        bgm_story.Stop();
        bgm_ef_battleWin.bgm.Stop();
        bgm_ef_dead.bgm.Stop();

        curBGM = bgm_entrance;
        curBGM_name = BGMList.entrance;
        curBGM.Play();
    }

    private void Update()
    {
        if (delayTime > 0)
        {
            delayTime -= Time.deltaTime;
        }
        else if (changeQueue.Count > 0)
        {
            changeQueue.Dequeue().Invoke();
        }
    }

    public void BGMChange(BGMList _name)
    {
        changeQueue.Enqueue(BGMChange_private);
        void BGMChange_private()
        {
            if (curBGM_name != _name)
            {
                curBGM?.Stop();

                if (_name == BGMList.entrance)
                    curBGM = bgm_entrance;
                else if (_name == BGMList.battle)
                    curBGM = bgm_battle;
                else if (_name == BGMList.shop)
                    curBGM = bgm_shop;
                else if (_name == BGMList.rest)
                    curBGM = bgm_rest;
                else if (_name == BGMList.story)
                    curBGM = bgm_story;

                curBGM.Play();
                curBGM_name = _name;
            }
        }
    }

    private void EffectBGMStop(EffectBGM _t)
    {
        _t.bgm.Stop();
        if (changeQueue.Count == 0)
        {
            string name = SceneManager.GetActiveScene().name;
            if (name == "Enterance")
                BGMChange(BGMList.entrance);
            else if (name == "StoryScene")
                BGMChange(BGMList.story);
            else if (name == "Battle")
                BGMChange(BGMList.battle);
            else if (name == "Ev_Shop")
                BGMChange(BGMList.shop);
            else if (name == "Ev_Rest")
                BGMChange(BGMList.rest);
            else if (name == "Ev_Trap")
                BGMChange(BGMList.battle);
        }
    }

    public void EffectBGM_BatlleWin()
    {
        changeQueue.Enqueue(BGMChange);
        changeQueue.Enqueue(BGMStop);
        void BGMChange()
        {
            curBGM?.Stop();
            curBGM_name = null;
            bgm_ef_battleWin.bgm.Play();
            delayTime += bgm_ef_battleWin.playTime;
        }
        void BGMStop()
        {
            EffectBGMStop(bgm_ef_battleWin);
        }
    }

    public void EffectBGM_dead()
    {
        changeQueue.Enqueue(BGMChange);
        changeQueue.Enqueue(BGMStop);
        void BGMChange()
        {
            curBGM?.Stop();
            curBGM_name = null;
            bgm_ef_dead.bgm.Play();
            delayTime += bgm_ef_dead.playTime;
        }
        void BGMStop()
        {
            EffectBGMStop(bgm_ef_dead);
        }
    }

    /// 0: Arrow, 1: Stone
    public void EFfectBGM_trap(int _index)
    {
        changeQueue.Enqueue(BGMChange);
        changeQueue.Enqueue(BGMStop);
        void BGMChange()
        {
            curBGM?.Stop();
            curBGM_name = null;
            switch(_index)
            {
                case 0:
                    bgm_ef_trap_Arrow.bgm.Play();
                    delayTime += bgm_ef_trap_Arrow.playTime;
                    break;
                case 1:
                    bgm_ef_trap_Stone.bgm.Play();
                    delayTime += bgm_ef_trap_Stone.playTime;
                    break;
            }
        }
        void BGMStop()
        {
            switch(_index)
            {
                case 0:
                    EffectBGMStop(bgm_ef_trap_Arrow);
                    break;
                case 1:
                    EffectBGMStop(bgm_ef_trap_Stone);
                    break;
            }
        }
    }
}
