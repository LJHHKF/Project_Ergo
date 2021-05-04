using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUI_restV : MonoBehaviour
{
    [SerializeField] private GameObject selectedImg;
    private RestSceneManager restM;
    private bool isSelect = false;
    private int m_index;
    // Start is called before the first frame update
    void Start()
    {
        restM = GameObject.FindGameObjectWithTag("UIManager").GetComponent<RestSceneManager>();
    }

    private void OnEnable()
    {
        restM.ev_otherSelect.AddListener(EventOtherSelect);
        
        selectedImg.SetActive(false);
    }

    private void OnDisable()
    {
        restM.ev_otherSelect.RemoveListener(EventOtherSelect);
        if(isSelect)
        {
            isSelect = false;
            restM.ev_DeleteConfirm -= EventDeleteConfirm;
        }
    }

    public void SetIndex(int _index)
    {
        m_index = _index;
    }

    public void BTNClicked()
    {
        if (isSelect)
        {
            selectedImg.SetActive(false);
            isSelect = false;
            restM.UnSetSelected();
            restM.ev_DeleteConfirm -= EventDeleteConfirm;
        }
        else
        {
            restM.OnEventOtherSelect();
            selectedImg.SetActive(true);
            isSelect = true;
            restM.SetSelected(m_index);
            restM.ev_DeleteConfirm += EventDeleteConfirm;
        }
    }

    private void EventOtherSelect()
    {
        if (isSelect)
        {
            selectedImg.SetActive(false);
            isSelect = false;
        }
    }
    private bool EventDeleteConfirm()
    {
        if(isSelect)
        {
            return DeckManager.instance.DeleteCard_listindex(m_index); //기존 덱의 0~n의 순서로 인덱스 가져온 리스트의 인덱스이므로, 동일한 인덱스이므로 해당 방식으로 함. 가져오는 방식 변경 시 수정할 것.
        }
        else
        {
            return false;
        }
    }

}
