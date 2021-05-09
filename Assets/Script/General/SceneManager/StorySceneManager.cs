using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class StorySceneManager : MonoBehaviour
{
    [Header("Object Registration")]
    [SerializeField] private Text t_body;

    [Header("Text Setting_common")]
    [SerializeField] private float textUpSecond = 0.1f;
    private StringBuilder fullText = new StringBuilder();
    private StringBuilder curText = new StringBuilder();
    private StringBuilder tempText = new StringBuilder();
    private StringBuilder tempText_end = new StringBuilder();
    private int cnt_TextEffectStart = 0;
    private bool isTextEnded = false;
    private int cnt_text = 0;
    private float cnt_time = 0;

    [Header("Text Setting_Special")]
    [SerializeField] [TextArea] private string txt_Tuto;
    [SerializeField] [TextArea] private string txt_Aries_elite;
    [SerializeField] [TextArea] private string txt_Aries_boss;
    [SerializeField] [TextArea] private string txt_Creta_elite;
    [SerializeField] [TextArea] private string txt_Creta_boss;
    [SerializeField] [TextArea] private string txt_JunkGoblin_elite;
    [SerializeField] [TextArea] private string txt_JunkGoblin_boss;
    [SerializeField] [TextArea] private string txt_ChapterEnd_1;

    [Header("Text Setting_Plain")]
    [SerializeField] [TextArea] private string txt_Battle_Perfect;
    [SerializeField] [TextArea] private string txt_Battle_Welldone;
    [SerializeField] [TextArea] private string txt_Battle_HardWin;
    [SerializeField] [TextArea] private string txt_Shop_Bought;
    [SerializeField] [TextArea] private string txt_Shop_Eyeshoping;
    [SerializeField] [TextArea] private string txt_Rest_rest;
    [SerializeField] [TextArea] private string txt_Rest_deleteCard;


    // Start is called before the first frame update
    void Start()
    {
        fullText.Clear();
        curText.Clear();
        tempText.Clear();
        tempText_end.Clear();
        if(StoryTurningManager.instance.isTutorial)
        {
            fullText.Append(txt_Tuto);
        }
        else if(StoryTurningManager.instance.isShopStage)
        {
            if (StoryTurningManager.instance.isShop_Bought)
                fullText.Append(txt_Shop_Bought);
            else
                fullText.Append(txt_Shop_Eyeshoping);
            StoryTurningManager.instance.SetShopStage(false);
        }
        else if(StoryTurningManager.instance.isRestStage)
        {
            if (StoryTurningManager.instance.isRest_Rest)
                fullText.Append(txt_Rest_rest);
            else if (StoryTurningManager.instance.isRest_CDelete)
                fullText.Append(txt_Rest_deleteCard);
            StoryTurningManager.instance.SetRestStage(false);
        }
        else if(StoryTurningManager.instance.isEliteStage)
        {
            switch(StoryTurningManager.instance.index_Elite)
            {
                case 0:
                    fullText.Append(txt_Aries_elite);
                    break;
                case 1:
                    fullText.Append(txt_Creta_elite);
                    break;
                case 2:
                    fullText.Append(txt_JunkGoblin_elite);
                    break;
            }
            StoryTurningManager.instance.SetEliteStage(false);
            fullText.AppendLine();
            AppendBattleText();
        }
        else if(StoryTurningManager.instance.isBossStage)
        {
            switch(StoryTurningManager.instance.index_Boss)
            {
                case 0:
                    fullText.Append(txt_Aries_boss);
                    break;
                case 1:
                    fullText.Append(txt_Creta_boss);
                    break;
                case 2:
                    fullText.Append(txt_JunkGoblin_boss);
                    break;
            }
            StoryTurningManager.instance.SetBossStage(false);
            fullText.AppendLine();
            AppendBattleText();
        }
        else if(StoryTurningManager.instance.isChapterEnd_1)
        {
            fullText.Append(txt_ChapterEnd_1);
        }
        else
        {
            AppendBattleText();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTextEnded)
        {
            if (Input.GetMouseButtonDown(0))
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
            if (Input.GetMouseButtonDown(0))
            {
                if (StoryTurningManager.instance.isTutorial)
                {
                    StoryTurningManager.instance.isTutorial = false;
                    LoadManager.instance.LoadFirst_Init();
                }
                else
                    LoadManager.instance.LoadNextStage();
            }
        }
    }

    private void AppendBattleText()
    {
        if (StoryTurningManager.instance.battleDamage == 0)
            fullText.Append(txt_Battle_Perfect);
        else if (StoryTurningManager.instance.battleDamage < 20)
            fullText.Append(txt_Battle_Welldone);
        else
            fullText.Append(txt_Battle_HardWin);
    }
}
