using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaButton : MonoBehaviour
{
    [SerializeField] private float AlphaThreshold = 0.1f;

    void Start()
    {
        gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;
    }
}
