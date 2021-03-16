using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUF_Repeat : MonoBehaviour
{
    public Card_Base myCard;
    public int maxNum = 2;
    public CUF_Base repeatTarget;
    public float timeInterval = 1.0f;

    // Start is called before the first frame update
    public void Start()
    {
        for(int i = 1; i < maxNum; i++)
        {
            myCard.sub_use += () => StartCoroutine(DelayedUse());
        }
    }

    IEnumerator DelayedUse()
    {
        Debug.Log("반복 사용중, 대기시간:" + timeInterval + "초");
        yield return new WaitForSeconds(timeInterval);
        repeatTarget.Use();
        yield break;
    }
}
