using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IAbleEvent : MonoBehaviour
{
    public event Action enable;
    public event Action disable;

    protected virtual void OnEnable()
    {
        enable?.Invoke();
    }

    protected virtual void OnDisable()
    {
        disable?.Invoke();
    }
}
