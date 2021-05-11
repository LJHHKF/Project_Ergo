using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaking : MonoBehaviour
{
    //카메라에 설치할 것
    [SerializeField] private float shakePower = 0.05f;
    [SerializeField] private float shakeTime = 1f;
    private Vector3 initPos;
    // Start is called before the first frame update
    void Start()
    {
        initPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeTime > 0)
        {
            Vector3 shakePos = Random.insideUnitCircle * shakePower;

            transform.position = initPos + shakePos;
            shakeTime -= Time.deltaTime;
        }
        else
        {
            transform.position = initPos;
        }
    }

    public void SetShakeTime(float _sec)
    {
        shakeTime = _sec;
    }

    public void SetShakePower(float _power)
    {
        shakePower = _power;
    }
}
