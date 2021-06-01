using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    private bool isGetRes = false;
    private bool isRollEnd = false;
    private Rigidbody m_rb;
    private DiceSystemManager m_DsystemManager;
    private int resNum = 0;
    private Sprite resImg;

    private bool isBottom = false;
    private float rollPower = 0;
    private float rollMinPowerRate = 0;
    private Quaternion t_rot;
    private bool isSetTargetRot = false;
    private bool isRotEnd = false;
    private float bottomTime = 0f;
    [SerializeField] private float chkTimeInterval = 1.0f;

    private void OnEnable()
    {
        isRollEnd = false;
        isGetRes = false;
        isBottom = false;
        isSetTargetRot = false;
        isRotEnd = false;
        bottomTime = 0;
    }

    private void Start()
    {
        m_rb = gameObject.GetComponent<Rigidbody>();
        m_DsystemManager = GameObject.FindGameObjectWithTag("DiceBox").GetComponent<DiceSystemManager>();
    }

    private void Update()
    {
        if (!isBottom)
        {
            DiceRolled();
        }
        else
        {
            if (!isSetTargetRot)
            {
                if (m_rb.velocity.magnitude == 0 && bottomTime + chkTimeInterval <= Time.time)
                {
                    t_rot = Quaternion.Euler(RoundAngles(transform.eulerAngles.x), RoundAngles(transform.eulerAngles.y), RoundAngles(transform.eulerAngles.z));
                    isSetTargetRot = true;
                }
            }
            else if (transform.eulerAngles != t_rot.eulerAngles)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, t_rot, Time.deltaTime * 10);
            }
            else
            {
                isRotEnd = true;
            }
        }

        if (isRotEnd)
        {
            if (!isRollEnd)
            {
                if (m_rb.velocity.magnitude == 0 || Time.time > bottomTime + chkTimeInterval + 2.0f)
                {
                    m_DsystemManager.SumRollEnd();
                    isRollEnd = true;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("DiceBox_Bottom"))
        {
            bottomTime = Time.time;
            isBottom = true;
        }
        if(collision.collider.CompareTag("Dice"))
        {
            Vector3 vv = Vector3.zero;
            foreach(ContactPoint contact in collision.contacts)
            {
                vv += contact.point;
            }
            vv /= collision.contacts.Length;
            vv = transform.position - vv;

            m_rb.AddForce(vv * 30f);
        }
    }

    private float RoundAngles(float _value)
    {
        _value %= 360;
        if(Mathf.Abs(_value % 90) < 45)
        {
            if (_value > 0)
                return 0 + (90 * Mathf.FloorToInt(_value / 90));
            else
                return 0 + (90 * Mathf.CeilToInt(_value / 90));
        }
        else
        {
            if (_value > 0)
                return 90 + (90 * Mathf.FloorToInt(_value / 90));
            else
                return -90 + (90 * Mathf.CeilToInt(_value / 90));
        }
    }

    public void SetRes(int _num, Sprite _spr)
    {
        if (!isGetRes)
        {
            resNum = _num;
            resImg = _spr;
            isGetRes = true;
        }
    }

    public void SetDiceRollPower(float _rollpower, float r_minPowerRate)
    {
        rollPower = _rollpower;
        rollMinPowerRate = r_minPowerRate;

    }

    private void DiceRolled()
    {
        m_rb.AddTorque(Random.Range(rollPower * rollMinPowerRate, rollPower)
            , Random.Range(rollPower * rollMinPowerRate, rollPower)
            , Random.Range(rollPower * rollMinPowerRate, rollPower));
    }

    public void SetResValue()
    {
        m_DsystemManager.resValue += resNum;
        m_DsystemManager.SetResImg(resImg);
    }
}
