using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InputSystem : MonoBehaviour
{
    public static InputSystem instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<InputSystem>();
            }
            return m_instance;
        }
    }
    private static InputSystem m_instance;

    private GameObject line;
    private LineDrawer m_line;
    private Vector2 mousePosition2D
    {
        get
        {
            if(mousePosition == null)
            {
                mousePosition = Input.mousePosition;
            }
         Vector2 temp = mousePosition;
         return temp;
        }
    }
    private Vector3 mousePosition;
    private Camera myMainCam;
    private float maxDistance = 15f;
    private Card_Base selectedCard;
    private IItem selectedItem;
    private int useTypeNum = -1;
    private bool isSelected = false;
    private Vector2 targetedPos;
    private bool isTempTargeted = false;
    private Vector2 targetColSize;
    private string SceneName;
    private bool isInEnlargeArea;

    //[SerializeField] private Canvas m_cardCanvas;
    //private GraphicRaycaster m_GRay;
    //private PointerEventData m_ped;

    private DiceSystemManager diceSManager;
    private BattleUIManager m_BaUIManager;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameMaster.instance.battleStageStart += Event_BattleStageStart;
        //TurnManager.instance.firstTurn += Event_BattleStageStart;

        //m_GRay = m_cardCanvas.GetComponent<GraphicRaycaster>();
        //m_ped = new PointerEventData(null);
    }

    private void Event_BattleStageStart()
    {
        myMainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        line = GameObject.FindGameObjectWithTag("Line");
        m_line = line.GetComponent<LineDrawer>();
        line.SetActive(false);
        diceSManager = GameObject.FindGameObjectWithTag("DiceBox").GetComponent<DiceSystemManager>();
        m_BaUIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<BattleUIManager>();
    }

    private void OnDestroy()
    {
        GameMaster.instance.battleStageStart -= Event_BattleStageStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneName == "Battle")
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetMousePosition();

                RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, maxDistance);

                //List<RaycastResult> results = new List<RaycastResult>();
                //m_GRay.Raycast(m_ped, results);

                //if (results.Count != 0)
                if (hit)
                {
                    if (hit.collider.tag == "Card")
                    {
                        if (diceSManager.GetIsReadyToThrow())
                        {
                            selectedCard = (Card_Base)hit.transform.GetComponent<ICard>().Selected();
                            if (selectedCard != null)
                            {
                                useTypeNum = 0;
                                isSelected = true;
                                m_BaUIManager.OnEnlargeCard(selectedCard);
                                line.SetActive(true);
                            }
                        }
                    }
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (isSelected)
                {
                    SetMousePosition();

                    if (useTypeNum == 0)
                    {
                        if (!selectedCard.GetIsNonTarget())
                        {
                            Target_Use_Hold_Method();
                        }
                        else
                        {
                            isInEnlargeArea = selectedCard.Dragged(mousePosition2D, m_line);
                        }

                        if (!isInEnlargeArea)
                            m_BaUIManager.OffEnlargeCard();
                        else
                            m_BaUIManager.OnEnlargeCard(selectedCard);
                    }
                    else if (useTypeNum == 1)
                    {
                        if (!selectedItem.GetIsNonTarget())
                        {
                            Target_Use_Hold_Method();
                        }
                        else
                        {
                            isInEnlargeArea = selectedItem.Dragged(mousePosition2D, m_line);
                        }
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (isSelected)
                {
                    switch (useTypeNum)
                    {
                        case 0:
                            if (selectedCard.GetIsNonTarget())
                            {
                                if (selectedCard.GetReady())
                                {
                                    selectedCard.SetTarget(null);
                                    diceSManager.activatedCard = selectedCard;
                                }
                            }
                            else
                            {
                                SetMousePosition();

                                RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, maxDistance);
                                if (hit)
                                {
                                    if (hit.collider.CompareTag("Enemy"))
                                    {
                                        selectedCard.SetTarget(hit.transform.gameObject);
                                        diceSManager.activatedCard = selectedCard;
                                    }
                                }
                            }
                            m_BaUIManager.OffEnlargeCard();
                            selectedCard.BringUpCard(false);
                            selectedCard = null;
                            break;
                        case 1:
                            if (selectedItem.GetIsNonTarget())
                            {
                                if (selectedItem.GetReady())
                                {
                                    selectedItem.SetTarget(null);
                                    //selectedItem.Use(0); // Card도, Item도, SetTarget 안에 Use가 있음.
                                }
                                else
                                {
                                    selectedItem.UnSetSelected();
                                }
                            }
                            else
                            {
                                SetMousePosition();

                                RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, maxDistance);
                                if (hit)
                                {
                                    if (hit.collider.CompareTag("Enemy"))
                                    {
                                        selectedItem.SetTarget(hit.transform.gameObject);
                                    }
                                    else
                                        selectedItem.UnSetSelected();
                                }
                                else
                                    selectedItem.UnSetSelected();
                            }
                            selectedItem = null;
                            break;
                    }
                    isSelected = false;
                    useTypeNum = -1;
                    line.SetActive(false);

                    isTempTargeted = false;
                    targetedPos = Vector2.zero;
                    targetColSize = Vector2.zero;
                }
            }
        }
    }

    private void SetMousePosition()
    {
        mousePosition = Input.mousePosition;
        mousePosition = myMainCam.ScreenToWorldPoint(mousePosition);
        
        //m_ped.position = Input.mousePosition;
    }

    public void SetSceneName(string _input)
    {
        SceneName = _input;
    }

    public void SetItem(IItem _item)
    {
        selectedItem = _item;
        isSelected = true;
        useTypeNum = 1;
        line.SetActive(true);
    }

    private void Target_Use_Hold_Method()
    {
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, maxDistance);

        if (hit)
        {
            if (hit.transform.gameObject.tag == "Enemy")
            {
                targetedPos = hit.transform.position;
                targetColSize = hit.transform.GetComponent<Collider2D>().bounds.size;
                targetColSize = targetColSize / 2;
                isTempTargeted = true;
            }
        }

        if (mousePosition.x < targetedPos.x - targetColSize.x
            || mousePosition.x > targetedPos.x + targetColSize.x
            || mousePosition.y < targetedPos.y - targetColSize.y
            || mousePosition.y > targetedPos.y + targetColSize.y)
        {
            isTempTargeted = false;
            targetedPos = Vector2.zero;
            targetColSize = Vector2.zero;
        }

        if (isTempTargeted)
        {
            switch (useTypeNum)
            {
                case 0:
                    isInEnlargeArea = selectedCard.Dragged(targetedPos, m_line);
                    break;
                case 1:
                    isInEnlargeArea = selectedItem.Dragged(targetedPos, m_line);
                    break;
            }
            
        }
        else
        {
            switch (useTypeNum)
            {
                case 0:
                    isInEnlargeArea = selectedCard.Dragged(mousePosition2D, m_line);
                    break;
                case 1:
                    isInEnlargeArea = selectedItem.Dragged(mousePosition2D, m_line);
                    break;
            }
        }
    }
}
