using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSystemManager : MonoBehaviour
{
    [Header("Dice Setting")]
    public GameObject[] normalDice; //2개 설정하면 됨.
    public GameObject[] sixDice;
    public int cnt_dice = 2;
    private List<GameObject> normal_listPool;
    private List<GameObject> six_listPool;
    private bool isOnSixDice = false;
    public GameObject diceChecker;
    public BattleUIManager battleUIManager;

    [Header("Throw Setting")]
    public float power = 500f;
    public float rotatePowerRate = 0.5f;
    private int cnt_RollEnded = 0;
    private bool isReadyToThrow = true;
    public int resValue { get; set; }
    public ICard activatedCard { get; set; }

    [Header("Other")]
    public TurnManager m_TurnM;
    public BSCManager m_CardM;

    // Start is called before the first frame update
    void Start()
    {
        normal_listPool = new List<GameObject>();
        six_listPool = new List<GameObject>();
        for (int i = 0; i < cnt_dice; i++)
        {
            GameObject dice = Instantiate(normalDice[i], gameObject.transform);
            dice.name = "NormalDice_" + (i + 1).ToString("0");
            dice.SetActive(false);
            normal_listPool.Add(dice);

            GameObject sixdice = Instantiate(sixDice[i], gameObject.transform);
            sixdice.name = "SixDice_" + (i + 1).ToString("0");
            sixdice.SetActive(false);
            six_listPool.Add(sixdice);
        }

        diceChecker.SetActive(false);
        resValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0) && isReadyToThrow)
        //{
        //    ActiveDice();
        //}
        
        if(cnt_RollEnded == 2)
        {
            ActiveChecker();
        }
    }

    public bool GetIsReadyToThrow()
    {
        return isReadyToThrow;
    }

    public void ActiveDice(out bool isSucess)
    {
        battleUIManager.OffDiceSystem();
        if (isReadyToThrow && activatedCard != null)
        {
            cnt_RollEnded = 0;
            resValue = 0;
            isReadyToThrow = false;
            if (isOnSixDice)
            {
                for (int i = 0; i < six_listPool.Count; i++)
                {
                    six_listPool[i].SetActive(true);
                    six_listPool[i].transform.position = new Vector3(-2 + (i * 4), gameObject.transform.position.y, -7);
                    six_listPool[i].transform.localEulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
                    Rigidbody rb = normal_listPool[i].GetComponent<Rigidbody>();
                    rb.AddForce(transform.up * power);
                    rb.AddTorque(Random.Range(power * rotatePowerRate * 0.5f, power * rotatePowerRate)
                        , Random.Range(power * rotatePowerRate * 0.5f, power * rotatePowerRate)
                        , Random.Range(power * rotatePowerRate * 0.5f, power * rotatePowerRate));
                }
            }
            else
            {
                for (int i = 0; i < normal_listPool.Count; i++)
                {
                    normal_listPool[i].SetActive(true);
                    normal_listPool[i].transform.position = new Vector3(-2 + (i * 4), gameObject.transform.position.y, -7);
                    normal_listPool[i].transform.localEulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
                    Rigidbody rb = normal_listPool[i].GetComponent<Rigidbody>();
                    rb.AddForce(transform.up * power);
                    rb.AddTorque(Random.Range(power * rotatePowerRate * 0.5f, power * rotatePowerRate)
                        , Random.Range(power * rotatePowerRate * 0.5f, power * rotatePowerRate)
                        , Random.Range(power * rotatePowerRate * 0.5f, power * rotatePowerRate));
                }

            }
            isSucess = true;
        }
        else
        {
            isSucess = false;
        }
    }

    public void SumRollEnd()
    {
        cnt_RollEnded += 1;
    }

    private void ActiveChecker()
    {
        diceChecker.SetActive(true);
    }

    public void CheckEnd()
    {
        if(cnt_RollEnded == 2)
        {
            cnt_RollEnded = 0;
            StartCoroutine(UnActiveDice());
        }
    }

    public void OnSixDice()
    {
        isOnSixDice = true;
    }

    IEnumerator UnActiveDice()
    {
        yield return new WaitForSeconds(1.0f);
        isReadyToThrow = true;
        if (isOnSixDice)
        {
            isOnSixDice = false;
            for (int i = 0; i < six_listPool.Count; i++)
            {
                six_listPool[i].GetComponent<DiceManager>().SetResValue();
            }
            for (int i = 0; i < six_listPool.Count; i++)
            {
                six_listPool[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < normal_listPool.Count; i++)
            {
                normal_listPool[i].GetComponent<DiceManager>().SetResValue();
            }
            for (int i = 0; i < normal_listPool.Count; i++)
            {
                normal_listPool[i].SetActive(false);
            }
        }

        Debug.Log("결과: " + resValue);

        activatedCard.Use(resValue);
        activatedCard = null;
        m_CardM.UndoHandsTaransparency();

        yield break;
    }
}
