using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public int startCardAmount = 5;

    private BSCManager m_BSCManager;
    private List<GameObject> list_deck = new List<GameObject>();
    private TurnManager m_turnM;
    private CardPack c_pack;

    private void Awake()
    {
        c_pack = GameObject.FindGameObjectWithTag("InfoM").GetComponentInChildren<CardPack>();
        ResetDeck();
    }

    private void Start()
    {
        //SetTurnManager();
    }

    private void OnDestroy()
    {
        //저장될 때.
    }

    public void ResetDeck()
    {
        //for (int i = 0; i < gameObject.transform.childCount; i++)
        //{
        //    if (gameObject.transform.GetChild(i).GetComponent<ICard>() != null)
        //    {
        //        list_deck.Add(gameObject.transform.GetChild(i).gameObject);
        //    }
        //}

        c_pack.InstantiateCards(gameObject.transform, ref list_deck);
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

        int rand = Random.Range(0, list_deck.Count);
        m_BSCManager.AddToHand(list_deck[rand]);
        list_deck.RemoveAt(rand);
    }

    public void PullingInDeck_FirstTurn()
    {
        if (list_deck.Count < startCardAmount)
        {
            Debug.LogError("덱의 카드 수가 설정한 수(" + startCardAmount + ")보다 적기 때문에 현재 덱의 수(" + list_deck.Count + ")만큼으로만 시작 드로우 수를 조정했습니다.");
            startCardAmount = list_deck.Count;
        }

        for (int i = 0; i < startCardAmount; i++)
            PullingInDeck();
        ChkAndFindBSCManager();
        m_BSCManager.SortingHand(0);
    }

    public void MoveToDeck(GameObject moved)
    {
        ICard temp = moved.GetComponent<ICard>();
        if(temp != null)
        {
            moved.transform.SetParent(gameObject.transform);
            moved.SetActive(false);
            list_deck.Add(moved);
        }
    }

    public void AddToDeck(GameObject added)
    {
        ICard temp = added.GetComponent<ICard>();
        if (temp != null)
        {
            //여기에 같은 ID의 카드가 3장 이상이면~ 같은 예외 조항 추가해야 함.

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

    //public void SetTurnManager()
    //{
    //    m_turnM = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
    //    m_turnM.firstTurn += () => PullingInDeck_FirstTurn();
    //    m_turnM.turnStart += () => PullingInDeck();
    //}

    public void SetTurnManager(TurnManager tm)
    {
        m_turnM = tm;
        m_turnM.firstTurn += () => PullingInDeck_FirstTurn();
        m_turnM.turnStart += () => PullingInDeck();
    }
}
