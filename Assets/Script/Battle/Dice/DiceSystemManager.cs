using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceSystemManager : MonoBehaviour
{
    [Header("Dice Setting")]
    [SerializeField] private GameObject normalDice; //2개 설정하면 됨.
    [SerializeField] private GameObject sixDice;
    [SerializeField] private int cnt_dice = 2;
    private List<GameObject> normal_listPool = new List<GameObject>();
    private List<GameObject> six_listPool = new List<GameObject>();
    private bool isOnSixDice = false;
    [SerializeField] private GameObject diceChecker;
    [SerializeField] private BattleUIManager battleUIManager;

    [Header("Throw Setting")]
    [SerializeField] private float power = 500f;
    [SerializeField] private float rotatePower = 300f;
    [SerializeField] private float rotatePowerMinRate = 0.5f;
    [SerializeField] private float spawnP_minX = -2;
    [SerializeField] private float interval_x = 4;
    [SerializeField] private float spawnP_Y = 2;
    [SerializeField] private float spawnP_Z = -7;
    private int cnt_RollEnded = 0;
    private bool isReadyToThrow = true;
    public int resValue { get; set; }
    public ICard activatedCard { get; set; }

    [Header("Other")]
    [SerializeField] private BSCManager m_CardM;
    [SerializeField] private Image[] d_resImgs;
    private int imgIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < cnt_dice; i++)
        {
            GameObject dice = Instantiate(normalDice, gameObject.transform);
            dice.name = "NormalDice_" + (i + 1).ToString("0");
            dice.SetActive(false);
            normal_listPool.Add(dice);

            GameObject sixdice = Instantiate(sixDice, gameObject.transform);
            sixdice.name = "SixDice_" + (i + 1).ToString("0");
            sixdice.SetActive(false);
            six_listPool.Add(sixdice);
        }

        diceChecker.SetActive(false);
        resValue = 0;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    //if (Input.GetMouseButtonDown(0) && isReadyToThrow)
    //    //{
    //    //    ActiveDice();
    //    //}
    //}

    public bool GetIsReadyToThrow()
    {
        return isReadyToThrow;
    }

    public void ActiveDice(out bool isSuccess)
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
                    six_listPool[i].transform.position = new Vector3(spawnP_minX + (i * interval_x), transform.position.y + spawnP_Y, spawnP_Z);
                    //six_listPool[i].transform.position = new Vector3(spawnPoint.position.x + spawnP_minX + (i * interval_x), spawnPoint.position.y, spawnPoint.position.z);
                    six_listPool[i].transform.localEulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

                    DiceManager m_dice = six_listPool[i].GetComponent<DiceManager>();
                    m_dice.SetDiceRollPower(rotatePower, rotatePowerMinRate);

                    Rigidbody rb = normal_listPool[i].GetComponent<Rigidbody>();
                    rb.AddForce(transform.up * power);
                    //rb.AddTorque(Random.Range(rotatePower * rotatePowerMinRate, rotatePower)
                    //    , Random.Range(rotatePower * rotatePowerMinRate, rotatePower)
                    //    , Random.Range(rotatePower * rotatePowerMinRate, rotatePower));
                }
            }
            else
            {
                for (int i = 0; i < normal_listPool.Count; i++)
                {
                    normal_listPool[i].SetActive(true);
                    normal_listPool[i].transform.position = new Vector3(spawnP_minX + (i * interval_x), transform.position.y + spawnP_Y, spawnP_Z);
                    //normal_listPool[i].transform.position = new Vector3(spawnPoint.position.x + spawnP_minX + (i * interval_x), spawnPoint.position.y, spawnPoint.position.z);
                    normal_listPool[i].transform.localEulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

                    DiceManager m_dice = normal_listPool[i].GetComponent<DiceManager>();
                    m_dice.SetDiceRollPower(rotatePower, rotatePowerMinRate);

                    Rigidbody rb = normal_listPool[i].GetComponent<Rigidbody>();
                    rb.AddForce(transform.up * power);
                    //rb.AddTorque(Random.Range(rotatePower * rotatePowerMinRate, rotatePower)
                    //    , Random.Range(rotatePower * rotatePowerMinRate, rotatePower)
                    //    , Random.Range(rotatePower * rotatePowerMinRate, rotatePower));
                }

            }
            isSuccess = true;
        }
        else
        {
            isSuccess = false;
        }
    }

    public void SumRollEnd()
    {
        cnt_RollEnded += 1;

        if(cnt_RollEnded >= cnt_dice)
        {
            cnt_RollEnded = 0;
            diceChecker.SetActive(true);
            StartCoroutine(UnActiveDice());
        }
    }

    //public void CheckEnd()
    //{
    //    if(cnt_RollEnded == 2)
    //    {
    //        cnt_RollEnded = 0;
    //        StartCoroutine(UnActiveDice());
    //    }
    //}

    public void SetResImg(Sprite _img)
    {
        if(imgIndex < d_resImgs.Length)
        {
            d_resImgs[imgIndex++].sprite = _img;
        }
    }

    public void OnSixDice()
    {
        isOnSixDice = true;
    }

    IEnumerator UnActiveDice()
    {
        yield return new WaitForSeconds(1.0f);
        diceChecker.SetActive(false);
        isReadyToThrow = true;
        if (isOnSixDice)
        {
            isOnSixDice = false;
            for (int i = 0; i < six_listPool.Count; i++)
            {
                six_listPool[i].GetComponent<DiceManager>().SetResValue();
                six_listPool[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < normal_listPool.Count; i++)
            {
                normal_listPool[i].GetComponent<DiceManager>().SetResValue();
                normal_listPool[i].SetActive(false);
            }
        }

        activatedCard.Use(resValue);
        activatedCard = null;
        m_CardM.UndoHandsTransparency();

        imgIndex = 0;
        battleUIManager.OnDiceRes();

        yield break;
    }
}
