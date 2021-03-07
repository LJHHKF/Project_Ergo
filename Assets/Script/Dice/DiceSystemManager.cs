using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSystemManager : MonoBehaviour
{
    [Header("다이스 설정")]
    public GameObject[] normalDice; //2개 설정하면 됨.
    //public GameObject sixDice;
    private List<GameObject> normal_listPool;
    //private List<GameObject> six_listPool;
    public GameObject diceChecker;

    [Header("던지기 설정")]
    public float power = 500f;
    public float rotatePowerRate = 0.5f;
    private int cnt_RollEnded = 0;
    private bool isReadyToThrow = true;
    private int result = 0;
    public int resValue { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        normal_listPool = new List<GameObject>();
        for (int i = 0; i < normalDice.Length; i++)
        {
            GameObject dice = Instantiate(normalDice[i], gameObject.transform);
            dice.name = "NormalDice_" + (i + 1).ToString("0");
            dice.SetActive(false);
            normal_listPool.Add(dice);
            
            //여기서 sixDice도 똑같이 만들고 리스트화하면 됨.
        }

        diceChecker.SetActive(false);
        resValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isReadyToThrow)
        {
            ActiveDice();
        }
        
        if(cnt_RollEnded == 2)
        {
            ActiveChecker();
        }
    }

    private void ActiveDice()
    {
        cnt_RollEnded = 0;
        resValue = 0;
        isReadyToThrow = false;
        for (int i = 0; i < normal_listPool.Count; i++)
        {
            normal_listPool[i].SetActive(true);
            normal_listPool[i].transform.position = new Vector3(-2 + (i * 4), 0, -8);
            normal_listPool[i].transform.localEulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            Rigidbody rb = normal_listPool[i].GetComponent<Rigidbody>();
            rb.AddForce(transform.up * power);
            rb.AddTorque(Random.Range(power * rotatePowerRate * 0.5f, power * rotatePowerRate)
                , Random.Range(power * rotatePowerRate * 0.5f, power * rotatePowerRate)
                , Random.Range(power * rotatePowerRate * 0.5f, power * rotatePowerRate));
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

    IEnumerator UnActiveDice()
    {
        yield return new WaitForSeconds(1.0f);
        isReadyToThrow = true;
        for (int i = 0; i < normal_listPool.Count; i++)
        {
            normal_listPool[i].GetComponent<DiceManager>().SetResValue();
        }
        for (int i = 0; i < normal_listPool.Count; i++)
        {
            normal_listPool[i].SetActive(false);
        }

        Debug.Log("결과: " + resValue);

        yield break;
    }
}
