using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class UnitUI : MonoBehaviour
{
    [Header("UI Object Setting")]
    [SerializeField] private Image hpBar_img;
    [SerializeField] private TextMeshProUGUI hp_txt;
    [SerializeField] private GameObject guard_img_obj;
    private TextMeshProUGUI guard_txt;
    [SerializeField] private Image icon_EnemyActInfo;
    [SerializeField] private TextMeshProUGUI[] actPower_Text;
    [SerializeField] private float delayedAbsAlpha = 0.5f;
    [SerializeField] private Image[] icons_condition;
    private TextMeshProUGUI[] pileds_txt;
    private int[] ids;
    //private Sprite[] sprs_icon;
    private int[] nums_piled;
    private bool[] isDAbs;


    [Header("Ref Setting")]
    [SerializeField] private LivingEntity livTarget;
    [SerializeField] private AbCondition abcondTarget;

    // Start is called before the first frame update
    void Start()
    {
        guard_txt = guard_img_obj.transform.Find("GuardText").GetComponent<TextMeshProUGUI>();

        //if(icon_EnemyActInfo != null)
        //{
        //    actPower_Text = icon_EnemyActInfo.gameObject.transform.Find("PowerNumText").GetComponent<TextMeshProUGUI>();
        //}

        //sprs_icon = new Sprite[icons_condition.Length];
        ids = new int[icons_condition.Length];
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
        int hpCur = livTarget.health;
        int hpMax = livTarget.GetFullHealth();

        hp_txt.text = hpCur.ToString() + " / " + hpMax.ToString();
        hpBar_img.fillAmount = hpCur / hpMax;
    }

    public void GuardUpdate()
    {
        guard_txt.text = livTarget.GuardPoint.ToString();
    }

    public void AbConditionsUpdate()
    {
        int rs = abcondTarget.GetUIInfo(/*ref sprs_icon,*/ref ids, ref nums_piled, ref isDAbs, icons_condition.Length);

        if(rs == 0) // 없다
        {
            for (int i = 0; i < icons_condition.Length; i++)
            {
                icons_condition[i].gameObject.SetActive(false);
            }
        }
        else if(rs == -1) // 넘친다
        {
            for (int _i = 0; _i < icons_condition.Length; _i++)
            {
                int i = _i;
                icons_condition[i].gameObject.SetActive(true);
                icons_condition[i].sprite = AbCondInfoManager.instance.GetAbCond_Img(ids[i]);
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
                icons_condition[i].sprite = AbCondInfoManager.instance.GetAbCond_Img(ids[i]);
                    //sprs_icon[i];
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

    public void SetActInfo(Sprite _actSprite, int[] _powers, EnemyActType.AffectType[] _actTypes,int[] _repeat ,int _ActVariationNum)
    {
        for (int i = 0; i < actPower_Text.Length; i++)
            actPower_Text[i].gameObject.SetActive(false);

        icon_EnemyActInfo.sprite = _actSprite;
        StringBuilder sb = new StringBuilder(10);

        int max;
        if (_ActVariationNum < actPower_Text.Length)
            max = _ActVariationNum;
        else
            max = actPower_Text.Length;

        for (int i = 0; i < max; i++)
        {
            sb.Append(_powers[i].ToString());
            if (_repeat[i] > 1)
            {
                sb.Append(" *" + _repeat[i].ToString());
            }

            actPower_Text[i].gameObject.SetActive(true);
            actPower_Text[i].text = sb.ToString();

            sb.Clear();

            if (_actTypes[i] == EnemyActType.AffectType.Guard)
            {
                actPower_Text[i].color = new Color(0, 0, 255 / 255, 255 / 255);
            }
            else if (_actTypes[i] == EnemyActType.AffectType.Attack)
            {
                actPower_Text[i].color = new Color(255 / 255, 0, 0, 255 / 255);
            }
            else if (_actTypes[i] == EnemyActType.AffectType.CondAttack_Info)
            {
                actPower_Text[i].color = new Color(255 / 255, 0, 0, 128 / 255);
            }
            else if (_actTypes[i] == EnemyActType.AffectType.Abcond)
            {
                actPower_Text[i].color = new Color(0, 255 / 255, 0, 255 / 255);
            }
            else if(_actTypes[i] == EnemyActType.AffectType.CondAbcond_Info)
            {
                actPower_Text[i].color = new Color(0, 255 / 255, 0, 128 / 255);
            }
            else
            {
                actPower_Text[i].color = new Color(255 / 255, 255 / 255, 255 / 255, 255 / 255);
            }
        }
    }

    public void GetAbcondIDAndPiled(int _index, out int _id, out int _piled)
    {
        _id = ids[_index];
        _piled = nums_piled[_index];
    }
}
