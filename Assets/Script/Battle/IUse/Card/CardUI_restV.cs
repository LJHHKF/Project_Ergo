using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUI_restV : MonoBehaviour
{
    [SerializeField] private GameObject selectedImg;
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private Transform top;
    [SerializeField] private Transform down;
    private RestSceneManager restM;
    private bool isSelect = false;
    private int m_index;
    private float delay = 0;
    // Start is called before the first frame update
    void Awake()
    {
        restM = GameObject.FindGameObjectWithTag("UIManager").GetComponent<RestSceneManager>();
    }

    private void OnEnable()
    {
        restM.ev_otherSelect += EventOtherSelect;
        
        selectedImg.SetActive(false);
        isSelect = false;
    }

    private void OnDisable()
    {
        restM.ev_otherSelect -= EventOtherSelect;
        if(isSelect)
        {
            isSelect = false;
            restM.ev_DeleteConfirm -= EventDeleteConfirm;
        }
    }

    private void Update()
    {
        //���� UI ��ư�� �ȵǰ�, EventSystem���� �����͵� �ν� ����. ������ ������ ���� ���ϰھ �ܼ��� ������� ����.
        if (delay <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.mousePosition.x >= left.position.x
                    && Input.mousePosition.x <= right.position.x
                    && Input.mousePosition.y >= down.position.y
                    && Input.mousePosition.y <= top.position.y)
                {
                    BTNClicked();
                    delay = 0.2f;
                }
            }
        }
        else
            delay -= Time.deltaTime;
    }

    public void SetIndex(int _index)
    {
        m_index = _index;
    }

    public void BTNClicked()
    {
        if (!isSelect)
        {
            restM.OnEventOtherSelect();
            selectedImg.SetActive(true);
            isSelect = true;
            restM.SetSelected(m_index);
            restM.ev_DeleteConfirm += EventDeleteConfirm;
        }
        else
        {
            selectedImg.SetActive(false);
            isSelect = false;
            restM.UnSetSelected();
            restM.ev_DeleteConfirm -= EventDeleteConfirm;
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
            return DeckManager.instance.DeleteCard_listindex(m_index); //���� ���� 0~n�� ������ �ε��� ������ ����Ʈ�� �ε����̹Ƿ�, ������ �ε����̹Ƿ� �ش� ������� ��. �������� ��� ���� �� ������ ��.
        }
        else
        {
            return false;
        }
    }

}
