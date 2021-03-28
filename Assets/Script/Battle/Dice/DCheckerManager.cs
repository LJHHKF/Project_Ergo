using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCheckerManager : MonoBehaviour
{
    [SerializeField] private float bottomValue = -5f;
    [SerializeField] private float startZValue = -10f;
    [SerializeField] private float moveSpeed = 30.0f;
    [SerializeField] private DiceSystemManager m_DsysetmManager;

    private void OnEnable()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, startZValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.z <= bottomValue)
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
