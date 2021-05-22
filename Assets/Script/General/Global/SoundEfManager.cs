using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using mySoundEffect;

namespace mySoundEffect
{
    public enum SoundEf
    {
        storyEnd,
        monsterDead,
        heal,
        discard_card,
        ui_touch,
        card_sword,
        card_magic_attack,
        card_magic_other,
        card_OverKill,
        card_ManaStorm,
        playerHit,
        hit_sheep,
        hit_minota,
        hit_madGoblin,
        hit_creta
    }
}

public class SoundEfManager : MonoBehaviour
{
    private static SoundEfManager m_instance;
    public static SoundEfManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<SoundEfManager>();
            return m_instance;
        }
    }

    [Serializable]
    public struct SoundEffect
    {
        public GameObject prefab;
        public float playTime;
        public float queudDelay;
    }

    [Header("Instance Sound Effect")]
    [SerializeField] private SoundEffect storyEnd;
    [SerializeField] private SoundEffect monsterDead;
    [SerializeField] private SoundEffect heal;
    [SerializeField] private SoundEffect discard_card;
    [SerializeField] private SoundEffect ui_touch;

    [Header("Queued Delay Sound Effect")]
    [SerializeField] private SoundEffect card_sword;
    [SerializeField] private SoundEffect card_magic_attack;
    [SerializeField] private SoundEffect card_magic_other;
    [SerializeField] private SoundEffect card_OverKill;
    [SerializeField] private SoundEffect card_ManaStorm;
    [SerializeField] private SoundEffect playerHit;
    [SerializeField] private SoundEffect hit_sheep;
    [SerializeField] private SoundEffect hit_minota;
    [SerializeField] private SoundEffect hit_madGoblin;
    [SerializeField] private SoundEffect hit_creta;

    private List<GameObject> list_storyEnd = new List<GameObject>();
    private List<GameObject> list_monsterDead = new List<GameObject>();
    private List<GameObject> list_heal = new List<GameObject>();
    private List<GameObject> list_discard_card = new List<GameObject>();
    private List<GameObject> list_ui_touch = new List<GameObject>();

    private List<GameObject> list_card_sword = new List<GameObject>();
    private List<GameObject> list_card_magic_attack = new List<GameObject>();
    private List<GameObject> list_card_magic_other = new List<GameObject>();
    private List<GameObject> list_card_OverKill = new List<GameObject>();
    private List<GameObject> list_card_ManaStorm = new List<GameObject>();
    private List<GameObject> list_playerHit = new List<GameObject>();
    private List<GameObject> list_hit_sheep = new List<GameObject>();
    private List<GameObject> list_hit_minota = new List<GameObject>();
    private List<GameObject> list_hit_madGoblin = new List<GameObject>();
    private List<GameObject> list_hit_creta = new List<GameObject>();

    private float delayTime = 0.0f;
    private Queue<Action> changeQueue = new Queue<Action>();

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
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
    public void SetSoundEffect(SoundEf name)
    {
        SoundEffect t_go = storyEnd; //nullable로도 오류가 나서 초기값으로 넣어둔 것.
        List<GameObject> t_list = null;
        switch(name)
        {
            case SoundEf.storyEnd:
                t_go = storyEnd;
                t_list = list_storyEnd;
                Exe_Instant();
                break;
            case SoundEf.monsterDead:
                t_go = monsterDead;
                t_list = list_monsterDead;
                Exe_Instant();
                break;
            case SoundEf.heal:
                t_go = heal;
                t_list = list_heal;
                Exe_Instant();
                break;
            case SoundEf.discard_card:
                t_go = discard_card;
                t_list = list_discard_card;
                Exe_Instant();
                break;
            case SoundEf.ui_touch:
                t_go = ui_touch;
                t_list = list_ui_touch;
                Exe_Instant();
                break;
            case SoundEf.card_sword:
                t_go = card_sword;
                t_list = list_card_sword;
                changeQueue.Enqueue(Exe);
                break;
            case SoundEf.card_magic_attack:
                t_go = card_magic_attack;
                t_list = list_card_magic_attack;
                changeQueue.Enqueue(Exe);
                break;
            case SoundEf.card_magic_other:
                t_go = card_magic_other;
                t_list = list_card_magic_other;
                changeQueue.Enqueue(Exe);
                break;
            case SoundEf.card_OverKill:
                t_go = card_OverKill;
                t_list = list_card_OverKill;
                changeQueue.Enqueue(Exe);
                break;
            case SoundEf.card_ManaStorm:
                t_go = card_ManaStorm;
                t_list = list_card_ManaStorm;
                changeQueue.Enqueue(Exe);
                break;
            case SoundEf.playerHit:
                t_go = playerHit;
                t_list = list_playerHit;
                changeQueue.Enqueue(Exe);
                break;
            case SoundEf.hit_sheep:
                t_go = hit_sheep;
                t_list = list_hit_sheep;
                changeQueue.Enqueue(Exe);
                break;
            case SoundEf.hit_minota:
                t_go = hit_minota;
                t_list = list_hit_minota;
                changeQueue.Enqueue(Exe);
                break;
            case SoundEf.hit_madGoblin:
                t_go = hit_madGoblin;
                t_list = list_hit_madGoblin;
                changeQueue.Enqueue(Exe);
                break;
            case SoundEf.hit_creta:
                t_go = hit_creta;
                t_list = list_hit_creta;
                changeQueue.Enqueue(Exe);
                break;
        }

        void Exe()
        {
            if (t_list.Count == 0)
                Create();
            else
            {
                bool isSucess = false;
                for (int i = 0; i < t_list.Count; i++)
                {
                    int _i = i;
                    if (!t_list[_i].activeSelf)
                    {
                        t_list[_i].SetActive(true);
                        StartCoroutine(DelayedUnActive(t_list[_i], t_go.playTime));
                        delayTime += t_go.queudDelay;
                        isSucess = true;
                        break;
                    }
                }
                if (!isSucess)
                    Create();
            }
        }

        void Exe_Instant()
        {
            if (t_list.Count == 0)
                Create();
            else
            {
                bool isSucess = false;
                for (int i = 0; i < t_list.Count; i++)
                {
                    int _i = i;
                    if (!t_list[_i].activeSelf)
                    {
                        t_list[_i].SetActive(true);
                        StartCoroutine(DelayedUnActive(t_list[_i], t_go.playTime));
                        isSucess = true;
                        break;
                    }
                }
                if (!isSucess)
                    Create();
            }
        }

        void Create()
        {
            GameObject _go = Instantiate(t_go.prefab, gameObject.transform);
            t_list.Add(_go);
            StartCoroutine(DelayedUnActive(_go, t_go.playTime));
        }
    }


    IEnumerator DelayedUnActive(GameObject _go, float _sec)
    {
        yield return new WaitForSeconds(_sec);
        _go.SetActive(false);
        yield break;
    }
    
}
