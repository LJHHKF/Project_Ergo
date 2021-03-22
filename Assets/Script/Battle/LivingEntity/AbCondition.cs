using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AbCondition : MonoBehaviour
{
    public class AbCond
    {
        public int ID;
        public Sprite Icon;
        public int piledNum;
        public int onePower;

        public AbCond(int _id, Sprite _icon, int _piledNum, int _onePower)
        {
            ID = _id;
            Icon = _icon;
            piledNum = _piledNum;
            onePower = _onePower;
        }

        public void DecresePTurn()
        {
            this.piledNum -= 1;
        }

        public void DecreseP(int num)
        {
            this.piledNum -= num;
        }

        public void IncreaseP(int num)
        {
            this.piledNum += num;
        }
    }

    private List<AbCond> list_conditions = new List<AbCond>();
    private AbCondInfoManager abCondInfoM;
    public LivingEntity m_target;
    public UnitUI myUI;
    
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
            AbCond temp = new AbCond(id, abCondInfoM.GetAbCond_Img(id), piledN, abCondInfoM.GetAbCond_OnePower(id));
            //temp.ID = id;
            //temp.Icon = abCondInfoM.GetAbCond_Img(id);
            //temp.piledNum = piledN;
            //temp.onePower = abCondInfoM.GetAbCond_OnePower(id);

            list_conditions.Add(temp);
        }

        myUI.AbConditionsUpdate();
    }

    public void Affected() //자신에 걸린 모든 효과 발동
    {
        for(int i = 0; i < list_conditions.Count; i++)
        {
            if (list_conditions[i].piledNum <= 0)
            {
                Affected(i);
                list_conditions.RemoveAt(i);
                continue;
            }
        }
        for (int i = 0; i < list_conditions.Count; i++)
        {
            Affected(i);
            list_conditions[i].DecresePTurn();
        }
        myUI.AbConditionsUpdate();
    }

    private void Affected(int listIndex)
    {
        int _id = list_conditions[listIndex].ID;
        int _power = list_conditions[listIndex].onePower * list_conditions[listIndex].piledNum;

        Debug.Log("상태이상 발동, ID(" + _id + "), 중첩수(" + list_conditions[listIndex].piledNum + ")");

        switch(_id)
        {
            case 0: // 중독
                if (m_target.health - _power < 2)
                {
                    _power = (m_target.health - 1);
                }
                m_target.OnPenDamage(_power);
                break;
            case 1: // 쇠약(1), 파워업(5)
            SetStr:
                m_target.fluc_strength = +_power;
                m_target.CalculateStat();
                break;
            case 2: // 골절(2), 단단함(6)
            SetSolid:
                m_target.fluc_solid = +_power;
                m_target.CalculateStat();
                break;
            case 3: // 고갈(3), 충만(7)
            SetInt:
                m_target.fluc_intel = +_power;
                m_target.CalculateStat();
                break;
            case 4: // 피로(4), 각성(8)
            SetSp:
                m_target.ChangeCost(_power);
                break;
            case 5:
                goto SetStr;
            case 6:
                goto SetSolid;
            case 7:
                goto SetInt;
            case 8:
                goto SetSp;
        }
    }

    public int GetUIInfo(ref Sprite[] icons, ref int[] piledNums, int max)
    {
        if (list_conditions.Count > 0)
        {
            if (max < list_conditions.Count)
            {
                for (int i = 0; i < max; i++)
                {
                    icons[i] = list_conditions[i].Icon;
                    piledNums[i] = list_conditions[i].piledNum;
                }
                return -1;
            }
            else
            {
                for (int i = 0; i < list_conditions.Count; i++)
                {
                    icons[i] = list_conditions[i].Icon;
                    piledNums[i] = list_conditions[i].piledNum;
                }
                return list_conditions.Count;
            }
        }
        else
            return 0;
    }
}
