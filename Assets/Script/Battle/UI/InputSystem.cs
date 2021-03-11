using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float holdingDistance = 5.0f;

    private DiceSystemManager diceSManager;
    private BattleUIManager battleUIManager;

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
        battleUIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<BattleUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SetMousePosition();

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, maxDistance);
            Debug.DrawRay(mousePosition, transform.forward * 10, Color.red, 0.3f);
            if(hit)
            {
                selectedCard = hit.transform.GetComponent<ICard>();
                if(selectedCard != null)
                {
                    isSelected = true;
                    StartCoroutine(MouseHolding(mousePosition));
                    line.SetActive(true);
                }
            }
        }
        if(Input.GetMouseButton(0))
        {
            SetMousePosition();

            if(isSelected)
            {
                selectedCard.Dragged(mousePosition2D, m_line);
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            SetMousePosition();

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, maxDistance);
            Debug.DrawRay(mousePosition, transform.forward * 10, Color.red, 0.3f);
            if(hit)
            {
                selectedCard.SetTarget(hit.transform.gameObject);
                diceSManager.activatedCard = selectedCard;
                battleUIManager.OnDiceSysetm();
            }

            if (isSelected)
            {
                isSelected = false;
                selectedCard = null;
                line.SetActive(false);
            }
        }
    }

    private void SetMousePosition()
    {
        mousePosition = Input.mousePosition;
        mousePosition = myMainCam.ScreenToWorldPoint(mousePosition);
    }

    IEnumerator MouseHolding(Vector2 prevMousePosition)
    {
        yield return new WaitForSeconds(1.0f);
        if ((mousePosition2D - prevMousePosition).magnitude < 5.0f && isSelected)
            selectedCard.Holded();
        yield break;
    }
}
