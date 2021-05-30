using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class StorySceneManager : MonoBehaviour
{
    [Header("Object Registration")]
    [SerializeField] private GameObject textBox;
    [SerializeField] private Text t_body;
    [SerializeField] private GameObject diedObjects;
    [SerializeField] private Text t_body_died;
    [SerializeField] private SettingWindowM settingWindowManager;
    [SerializeField] private Transform max_y_pos;

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
    private bool isPointerIn = false;
    private bool isSetWinOpen = false;

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
    [SerializeField] [TextArea] private string txt_Died;
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
        isSetWinOpen = false;
        settingWindowManager.enable += () => isSetWinOpen = true;
        settingWindowManager.disable += () => isSetWinOpen = false;

        if (StoryTurningManager.instance.isDied)
        {
            textBox.SetActive(false);
            diedObjects.SetActive(true);
            isTextEnded = true;
            t_body_died.text = txt_Died;
        }
        else
        {
            textBox.SetActive(true);
            diedObjects.SetActive(false);
            isTextEnded = false;
            fullText.Clear();
            curText.Clear();
            tempText.Clear();
            tempText_end.Clear();
            if (StoryTurningManager.instance.isTutorial)
            {
                fullText.Append(txt_Tuto);
            }
            else if (StoryTurningManager.instance.isShopStage)
            {
                if (StoryTurningManager.instance.isShop_Bought)
                    fullText.Append(txt_Shop_Bought);
                else
                    fullText.Append(txt_Shop_Eyeshoping);
                StoryTurningManager.instance.SetShopStage(false);
            }
            else if (StoryTurningManager.instance.isRestStage)
            {
                if (StoryTurningManager.instance.isRest_Rest)
                    fullText.Append(txt_Rest_rest);
                else if (StoryTurningManager.instance.isRest_CDelete)
                    fullText.Append(txt_Rest_deleteCard);
                StoryTurningManager.instance.SetRestStage(false);
            }
            else if (StoryTurningManager.instance.isEliteStage)
            {
                switch (StoryTurningManager.instance.index_Elite)
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
            else if (StoryTurningManager.instance.isBossStage)
            {
                switch (StoryTurningManager.instance.index_Boss)
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
            else if (StoryTurningManager.instance.isChapterEnd_1)
            {
                fullText.Append(txt_ChapterEnd_1);
            }
            else
            {
                AppendBattleText();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSetWinOpen)
        {
            if (!isTextEnded)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (Input.mousePosition.y < max_y_pos.position.y)
                    {
                        t_body.text = fullText.ToString();
                        isTextEnded = true;
                    }
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
                                curText.Insert(cnt_text - 1, tempText.ToString());
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
                if (!isPointerIn)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (Input.mousePosition.y < max_y_pos.position.y)
                        {
                            SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.storyEnd);
                            if (StoryTurningManager.instance.isTutorial)
                            {
                                //StoryTurningManager.instance.isTutorial = false; // 전투씬에서 듀토리얼 이미지를 띄워주므로, 거기서 false로 바꾸도록 수정함.
                                LoadManager.instance.LoadFirst_Init();
                            }
                            else
                                LoadManager.instance.LoadNextStage();
                        }
                    }
                }
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

    public void PointerIn()
    {
        isPointerIn = true;
    }
    public void PointerOut()
    {
        isPointerIn = false;
    }
    public void PointerClicked(string _SceneName)
    {
        if (_SceneName == "Battle")
            LoadManager.instance.ChangeNextStage(0);
        else if (_SceneName == "Ev_Shop")
            LoadManager.instance.ChangeNextStage(1);
        else if (_SceneName == "Ev_Rest")
            LoadManager.instance.ChangeNextStage(2);
        else if (_SceneName == "Ev_Trap")
            LoadManager.instance.ChangeNextStage(3);

        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.storyEnd);
        StoryTurningManager.instance.ResetTriggers();

        if (StoryTurningManager.instance.isTutorial)
            LoadManager.instance.ChangeNextStage_InitStart();
        else
            LoadManager.instance.LoadNextStage();
    }
}
