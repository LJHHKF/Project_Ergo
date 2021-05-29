using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class ItemSlot : MonoBehaviour
{
    private static ItemSlot m_instance;
    public static ItemSlot instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<ItemSlot>();
            return m_instance;
        }
    }

    [SerializeField] private int slot_max = 4;
    private float use_min_X = 100;
    private float screenSize_x;

    private List<GameObject> item_list = new List<GameObject>();
    private GameObject rewardItem = null;
    private StringBuilder m_sb = new StringBuilder();
    private string key;

    public event Action ev_listChange;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        item_list.Capacity = slot_max;
        RectTransform temp = GameObject.FindGameObjectWithTag("UIManager").GetComponent<RectTransform>();
        screenSize_x = temp.sizeDelta.x * temp.localScale.x;
    }

    private void OnEnable()
    {
        GameMaster.instance.initSaveData_Start += GameStart_Init;
        GameMaster.instance.startGame_Start += GameStart;
        GameMaster.instance.gameOver += ClearList;
        GameMaster.instance.battleStageStart += BattleStageStart;
        GameMaster.instance.stageEnd += StageEnd;
    }

    private void OnDisable()
    {
        GameMaster.instance.initSaveData_Start -= GameStart_Init;
        GameMaster.instance.startGame_Start -= GameStart;
        GameMaster.instance.gameOver -= ClearList;
        GameMaster.instance.battleStageStart -= BattleStageStart;
        GameMaster.instance.stageEnd -= StageEnd;
    }

    private void GameStart()
    {
        m_sb.Clear();
        key = $"SaveID{GameMaster.instance.GetSaveID()}.ItemSlot.";
        m_sb.Append(key);
        for(int i = 0; i < slot_max; i++)
        {
            int _i = i;
            if (_i == 0)
                m_sb.Append(0);
            else
                m_sb.Replace($"ItemSlot.{_i - 1}", $"ItemSlot.{_i}");

            if(PlayerPrefs.HasKey(m_sb.ToString()) && PlayerPrefs.GetInt(m_sb.ToString()) >= 0)
            {
                AddItem(PlayerPrefs.GetInt(m_sb.ToString()));
            }
            else
            {
                PlayerPrefs.SetInt(m_sb.ToString(), -1);
            }
        }
    }

    private void GameStart_Init()
    {
        m_sb.Clear();
        key = $"SaveID{GameMaster.instance.GetSaveID()}.ItemSlot.";
        m_sb.Append(key);
        for (int i = 0; i < slot_max; i++)
        {
            int _i = i;
            if (_i == 0)
                m_sb.Append(0);
            else
                m_sb.Replace($"ItemSlot.{_i - 1}", $"ItemSlot.{_i}");

            PlayerPrefs.SetInt(m_sb.ToString(), -1);
        }
    }

    private void BattleStageStart()
    {
        SaveList();
        use_min_X = screenSize_x * 0.13f;
    }

    private void StageEnd()
    {
        SaveList();
        use_min_X = screenSize_x;
    }

    private void ClearList()
    {
        item_list.Clear();
        m_sb.Clear();
        key = $"SaveID{GameMaster.instance.GetSaveID()}.ItemSlot.";
        m_sb.Append(key);
        for (int i = 0; i < slot_max; i++)
        {
            int _i = i;
            if (_i == 0)
                m_sb.Append(0);
            else
                m_sb.Replace($"ItemSlot.{_i - 1}", $"ItemSlot.{_i}");

            PlayerPrefs.DeleteKey(m_sb.ToString());
        }
    }

    private void SaveList()
    {
        m_sb.Clear();
        //key = $"SaveID{GameMaster.instance.GetSaveID()}.ItemSlot.";
        m_sb.Append(key);
        for(int i = 0; i < slot_max; i++)
        {
            int _i = i;
            if (_i == 0)
                m_sb.Append(0);
            else
                m_sb.Replace($"ItemSlot.{_i - 1}", $"ItemSlot.{_i}");

            if (_i < item_list.Count)
                PlayerPrefs.SetInt(m_sb.ToString(), item_list[_i].GetComponent<IItem>().GetID());
            else
                PlayerPrefs.SetInt(m_sb.ToString(), -1);
        }
    }

    public bool GetCanAdd()
    {
        if (item_list.Count >= slot_max)
            return false;
        else
            return true;
    }

    public int GetSlotCount()
    {
        return item_list.Count;
    }

    public void AddItem(int _id)
    {
        if (GetCanAdd())
        {
            GameObject temp = Instantiate(ItemInfoManager.instance.GetItem(_id), gameObject.transform);
            temp.GetComponent<IItem>().SetSlotIndex(item_list.Count); // add 후면 -1을 해줘야 해서 이 때 바로 함.
            item_list.Add(temp);
            ev_listChange?.Invoke();
        }
    }

    public void ItemSelected(int _index, Transform t_btn)
    {
        item_list[_index].GetComponent<IItem>().SetBtnPos(t_btn);
        InputSystem.instance.SetItem((IItem)item_list[_index].GetComponent<IItem>().Selected());
    }

    public Sprite GetItemImg(int _index)
    {
        return item_list[_index].GetComponent<IItem>().GetItemImg();
    }

    public bool CheckUseMinX(float _x)
    {
        if (_x > use_min_X)
            return true;
        else
            return false;
    }

    public void DeleteItem(int _index)
    {
        GameObject temp = item_list[_index];
        item_list.RemoveAt(_index);
        DestroyImmediate(temp, true);
        ev_listChange?.Invoke();
    }

    public IItem SetRewardItem_ready()
    {
        //if(GetCanAdd)
        rewardItem = ItemInfoManager.instance.GetItem_Random();
        return rewardItem.GetComponent<IItem>();
    }

    public void UnSetRewardItem()
    {
        rewardItem = null;
    }

    public void SetRewardItem_confirm()
    {
        rewardItem.GetComponent<IItem>().SetSlotIndex(item_list.Count);
        item_list.Add(rewardItem);
    }


    public void SetUseMinX(float value)
    {
        use_min_X = value;
    }
}
