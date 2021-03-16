using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostManager : MonoBehaviour
{
    [Header("Cost Setting")]
    public int maxCost = 3; // 캐릭터 스테이터스 관리자를 만들면, 맥스 값은 거기서 관리하고, 시작하면 얻어올 것.
    private bool isChanged = false;
    private int m_cost = 0;
    public int cost
    {
        get
        {
            return m_cost;
        }
        set
        {
            m_cost = value;
            isChanged = true;
        }
    }// 만약 cost를 얻는 카드가 있고, 그게 max치보다 더 높게 쌓을 수 있다면 건들 필요 없지만, max까지만 찬다면 건드려야 함. 일단 안 건듦.
    
    [Header("Object registration")]
    public TurnManager turnManager;
    public Text txt_max;
    public Text txt_cur;

    // Start is called before the first frame update
    void Start()
    {
        turnManager.firstTurn += () => ResetCost();
        turnManager.turnStart += () => ResetCost();

        txt_cur.text = cost.ToString();
        txt_max.text = "/" + maxCost.ToString();
    }

    private void Update()
    {
        if(isChanged)
        {
            txt_cur.text = cost.ToString();
            isChanged = false;
        }
    }

    private void ResetCost()
    {
        cost = maxCost;
    }
}
