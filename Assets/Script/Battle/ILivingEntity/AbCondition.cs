using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AbCondition : MonoBehaviour
{
    private class AbCond
    {
        public int ID;
        public Sprite Icon;
        public int piledNum;
        public int onePower;

        public AbCond(int _id, /*Sprite _icon,*/ int _piledNum, int _onePower)
        {
            ID = _id;
            //Icon = _icon;
            piledNum = _piledNum;
            onePower = _onePower;
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

    private List<AbCond> list_conditions = new List<AbCond>();
    private List<AbCond> list_delayed = new List<AbCond>();
    [SerializeField] private LivingEntity m_target;
    [SerializeField] private UnitUI myUI;

    private void Awake()
    {
        int capacity = AbCondInfoManager.instance.GetAbCondListLength() + 1;
        list_conditions.Capacity = capacity;
        list_delayed.Capacity = capacity;
    }

    private void AddAbCondition(int id, int piledN)
    {
        bool isBeing = false;

        for (int i = 0; i < list_conditions.Count; i++)
        {
            int _i = i;
            if (list_conditions[_i].ID == id)
            {
                isBeing = true;
                list_conditions[_i].IncreaseP(piledN);
                break;
            }
        }

        if (!isBeing)
        {
            AbCond temp = new AbCond(id, /*AbCondInfoManager.instance.GetAbCond_Img(id),*/ piledN, AbCondInfoManager.instance.GetAbCond_OnePower(id));

            //temp.ID = id;
            //temp.Icon = abCondInfoM.GetAbCond_Img(id);
            //temp.piledNum = piledN;
            //temp.onePower = abCondInfoM.GetAbCond_OnePower(id);
            list_conditions.Add(temp);
        }
    }

    public void AddImdiateAbCondition(int id, int piledN)
    {
        bool isBeing = false;

        for (int i = 0; i < list_conditions.Count; i++)
        {
            int _i = i;
            if (list_conditions[_i].ID == id)
            {
                isBeing = true;
                list_conditions[_i].IncreaseP(piledN);
                Affected(_i);
                break;
            }
        }

        if (!isBeing)
        {
            AbCond temp = new AbCond(id, /*AbCondInfoManager.instance.GetAbCond_Img(id),*/ piledN, AbCondInfoManager.instance.GetAbCond_OnePower(id));
            //temp.ID = id;
            //temp.Icon = abCondInfoM.GetAbCond_Img(id);
            //temp.piledNum = piledN;
            //temp.onePower = abCondInfoM.GetAbCond_OnePower(id);


            list_conditions.Add(temp);
            Affected(list_conditions.Count - 1);
        }

        myUI.AbConditionsUpdate();
    }

    public void AddDelayedCondition(int id, int piledN)
    {
        bool isBeing = false;

        for (int i = 0; i < list_delayed.Count; i++)
        {
            int _i = i;
            if (list_delayed[_i].ID == id)
            {
                isBeing = true;
                list_delayed[_i].IncreaseP(piledN);
                if (list_delayed[_i].ID <= 4)
                    myUI.AddPopUpText_Debuff($"(D){AbCondInfoManager.instance.GetAbCond_Name(list_delayed[_i].ID)}", list_delayed[_i].piledNum);
                else
                    myUI.AddPopUpText_Buff($"(D){AbCondInfoManager.instance.GetAbCond_Name(list_delayed[_i].ID)}", list_delayed[_i].piledNum);
                break;
            }
        }

        if (!isBeing)
        {
            AbCond temp = new AbCond(id, piledN, AbCondInfoManager.instance.GetAbCond_OnePower(id));

            if (id <= 4)
                myUI.AddPopUpText_Debuff($"(D){AbCondInfoManager.instance.GetAbCond_Name(id)}", piledN);
            else
                myUI.AddPopUpText_Buff($"(D){AbCondInfoManager.instance.GetAbCond_Name(id)}", piledN);

            list_delayed.Add(temp);
        }

        myUI.AbConditionsUpdate();
    }

    public void Affected() //자신에 걸린 모든 효과 발동
    {
        if (list_delayed.Count > 0)
            D_Affected();
        for(int i = 0; i < list_conditions.Count; i++)
        {
            int _i = i;
            if (list_conditions[_i].piledNum <= 0)
            {
                Affected(_i);
                list_conditions.RemoveAt(_i);
                continue;
            }
            Affected(_i); // 순서 바꿔서 코드 줄이려 하지 말 것. 이유 있음.
        }
        myUI.AbConditionsUpdate();
    }

    private void Affected(int listIndex)
    {
        int _id = list_conditions[listIndex].ID;
        int _power = list_conditions[listIndex].onePower * list_conditions[listIndex].piledNum;

        if (list_conditions[listIndex].piledNum != 0)
        {
            if (_id <= 4)
                myUI.AddPopUpText_Debuff(AbCondInfoManager.instance.GetAbCond_Name(_id), list_conditions[listIndex].piledNum);
            else
                myUI.AddPopUpText_Buff(AbCondInfoManager.instance.GetAbCond_Name(_id), list_conditions[listIndex].piledNum);
        }

        list_conditions[listIndex].DecreseP(1);

        switch (_id)
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
                m_target.fluc_strength += _power;
                m_target.CalculateStat();
                break;
            case 2: // 골절(2), 단단함(6)
            SetSolid:
                m_target.fluc_solid += _power;
                m_target.CalculateStat();
                break;
            case 3: // 고갈(3), 충만(7)
            SetInt:
                m_target.fluc_intel += _power;
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

    private void D_Affected()
    {
        for (int i = 0; i < list_delayed.Count; i++)
        {
            int _i = i;
            AddAbCondition(list_delayed[_i].ID, list_delayed[_i].piledNum);
        }
        list_delayed.Clear();
    }

    public int GetUIInfo(/*ref Sprite[] icons,*/ ref int[] id, ref int[] piledNums, ref bool[] isDAbs, int max)
    {
        int c_sum = list_conditions.Count + list_delayed.Count;
        int cnt = 0;
        if (c_sum > 0)
        {
            if (max < c_sum)
            {
                while (cnt == max)
                {
                    if (list_conditions.Count != 0)
                    {
                        bool isBreaked = false;
                        while (cnt >= list_conditions.Count)
                        {
                            //icons[cnt] = list_conditions[cnt].Icon;
                            id[cnt] = list_conditions[cnt].ID;
                            piledNums[cnt] = list_conditions[cnt].piledNum;
                            isDAbs[cnt] = false;

                            if (cnt++ == max)
                            {
                                isBreaked = true;
                                break;
                            }
                        }
                        if (isBreaked)
                            break;
                    }
                    while (cnt < c_sum)
                    {
                        int j = cnt - list_conditions.Count;
                        //icons[cnt] = list_delayed[j].Icon;
                        id[cnt] = list_delayed[j].ID;
                        piledNums[cnt] = list_delayed[j].piledNum;
                        isDAbs[cnt] = true;

                        if (++cnt == max)
                        {
                            break;
                        }
                    }
                }
                return -1;
            }
            else
            {
                if (list_conditions.Count > 0)
                {
                    while (cnt < list_conditions.Count)
                    {
                        //icons[cnt] = list_conditions[cnt].Icon;
                        id[cnt] = list_conditions[cnt].ID;
                        piledNums[cnt] = list_conditions[cnt].piledNum;
                        isDAbs[cnt] = false;
                        cnt++;
                    }
                }
                while (cnt < c_sum)
                {
                    int j = cnt - list_conditions.Count;
                    //icons[cnt] = list_delayed[j].Icon;
                    id[cnt] = list_delayed[j].ID;
                    piledNums[cnt] = list_delayed[j].piledNum;
                    isDAbs[cnt] = true;
                    cnt++;
                }
                return c_sum;
            }
        }
        else
            return 0;
    }
}
