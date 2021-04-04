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
        string key = $"SaveID({GameMaster.instance.GetSaveID()}).LastTrap";
        if (!PlayerPrefs.HasKey(key) || PlayerPrefs.GetInt(key) == -1)
        {
            int full_weight = 0;
            for (int i = 0; i < traps.Length; i++)
            {
                full_weight += traps[i].weight;
            }

            int rand = UnityEngine.Random.Range(0, full_weight - 1);
            full_weight = 0;
            for (int i = 0; i < traps.Length; i++)
            {
                full_weight += traps[i].weight;
                if (traps[i].weight >= full_weight - traps[i].weight && traps[i].weight < full_weight)
                {
                    nameField.text = traps[i].name;
                    desciptionField.text = traps[i].description;
                    trap_index = i;
                    PlayerPrefs.SetInt(key, i);
                    break;
                }
            }
        }
        else
        {
            trap_index = PlayerPrefs.GetInt(key);
            nameField.text = traps[trap_index].name;
            desciptionField.text = traps[trap_index].description;
        }
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

        string key = $"SaveID({GameMaster.instance.GetSaveID()}).LastTrap";
        PlayerPrefs.SetInt(key, -1);

        if (CStatManager.instance.health > 0)
            LoadManager.instance.LoadNextStage();
    }
}
