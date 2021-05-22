using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class ICardEffectM : MonoBehaviour
{
    public abstract void OnEffect();

    protected IEnumerator DelayedInvoke(Action _action)
    {
        yield return new WaitForSeconds(1.0f);
        _action.Invoke();
        yield break;
    }
}
