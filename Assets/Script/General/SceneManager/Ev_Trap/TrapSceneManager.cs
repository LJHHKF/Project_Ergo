using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

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
    [SerializeField] private SpriteRenderer bg;
    //private Image bg;

    // 0: 화살, 1: 낙석, 2: 독늪, 3: 저주
    [Header("Trap Info Setting")]
    [SerializeField] private float textUpSecond = 0.1f;
    [SerializeField] private Trap[] traps;
    [SerializeField] private AbCond_Trap[] traps_abcond;
    private int trap_index;

    private StringBuilder fullText = new StringBuilder();
    private StringBuilder curText = new StringBuilder();
    private StringBuilder tempText = new StringBuilder();
    private StringBuilder tempText_end = new StringBuilder();
    private int cnt_TextEffectStart = 0;
    private bool isTextEnded = false;
    private int cnt_text = 0;
    private float cnt_time = 0;

    // Start is called before the first frame update
    void Start()
    {
        //bg = bg_object.GetComponent<Image>();
        fullText.Clear();
        curText.Clear();
        tempText.Clear();
        tempText_end.Clear();
        string key = $"SaveID({GameMaster.instance.GetSaveID()}).LastTrap";
        if (!PlayerPrefs.HasKey(key) || PlayerPrefs.GetInt(key) <= -1)
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
                if (rand >= full_weight - traps[i].weight && rand < full_weight)
                {
                    nameField.text = traps[i].name;
                    fullText.Append(traps[i].description);
                    bg.sprite = traps[i].sprite;
                    trap_index = i;
                    PlayerPrefs.SetInt(key, i);
                    if (i == 0)
                        BGMManager.instance.EFfectBGM_trap(0);
                    else if (i == 1)
                        BGMManager.instance.EFfectBGM_trap(1);
                    break;
                }
            }
        }
        else
        {
            trap_index = PlayerPrefs.GetInt(key);
            nameField.text = traps[trap_index].name;
            fullText.Append(traps[trap_index].description);
        }

        GameMaster.instance.OnStageStart();
    }

    private void Update()
    {
        if (!isTextEnded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                desciptionField.text = fullText.ToString();
                isTextEnded = true;
            }
            else if (cnt_text < fullText.Length)
            {
                cnt_time += Time.deltaTime;
                if (cnt_time / textUpSecond >= 1)
                {
                    tempText.Append(fullText.ToString(cnt_text++, 1));
                    if (tempText.ToString(0, 1) == "<")
                    {
                        if (tempText.ToString(tempText.Length - 1, 1) == ">")
                        {
                            if (cnt_TextEffectStart > 0)
                            {
                                cnt_TextEffectStart -= 1;
                                tempText.Clear();
                            }
                            else
                            {
                                if (tempText.Length > 5)
                                {
                                    if (tempText.ToString(1, 5) == "color")
                                        tempText_end.Append("</color>");
                                    else
                                    {
                                        tempText_end.Append(tempText.ToString());
                                        tempText_end.Insert(1, "/");
                                    }
                                }
                                else
                                {
                                    tempText_end.Append(tempText.ToString());
                                    tempText_end.Insert(1, "/");
                                }
                                cnt_TextEffectStart += 1;
                                curText.Append(tempText.ToString());
                                curText.Append(tempText_end.ToString());
                                tempText.Clear();
                                tempText_end.Clear();
                                desciptionField.text = curText.ToString();
                                cnt_time = 0;
                            }
                        }
                    }
                    else
                    {
                        if (cnt_TextEffectStart > 0)
                        {
                            curText.Insert(cnt_text - 1, tempText.ToString());
                            tempText.Clear();
                            desciptionField.text = curText.ToString();
                            cnt_time = 0;
                        }
                        else
                        {
                            curText.Append(tempText.ToString());
                            tempText.Clear();
                            desciptionField.text = curText.ToString();
                            cnt_time = 0;
                        }
                    }
                }
            }
            else
            {
                isTextEnded = true;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
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
                PlayerPrefs.DeleteKey(key.ToString());

                if (CStatManager.instance.health > 0)
                    LoadManager.instance.LoadNextStage();
            }
        }
    }
}
