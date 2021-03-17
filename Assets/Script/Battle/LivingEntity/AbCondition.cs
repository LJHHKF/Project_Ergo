using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AbCond
{
    public int ID;
    public Sprite Icon;
    public int persistTurn;
    public bool isChecked;  // 스탯 증감식의 녀석은, 처음 조우 시 리스트 전체서 같은 id 체크하며 cnt 늘려가는 방식으로 증감되는 스탯치 계산 후 true로. true면 affacted 재실행x.

    public AbCond(int _id, Sprite _icon, int _pTurn, bool _isChecked)
    {
        ID = _id;
        Icon = _icon;
        persistTurn = _pTurn;
        isChecked = _isChecked;
    }

    public void DecresePTurn()
    {
        persistTurn -= 1;
    }

    public void ChkFalse()
    {
        isChecked = false;
    }

    public void ChkTrue()
    {
        isChecked = true;
    }
}

public class AbCondition : MonoBehaviour
{
    private List<AbCond> list_conditions = new List<AbCond>();
    private AbCondInfoManager abCondInfoM;
    public LivingEntity m_target;
    
    // Start is called before the first frame update
    void Start()
    {
        abCondInfoM = GameObject.FindGameObjectWithTag("InfoM").GetComponent<AbCondInfoManager>();
    }

    public void AddAbCondition(int id, int persistTurn)
    {
        AbCond temp;
        temp.ID = id;
        temp.Icon = abCondInfoM.GetAbCond_Img(id);
        temp.persistTurn = persistTurn;
        temp.isChecked = false;

        list_conditions.Add(temp);
    }

    public void Affected() //자신에 걸린 모든 효과 발동
    {
        for (int i = 0; i < list_conditions.Count; i++)
        {
            if (!list_conditions[i].isChecked)
            {
                Affected(list_conditions[i].ID);
                list_conditions[i].DecresePTurn();
            }
            if(list_conditions[i].persistTurn <= 0)
            {
                list_conditions.RemoveAt(i);
            }
        }
    }

    private void Affected(int id)
    {

    }

    private void ResetListChecked()
    {
        for (int i = 0; i < list_conditions.Count; i++)
            list_conditions[i].ChkFalse();
    }
}
