using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private float delayTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedNextScene());
    }

    IEnumerator DelayedNextScene()
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene("Entrance");
        yield break;
    }
}
