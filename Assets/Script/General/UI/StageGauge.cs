﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageGauge : MonoBehaviour
{
    [SerializeField] private Image gaugeImg;
    private float cur;
    private float max;
    // Start is called before the first frame update
    void Start()
    {
        cur = StageManager.instance.curStage;
        max = StageManager.instance.GetCurStageMax();

        gaugeImg.fillAmount = cur / max;
    }
}
