using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerOff : MonoBehaviour
{
    [SerializeField] private IAbleEvent target;
    private EventTrigger trigger;

    // Start is called before the first frame update
    void Start()
    {
        trigger = GetComponent<EventTrigger>();
        target.enable += () => trigger.enabled = false;
        target.disable += () => trigger.enabled = true;
    }
}
