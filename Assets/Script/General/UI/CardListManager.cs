using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardListManager : MonoBehaviour
{
    private ScrollRect m_scroll;
    [Header("Object Ref Setting")]
    [SerializeField] private BSCManager m_CardManager;
    //private RectTransform t_content;
    [Header("Object Instantiate Setting")]
    [SerializeField] private GameObject card_UI_Prefab;
    [SerializeField] private float x_padding = 150;
    [SerializeField] private int x_cnt_max = 8;
    [SerializeField] private float y_padding = -200;
    private int x_cnt;
    private int y_cnt;
    private float y_height;
    private List<GameObject> list_spaces = new List<GameObject>();
    private List<Card_Base> list_Input = new List<Card_Base>();
    

    // Start is called before the first frame update
    void Awake()
    {
        m_scroll = gameObject.GetComponent<ScrollRect>();
        y_height = m_scroll.content.rect.height;
    }

    private void CreateSpaces(int _inputMax)
    {
        while(_inputMax > list_spaces.Count)
        {
            x_cnt = list_spaces.Count % x_cnt_max;
            y_cnt = list_spaces.Count / x_cnt_max;

            if (y_height < -(y_cnt * y_padding))
            {
                y_height = -(y_cnt * y_padding) + 400;
                //t_content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y_height);
                m_scroll.content.sizeDelta = new Vector2(m_scroll.content.sizeDelta.x, y_height);
            }

            GameObject temp = Instantiate(card_UI_Prefab, m_scroll.content.transform);
            temp.transform.localPosition = new Vector3(x_cnt * x_padding, y_cnt * y_padding);
            list_spaces.Add(temp);
            temp.SetActive(false);
        }
    }

    public void InputList(bool _isDeck)
    {
        list_Input.Clear();
        if (_isDeck)
            DeckManager.instance.GetDeckList(ref list_Input);
        else
            m_CardManager.GetGraveList(ref list_Input);

        if (list_spaces.Count < list_Input.Count)
            CreateSpaces(list_Input.Count);

        for (int i = 0; i < list_spaces.Count; i++)
        {
            int _i = i;
            if (list_Input.Count > _i)
            {
                list_spaces[_i].SetActive(true);
                list_spaces[_i].GetComponent<Card_UI>().SetTargetCard(list_Input[_i], false);
            }
            else
                list_spaces[_i].SetActive(false);
        }
    }

    public void InputList_RestV()
    {
        list_Input.Clear();
        DeckManager.instance.GetDeckList(ref list_Input);

        if (list_spaces.Count < list_Input.Count)
            CreateSpaces(list_Input.Count);

        for (int i = 0; i < list_spaces.Count; i++)
        {
            int _i = i;
            if (list_Input.Count > _i)
            {
                list_spaces[_i].SetActive(true);
                list_spaces[_i].GetComponent<Card_UI>().SetTargetCard(list_Input[_i], false);
                list_spaces[_i].GetComponent<CardUI_restV>().SetIndex(_i);
            }
            else
                list_spaces[_i].SetActive(false);
        }
    }

    public Card_Base GetInputInfo(int _index)
    {
        return list_Input[_index];
    }


}
