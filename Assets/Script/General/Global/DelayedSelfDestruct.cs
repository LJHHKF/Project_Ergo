using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedSelfDestruct : MonoBehaviour
{
    [SerializeField] private float time = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedDestruct());
    }

    IEnumerator DelayedDestruct()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        yield break;
    }
}
