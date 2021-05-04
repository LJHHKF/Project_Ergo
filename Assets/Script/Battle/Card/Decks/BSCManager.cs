using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Battle Scean Card Manager
public class BSCManager : MonoBehaviour
{
    [SerializeField] private Transform t_hand;
    [SerializeField] private int hand_max = 10;
    [SerializeField] private Transform t_grave;

    public int cntHand { get; private set; }

    private List<GameObject> list_hand = new List<GameObject>();
    private List<GameObject> list_grave = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        list_hand.Capacity = hand_max + 1;
        list_grave.Capacity = (CardPack.instance.GetCardsLength() * 3) + 1;
        DeckManager.instance.SetBSCManager(this);

        TurnManager.instance.battleEnd.AddListener(Event_BattleEnd);
        TurnManager.instance.playerTurnEnd.AddListener(Event_PlayerTurnEnd);

        GameMaster.instance.gameStop.AddListener(Event_GameStop);
    }

    private void OnDestroy()
    {
        TurnManager.instance.battleEnd.RemoveListener(Event_BattleEnd);
        TurnManager.instance.playerTurnEnd.RemoveListener(Event_PlayerTurnEnd);

        GameMaster.instance.gameStop.RemoveListener(Event_GameStop);
    }

    private void Event_BattleEnd()
    {
        CleanUpCards();
    }

    private void Event_PlayerTurnEnd()
    {
        ClearHandToGrave();
        //UndoHandsTaransparency();
        //SortingHand(0);
    }

    private void Event_GameStop()
    {
        CleanUpCards();
    }

    public void AddToHand(GameObject added, ref List<GameObject> prev_list, int prev_index)
    {
        AddToHand(added);
        prev_list.RemoveAt(prev_index);
    }

    public void AddToHand(GameObject added)
    {
        ICard temp = added.GetComponent<ICard>();
        if (temp != null)
        {
            if (list_hand.Count < hand_max)
            {
                added.transform.SetParent(t_hand);
                list_hand.Add(added);
                cntHand = list_hand.Count;
                temp.SetRenderPriority(cntHand);

                added.SetActive(true);

                if (TurnManager.instance.GetIsFirstActivated())
                {
                    SortingHand(0);
                }
            }
            else
            {
                MoveToGrave_Direct(added);
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

    public void MoveToGrave_Hand(GameObject moved)
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
            Debug.LogWarning("카드를 묘지로 이동하는 데 실패했습니다. 단, 전투 종료 직후라면 문제 없는 결과입니다.");
            return;
        }
        moved.transform.SetParent(t_grave);
        list_grave.Add(moved);
        SortingHand(moved.GetComponent<ICard>().GetRenderPriority());
        moved.GetComponent<ICard>().DoTransparency();
    }

    public void MoveToGrave_Direct(GameObject moved)
    {
        moved.transform.SetParent(t_grave);
        list_grave.Add(moved);
        moved.GetComponent<ICard>().DoTransparency();
    }

    public void MoveToGrave_Dircet(GameObject moved, ref List<GameObject> prev_list, int prev_index)
    {
        MoveToGrave_Direct(moved);
        prev_list.RemoveAt(prev_index);
    }

    public void PullingInGrave() //Pulling at card in graveyard
    {
        DeckManager.instance.MoveToDeck_All(ref list_grave);
    }

    private void CleanUpCards()
    {
        DeckManager.instance.MoveToDeck_All(ref list_hand);
        DeckManager.instance.MoveToDeck_All(ref list_grave);
    }

    private void ClearHandToGrave()
    {
        GameObject moved;
        for (int i = 0; i < list_hand.Count; i++)
        {
            moved = list_hand[i];

            moved.transform.SetParent(t_grave);
            list_grave.Add(moved);
            moved.GetComponent<ICard>().DoTransparency();
        }
        list_hand.Clear();
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

    public void GetGraveList(ref List<Card_Base> _cardList)
    {
        for(int i = 0; i < list_grave.Count; i++)
        {
            int _i = i;
            _cardList.Add(list_grave[_i].GetComponent<Card_Base>());
        }
    }

    //IEnumerator DelayedUnActive(GameObject target, float sec)
    //{
    //    yield return new WaitForSeconds(sec);
    //    target.SetActive(false);
    //    yield break;
    //}
}
