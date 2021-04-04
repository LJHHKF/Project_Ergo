using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TrapSceneManager : MonoBehaviour
{
    [Serializable]
    private struct Trap
    {
        public string name;
        public int damage;
        [TextArea]public string description;
        public Sprite sprite;
        public int weight;
        public bool hadAddAbcond;
    }

    [Serializable]
    private struct AbCond_Trap
    {
        public int match_index;
        public int abcond_ID;
        public int damage;
    }
    [Header("Object Registration")]
    [SerializeField] private Text nameField;
    [SerializeField] private Text desciptionField;

    [Header("Trap Info Setting")]
    [SerializeField] private Trap[] traps;
    [SerializeField] private AbCond_Trap[] traps_abcond;
    private int trap_index;

    // Start is called before the first frame update
    void Start()
    {
        int full_weight = 0;
        for(int i = 0; i < traps.Length; i++)
        {
            full_weight += traps[i].weight;
        }

        int rand = UnityEngine.Random.Range(0, full_weight-1);
        full_weight = 0;
        for (int i = 0; i < traps.Length; i++)
        {
            full_weight += traps[i].weight;
            if(traps[i].weight >= full_weight - traps[i].weight && traps[i].weight < full_weight)
            {
                nameField.text = traps[i].name;
                desciptionField.text = traps[i].description;
                trap_index = i;
                break;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BtnConfirm()
    {
        if (traps[trap_index].hadAddAbcond)
        {
            for (int i = 0; i < traps_abcond.Length; i++)
            {
                if (traps_abcond[i].match_index == trap_index)
                {
                    CStatManager.instance.SetInheriteAbCond(traps_abcond[i].abcond_ID, traps_abcond[i].damage);
                    break;
                }
            }
        }
        CStatManager.instance.HealthPointUpdate(CStatManager.instance.health - traps[trap_index].damage);

        if (CStatManager.instance.health > 0)
            LoadManager.instance.LoadNextStage();
    }
}
