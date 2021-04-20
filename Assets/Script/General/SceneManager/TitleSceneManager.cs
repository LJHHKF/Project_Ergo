using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private float delayTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedNextScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DelayedNextScene()
    {
        yield return new WaitForSeconds(delayTime);
        LoadingSceneManager.LoadScene("Entrance");
        yield break;
    }
}
