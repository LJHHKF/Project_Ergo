using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Battle Scean Card Manager
public class BSCManager : MonoBehaviour
{
    public Transform t_hand;
    public Transform t_grave;
    public int cntHand { get; private set; }
    public float rotP = 2f;
    public float moveP = 1f;

    private DeckManager m_Deck;
    private List<GameObject> list_hand = new List<GameObject>();
    private List<GameObject> list_grave = new List<GameObject>();

    private TurnManager m_turnM;

    // Start is called before the first frame update
    void Start()
    {
        m_Deck = GameObject.FindGameObjectWithTag("CDeck").GetComponent<DeckManager>();
        m_Deck.SetBSCManager(this);

        m_turnM = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
        m_turnM.battleEnd += () => CleanUpCards();
        m_turnM.playerTurnEnd += () => UndoHandsTaransparency();
        m_turnM.playerTurnEnd += () => SortingHand(0);
    }

    private void OnDestroy()
    {
        CleanUpCards();
    }

    public void AddToHand(GameObject added)
    {
        ICard temp = added.GetComponent<ICard>();
        if (temp != null)
        {
            added.transform.SetParent(t_hand);
            list_hand.Add(added);
            cntHand = list_hand.Count;
            temp.SetRenderPriority(cntHand);

            added.SetActive(true);

            if(m_turnM.GetIsFirstActivated())
            {
                SortingHand(0);
            }
        }
    }
    
    public void SortingHand(int renderPriority)
    {
        for(int i = 0; i < list_hand.Count; i++)
        {
            list_hand[i].GetComponent<ICard>().SortingCard(renderPriority, list_hand.Count);
        }
    }

    public void MoveToGrave(GameObject moved)
    {
        bool isSuccess = false;
        for(int i = 0; i < list_hand.Count; i++)
        {
            if (System.Object.ReferenceEquals(list_hand[i], moved))
            {
                list_hand.RemoveAt(i);
                isSuccess = true;
            }
        }
        if(!isSuccess)
        {
            Debug.LogError("카드를 묘지로 이동하는 데 실패했습니다.");
        }
        moved.transform.SetParent(t_grave);
        list_grave.Add(moved);
        SortingHand(moved.GetComponent<ICard>().GetRenderPriority());
        DelayedUnActive(moved, 1.0f);
    }

    public void PullingInGrave() //Pulling at card in graveyard
    {
        for(int i = 0; i < list_grave.Count; i++)
        {
            m_Deck.MoveToDeck(list_grave[i]);
        }
    }

    public void SetCardSortingValue(out float rotationPower, out float movePower)
    {
        rotationPower = rotP;
        movePower = moveP;
    }

    private void CleanUpCards()
    {
        for (int i = 0; i < list_hand.Count; i++)
        {
            m_Deck.MoveToDeck(list_hand[i]);
        }
        for (int i = 0; i < list_grave.Count; i++)
        {
            m_Deck.MoveToDeck(list_grave[i]);
        }
    }

    public void DoHandsTransparency()
    {
        for(int i = 0; i < list_hand.Count; i++)
        {
            list_hand[i].GetComponent<ICard>().DoTransparency();
        }
    }

    public void UndoHandsTaransparency()
    {
        for(int i = 0; i < list_hand.Count; i++)
        {
            list_hand[i].GetComponent<ICard>().UndoTransparency();
        }
    }

    IEnumerator DelayedUnActive(GameObject target, float sec)
    {
        yield return new WaitForSeconds(sec);
        target.SetActive(false);
        yield break;
    }
}
