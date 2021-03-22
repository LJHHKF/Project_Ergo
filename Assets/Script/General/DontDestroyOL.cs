using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOL : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
