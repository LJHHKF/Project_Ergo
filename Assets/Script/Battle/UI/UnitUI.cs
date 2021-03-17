using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUI : MonoBehaviour
{
    [Header("UI Object Setting")]
    public Image hpBar_img;
    private TextMeshProUGUI hp_txt;
    public GameObject guard_img_obj;
    private TextMeshProUGUI guard_txt;
    public Image[] icons_condition;
    private TextMeshProUGUI[] pileds_txt;
    private Sprite[] sprs_icon;
    private int[] nums_piled;

    [Header("Ref Setting")]
    public LivingEntity livTarget;
    public AbCondition abcondTarget;

    // Start is called before the first frame update
    void Start()
    {
        hp_txt = hpBar_img.transform.Find("HPText").GetComponent<TextMeshProUGUI>();
        guard_txt = guard_img_obj.transform.Find("GuardText").GetComponent<TextMeshProUGUI>();

        sprs_icon = new Sprite[icons_condition.Length];
        nums_piled = new int[icons_condition.Length];
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
        int rs = abcondTarget.GetUIInfo(ref sprs_icon, ref nums_piled, icons_condition.Length);

        if(rs == 0)
        {
            for (int i = 0; i < icons_condition.Length; i++)
                icons_condition[i].gameObject.SetActive(false);
        }
        else if(rs == -1)
        {
            for (int i = 0; i < icons_condition.Length; i++)
            {
                icons_condition[i].gameObject.SetActive(true);
                icons_condition[i].sprite = sprs_icon[i];
                pileds_txt[i].text = nums_piled[i].ToString();
            }
        }
        else
        {
            for(int i = 0; i < rs; i++)
            {
                icons_condition[i].gameObject.SetActive(true);
                icons_condition[i].sprite = sprs_icon[i];
                pileds_txt[i].text = nums_piled[i].ToString();
            }
        }
    }
}
