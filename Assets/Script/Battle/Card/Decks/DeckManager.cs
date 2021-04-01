using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<DeckManager>();
            return m_instance;
        }
    }
    private static DeckManager m_instance;

    [SerializeField]private int draw_num = 5;

    private BSCManager m_BSCManager;
    private List<GameObject> list_deck = new List<GameObject>();

    private void Awake()
    {
        list_deck.Capacity = (CardPack.instance.GetCardsLength() * 3) + 1;
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameMaster.instance.initSaveData_Start += Event_InitSaveDataStart;
        GameMaster.instance.startGame_Start += Event_StartGame;
        TurnManager.instance.firstTurn += Event_FirstTurn;
        TurnManager.instance.turnStart += Event_TurnStart;
        //GameMaster.instance.battleStageStart += Event_BattleStageStart;
        //GameMaster.instance.battleStageEnd += Event_BattleStageEnd;
    }

    private void OnDestroy()
    {
        m_instance = null;
        GameMaster.instance.initSaveData_Start -= Event_InitSaveDataStart;
        GameMaster.instance.startGame_Start -= Event_StartGame;
        TurnManager.instance.firstTurn += Event_FirstTurn;
        TurnManager.instance.turnStart += Event_TurnStart;
        //GameMaster.instance.battleStageStart -= Event_BattleStageStart;
        //GameMaster.instance.battleStageEnd -= Event_BattleStageEnd;
    }

    //private void BattleStageInitSetting()
    //{
    //    TurnManager.instance.firstTurn += Event_FirstTurn;
    //    TurnManager.instance.turnStart += Event_TurnStart;
    //}

    private void Event_FirstTurn(object _o, EventArgs _e)
    {
        PullingInDeck_DrawSet();
    }

    private void Event_TurnStart(object _o, EventArgs _e)
    {
        PullingInDeck_DrawSet();
    }

    private void Event_InitSaveDataStart(object _o, EventArgs e)
    {
        ResetDeck();
    }

    private void Event_StartGame(object _o, EventArgs _e)
    {
        ResetDeck();
    }

    //private void Event_BattleStageStart(object _o, EventArgs _e)
    //{
    //    BattleStageInitSetting();
    //}

    //private void Event_BattleStageEnd(object _o, EventArgs _e)
    //{
    //    GameMaster.instance.battleStageStart -= Event_BattleStageStart;
    //}

    public void ResetDeck()
    {
        //for (int i = 0; i < gameObject.transform.childCount; i++)
        //{
        //    if (gameObject.transform.GetChild(i).GetComponent<ICard>() != null)
        //    {
        //        list_deck.Add(gameObject.transform.GetChild(i).gameObject);
        //    }
        //}

        CardPack.instance.InstantiateCards(gameObject.transform, ref list_deck);
    }

    public void SetBSCManager(BSCManager input)
    {
         m_BSCManager = input;
    }

    public void PullingInDeck() // Pulling at card in deck to hand
    {
        if(list_deck.Count <= 0)
        {
            ChkAndFindBSCManager();
            m_BSCManager.PullingInGrave();
        }

        int rand = UnityEngine.Random.Range(0, list_deck.Count);
        m_BSCManager.AddToHand(list_deck[rand]);
        list_deck.RemoveAt(rand);
    }

    public void PullingInDeck_DrawSet()
    {
        if (CardPack.instance.GetHadCardsNum() < m_instance.draw_num)
        {
            Debug.LogError("보유한 카드 수가 설정한 수(" + draw_num + ")보다 적기 때문에 현재 덱의 수(" + CardPack.instance.GetHadCardsNum() + ")만큼으로만 시작 드로우 수를 조정했습니다.");
            draw_num = CardPack.instance.GetHadCardsNum();
        }

        for (int i = 0; i < draw_num; i++)
            PullingInDeck();
        ChkAndFindBSCManager();
        m_BSCManager.SortingHand(0);
    }

    public void MoveToDeck_Single(GameObject moved)
    {
        ICard temp = moved.GetComponent<ICard>();
        if(temp != null)
        {
            moved.transform.SetParent(gameObject.transform);
            moved.SetActive(false);
            list_deck.Add(moved);
        }
    }

    public void MoveToDeck_All(ref List<GameObject> move_list)
    {
        if(move_list.Count != 0)
        {
            for(int i = 0; i < move_list.Count; i++)
            {
                list_deck.Add(move_list[i]);
                move_list[i].transform.SetParent(gameObject.transform);
                move_list[i].SetActive(false);
            }
            move_list.Clear();
        }
    }

    public void AddToDeck_Single(GameObject added, ref List<GameObject> prev_list, int prev_index)
    {
        AddToDeck_NonList(added);
        prev_list.RemoveAt(prev_index);
    }

    public void AddToDeck_NonList(GameObject added)
    {
        ICard temp = added.GetComponent<ICard>();
        if (temp != null)
        {
            added.transform.SetParent(gameObject.transform);
            added.SetActive(false);
            list_deck.Add(added);

        }
    }

    private void ChkAndFindBSCManager()
    {
        if (m_BSCManager == null)
        {
            m_BSCManager = GameObject.FindGameObjectWithTag("CManager").GetComponent<BSCManager>();
        }
    }
}
