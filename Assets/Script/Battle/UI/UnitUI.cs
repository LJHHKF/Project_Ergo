using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUI : MonoBehaviour
{
    [Header("UI Object Setting")]
    [SerializeField] private Image hpBar_img;
    private TextMeshProUGUI hp_txt;
    [SerializeField] private GameObject guard_img_obj;
    private TextMeshProUGUI guard_txt;
    [SerializeField] private Image icon_EnemyActInfo;
    private TextMeshProUGUI actPower_Text;
    [SerializeField] private float delayedAbsAlpha = 0.5f;
    [SerializeField] private Image[] icons_condition;
    private TextMeshProUGUI[] pileds_txt;
    private Sprite[] sprs_icon;
    private int[] nums_piled;
    private bool[] isDAbs;


    [Header("Ref Setting")]
    [SerializeField] private LivingEntity livTarget;
    [SerializeField] private AbCondition abcondTarget;

    // Start is called before the first frame update
    void Start()
    {
        hp_txt = hpBar_img.transform.Find("HPText").GetComponent<TextMeshProUGUI>();
        guard_txt = guard_img_obj.transform.Find("GuardText").GetComponent<TextMeshProUGUI>();

        if(icon_EnemyActInfo != null)
        {
            actPower_Text = icon_EnemyActInfo.gameObject.transform.Find("PowerNumText").GetComponent<TextMeshProUGUI>();
        }

        sprs_icon = new Sprite[icons_condition.Length];
        nums_piled = new int[icons_condition.Length];
        isDAbs = new bool[icons_condition.Length];
        pileds_txt = new TextMeshProUGUI[icons_condition.Length];
        for(int i = 0; i < icons_condition.Length; i++)
        {
            pileds_txt[i] = icons_condition[i].transform.Find("PiledNumText").GetComponent<TextMeshProUGUI>();
            icons_condition[i].gameObject.SetActive(false);
        }

    }

    public void HpUpdate()
    {
        float hpCur = livTarget.health;
        float hpMax = livTarget.GetFullHealth();

        hp_txt.text = hpCur.ToString() + " / " + hpMax.ToString();
        hpBar_img.fillAmount = hpCur / hpMax;
    }

    public void GuardUpdate()
    {
        guard_txt.text = livTarget.GuardPoint.ToString();
    }

    public void AbConditionsUpdate()
    {
        int rs = abcondTarget.GetUIInfo(ref sprs_icon, ref nums_piled, ref isDAbs, icons_condition.Length);

        if(rs == 0) // 없다
        {
            for (int i = 0; i < icons_condition.Length; i++)
                icons_condition[i].gameObject.SetActive(false);
        }
        else if(rs == -1) // 넘친다
        {
            for (int _i = 0; _i < icons_condition.Length; _i++)
            {
                int i = _i;
                icons_condition[i].gameObject.SetActive(true);
                icons_condition[i].sprite = sprs_icon[i];
                pileds_txt[_i].text = (nums_piled[i] + 1).ToString();
                if(isDAbs[i])
                {
                    icons_condition[_i].color = new Color(255 / 255, 255 / 255, 255 / 255, (255 * delayedAbsAlpha) / 255);
                }
                else
                {
                    icons_condition[_i].color = new Color(255 / 255, 255 / 255, 255 / 255, 255 / 255);
                }
            }
        }
        else
        {
            for(int _i = 0; _i < rs; _i++)
            {
                int i = _i;
                icons_condition[i].gameObject.SetActive(true);
                icons_condition[i].sprite = sprs_icon[i];
                pileds_txt[i].text = (nums_piled[i] + 1).ToString();
                if (isDAbs[i])
                {
                    icons_condition[i].color = new Color(255 / 255, 255 / 255, 255 / 255, (255 * delayedAbsAlpha) / 255);
                }
                else
                {
                    icons_condition[i].color = new Color(255 / 255, 255 / 255, 255 / 255, 255 / 255);
                }
            }
        }
    }

    public void SetActInfo(Sprite _actSprite, int _power, int _actTypeNum)
    {
        icon_EnemyActInfo.sprite = _actSprite;
        actPower_Text.text = _power.ToString();

        if(_actTypeNum == 1)
        {
            actPower_Text.color = new Color(0, 0, 255 / 255, 255/255);
        }
        else
        {
            actPower_Text.color = new Color(255 / 255, 0, 0, 255/255);
        }
    }
}
