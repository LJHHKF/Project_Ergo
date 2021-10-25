using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCheckerManager : MonoBehaviour
{
    [SerializeField] private float bottomValue = -2.5f;
    //[SerializeField] private float startYValue = 2.5f;
    [SerializeField] private float moveSpeed;
    //[SerializeField] private DiceSystemManager m_DsysetmManager;
    [SerializeField] private Transform t_dBox_Top;

    private void OnEnable()
    {
        gameObject.transform.position = t_dBox_Top.position;
            //new Vector3(gameObject.transform.position.x, startYValue, gameObject.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y <= bottomValue)
        {
            gameObject.transform.Translate(0, moveSpeed * Time.deltaTime, 0);
        }
    }
}
