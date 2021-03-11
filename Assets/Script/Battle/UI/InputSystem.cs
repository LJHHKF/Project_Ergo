﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    public GameObject line;
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
    private ICard selectedCard;
    private bool isSelected = false;
    private Vector2 targetedPos;
    private bool isTempTargeted = false;
    private Vector2 targetColSize;

    public float holdingDistance = 5.0f;

    public Canvas m_cardCanvas;
    private GraphicRaycaster m_GRay;
    private PointerEventData m_ped;
    

    private DiceSystemManager diceSManager;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        line.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        myMainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        m_line = line.GetComponent<LineDrawer>();
        diceSManager = GameObject.FindGameObjectWithTag("DiceBox").GetComponent<DiceSystemManager>();

        m_GRay = m_cardCanvas.GetComponent<GraphicRaycaster>();
        m_ped = new PointerEventData(null);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SetMousePosition();

            if (diceSManager.GetIsReadyToThrow())
            {
                //RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, maxDistance);
                //Debug.DrawRay(mousePosition, transform.forward * 10, Color.red, 0.3f);

                List<RaycastResult> results = new List<RaycastResult>();
                m_GRay.Raycast(m_ped, results);
                
                //if (hit)
                if(results.Count != 0)
                {
                    //selectedCard = hit.transform.GetComponent<ICard>();
                    if(results[0].gameObject.CompareTag("Card"))
                    {
                        selectedCard = results[0].gameObject.GetComponent<ICard>();
                    }
                    if (selectedCard != null)
                    {
                        isSelected = true;
                        StartCoroutine(MouseHolding(mousePosition));
                        line.SetActive(true);
                    }
                }
            }
        }
        if(Input.GetMouseButton(0))
        {
            if (isSelected)
            {
                SetMousePosition();
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, maxDistance);
                Debug.DrawRay(mousePosition, transform.forward * 10, Color.red, 0.3f);

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
                    selectedCard.Dragged(targetedPos, m_line);

                }
                else
                {
                    selectedCard.Dragged(mousePosition2D, m_line);
                }
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            if (isSelected)
            {
                SetMousePosition();

                RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, maxDistance);
                Debug.DrawRay(mousePosition, transform.forward * 10, Color.red, 0.3f);
                if (hit)
                {
                    selectedCard.SetTarget(hit.transform.gameObject);
                    diceSManager.activatedCard = selectedCard;
                }
                isSelected = false;
                selectedCard = null;
                line.SetActive(false);

                isTempTargeted = false;
                targetedPos = Vector2.zero;
                targetColSize = Vector2.zero;
            }
        }
    }

    private void SetMousePosition()
    {
        mousePosition = Input.mousePosition;
        mousePosition = myMainCam.ScreenToWorldPoint(mousePosition);
        m_ped.position = Input.mousePosition;
    }

    IEnumerator MouseHolding(Vector2 prevMousePosition)
    {
        yield return new WaitForSeconds(1.0f);
        if ((mousePosition2D - prevMousePosition).magnitude < 5.0f && isSelected)
            selectedCard.Holded();
        yield break;
    }
}
