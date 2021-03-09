using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCheckerManager : MonoBehaviour
{
    public float bottomValue = -5f;
    public float startYvalue = 2.5f;
    public float moveSpeed = -15.0f;
    public DiceSystemManager m_DsysetmManager;

    private void OnEnable()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, startYvalue, gameObject.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y >= bottomValue)
        {
            gameObject.transform.Translate(0, moveSpeed * Time.deltaTime, 0);
        }
        else if (gameObject.activeSelf)
        {
            m_DsysetmManager.CheckEnd();
            gameObject.SetActive(false);
        }
    }
}
