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
        //GameMaster.instance.initSaveData_Start += Event_InitSaveDataStart;
        //GameMaster.instance.startGame_Start += Event_StartGame;
        //GameMaster.instance.gameStop += Event_GameStop;
        ResetDeck();
        TurnManager.instance.firstTurn += Event_FirstTurn;
        TurnManager.instance.turnStart += Event_TurnStart;
    }

    private void OnDestroy()
    {
        m_instance = null;
        //GameMaster.instance.initSaveData_Start -= Event_InitSaveDataStart;
        //GameMaster.instance.startGame_Start -= Event_StartGame;
        //GameMaster.instance.gameStop -= Event_GameStop;
        TurnManager.instance.firstTurn -= Event_FirstTurn;
        TurnManager.instance.turnStart -= Event_TurnStart;
        
    }

    private void Event_FirstTurn()
    {
        PullingInDeck_DrawSet();
    }

    private void Event_TurnStart()
    {
        PullingInDeck_DrawSet();
    }

    private void Event_InitSaveDataStart()
    {
        ResetDeck();
    }

    private void Event_StartGame()
    {
        ResetDeck();
    }

    private void Event_GameStop()
    {
        list_deck.Clear();
    }

    public void ResetDeck()
    {
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

    public void GetDeckList(ref List<Card_Base> _cardList)
    {
        for(int i = 0; i < list_deck.Count; i++)
        {
            int _i = i;
            _cardList.Add(list_deck[_i].GetComponent<Card_Base>());
        }
    }

    public bool DeleteCard_listindex(int _index)
    {
        if (list_deck.Count > 10)
        {
            SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.discard_card);
            CardPack.instance.DeleteCard_hadData(list_deck[_index].GetComponent<ICard>().GetID());
            list_deck.RemoveAt(_index);
            return true;
        }
        else
            return false;
    }

    public bool DeleteCard_CardID(int _id)
    {
        if(list_deck.Count > 10)
        {
            for (int i = 0; i < list_deck.Count; i++)
            {
                int _i = i;
                if (list_deck[_i].GetComponent<Card_Base>().GetID() == _id)
                {
                    SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.discard_card);
                    CardPack.instance.DeleteCard_hadData(list_deck[_i].GetComponent<ICard>().GetID());
                    list_deck.RemoveAt(_i);
                    return true;
                }
            }
            return false;
        }
        else
        {
            return false;
        }

    }
}
