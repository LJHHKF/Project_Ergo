using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AbCond
{
    public int ID;
    public Sprite Icon;
    public int piledNum;

    public AbCond(int _id, Sprite _icon, int _piledNum)
    {
        ID = _id;
        Icon = _icon;
        piledNum = _piledNum;
    }

    public void DecresePTurn()
    {
        piledNum -= 1;
    }


    public void DecreseP(int num)
    {
        piledNum -= num;
    }

    public void IncreaseP(int num)
    {
        piledNum += num;
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

    public void AddAbCondition(int id, int piledN)
    {
        bool isBeing = false;

        for(int i = 0; i < list_conditions.Count; i++)
        {
            if(list_conditions[i].ID == id)
            {
                isBeing = true;
                list_conditions[i].IncreaseP(piledN);
                break;
            }
        }

        if (!isBeing)
        {
            AbCond temp;
            temp.ID = id;
            temp.Icon = abCondInfoM.GetAbCond_Img(id);
            temp.piledNum = piledN;

            list_conditions.Add(temp);
        }
    }

    public void Affected() //자신에 걸린 모든 효과 발동
    {
        for (int i = 0; i < list_conditions.Count; i++)
        {

            Affected(list_conditions[i].ID);
            list_conditions[i].DecresePTurn();
            if(list_conditions[i].piledNum <= 0)
            {
                list_conditions.RemoveAt(i);
            }
        }
    }

    private void Affected(int id)
    {

    }
}
