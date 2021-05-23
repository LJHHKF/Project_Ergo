using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class AutoStatusSceneManager : MonoBehaviour
{
    [Header("Object Registaration")]
    [SerializeField] private Text t_body;

    [Header("Status Setting")]
    [SerializeField] private int max_init_statCnt = 12;
    private int p_endu = 0;
    private int p_str = 0;
    private int p_solid = 0;
    private int p_int = 0;

    [Header("Text Setting")]
    [SerializeField] private float textUpSecond = 0.1f;
    [SerializeField] [TextArea] private string plainText;
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
        for(int i = 0; i < max_init_statCnt; i++)
        {
            int rand = Random.Range(0, 3);
            switch(rand)
            {
                case 0:
                    p_endu += 1;
                    break;
                case 1:
                    p_str += 1;
                    break;
                case 2:
                    p_solid += 1;
                    break;
                case 3:
                    p_int += 1;
                    break;
            }
        }
        fullText.Clear();
        fullText.Append(plainText);
        fullText.AppendLine($"\n체력: {CStatManager.instance.GetInitEndurance() + p_endu} 힘: {CStatManager.instance.GetInitStrength() + p_str} 내구: {CStatManager.instance.GetInitSolid() + p_solid} 마력: {CStatManager.instance.GetInitInteligent() + p_int}");

        curText.Clear();
        tempText.Clear();
        tempText_end.Clear();
        StoryTurningManager.instance.isTutorial = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTextEnded)
        {
            if(Input.GetMouseButtonDown(0))
            {
                t_body.text = fullText.ToString();
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
                                t_body.text = curText.ToString();
                                cnt_time = 0;
                            }
                        }
                    }
                    else
                    {
                        if (cnt_TextEffectStart > 0)
                        {
                            curText.Insert(cnt_text-1, tempText.ToString());
                            tempText.Clear();
                            t_body.text = curText.ToString();
                            cnt_time = 0;
                        }
                        else
                        {
                            curText.Append(tempText.ToString());
                            tempText.Clear();
                            t_body.text = curText.ToString();
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
            if(Input.GetMouseButtonDown(0))
            {
                SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.storyEnd);
                CStatManager.instance.SetStatChange_Init(p_endu, p_str, p_solid, p_int);
                LoadManager.instance.LoadStoryScene();
            }
        }
    }
}
