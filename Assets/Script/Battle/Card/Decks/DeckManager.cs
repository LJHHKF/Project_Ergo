using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public DeckManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<DeckManager>();
            return m_instance;
        }
    }
    private static DeckManager m_instance;

    [SerializeField]private int startCardAmount = 5;

    private BSCManager m_BSCManager;
    private List<GameObject> list_deck = new List<GameObject>();
    private CardPack c_pack;
    private TurnManager m_TurnM;

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //SetTurnManager();
        c_pack = GameObject.FindGameObjectWithTag("InfoM").GetComponentInChildren<CardPack>();

        GameMaster.initSaveData_Start += () => ResetDeck();
        GameMaster.startGame_Start += () => ResetDeck();
        GameMaster.battleStageStart += () => BattleStageInitSetting();
    }

    private void BattleStageInitSetting()
    {
        m_TurnM = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
        m_TurnM.firstTurn += () => PullingInDeck_FirstTurn();
        m_TurnM.turnStart += () => PullingInDeck();
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

    public static void SetBSCManager(BSCManager input)
    {
         m_instance.m_BSCManager = input;
    }

    public static void PullingInDeck() // Pulling at card in deck to hand
    {
        if(m_instance.list_deck.Count <= 0)
        {
            m_instance.ChkAndFindBSCManager();
            m_instance.m_BSCManager.PullingInGrave();
        }

        int rand = Random.Range(0, m_instance.list_deck.Count);
        m_instance.m_BSCManager.AddToHand(m_instance.list_deck[rand]);
        m_instance.list_deck.RemoveAt(rand);
    }

    public static void PullingInDeck_FirstTurn()
    {
        if (m_instance.list_deck.Count < m_instance.startCardAmount)
        {
            Debug.LogError("덱의 카드 수가 설정한 수(" + m_instance.startCardAmount + ")보다 적기 때문에 현재 덱의 수(" + m_instance.list_deck.Count + ")만큼으로만 시작 드로우 수를 조정했습니다.");
            m_instance.startCardAmount = m_instance.list_deck.Count;
        }
        for (int i = 0; i < m_instance.startCardAmount; i++)
            PullingInDeck();
        m_instance.ChkAndFindBSCManager();
        m_instance.m_BSCManager.SortingHand(0);
    }

    public static void MoveToDeck(GameObject moved)
    {
        ICard temp = moved.GetComponent<ICard>();
        if(temp != null)
        {
            moved.transform.SetParent(m_instance.gameObject.transform);
            moved.SetActive(false);
            m_instance.list_deck.Add(moved);
        }
    }

    public static void AddToDeck(GameObject added)
    {
        ICard temp = added.GetComponent<ICard>();
        if (temp != null)
        {
            //여기에 같은 ID의 카드가 3장 이상이면~ 같은 예외 조항 추가해야 함.

            added.transform.SetParent(m_instance.gameObject.transform);
            added.SetActive(false);
            m_instance.list_deck.Add(added);
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
