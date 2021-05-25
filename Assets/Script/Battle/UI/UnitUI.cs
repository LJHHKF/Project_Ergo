using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class UnitUI : MonoBehaviour
{
    [Header("Basic Setting")]
    [SerializeField] private Image hpBar_img;
    [SerializeField] private TextMeshProUGUI hp_txt;
    [SerializeField] private GameObject guard_img_obj;
    [SerializeField] private GameObject guard_gainAnim;
    [SerializeField] private GameObject guard_breakAnim;
    [SerializeField] private Image icon_EnemyActInfo;
    [SerializeField] private TextMeshProUGUI[] actPower_Text;
    [SerializeField] private float delayedAbsAlpha = 0.5f;
    [SerializeField] private Image[] icons_condition;
    private TextMeshProUGUI guard_txt;
    private TextMeshProUGUI[] pileds_txt;
    private int[] ids;
    //private Sprite[] sprs_icon;
    private int[] nums_piled;
    private bool[] isDAbs;

    [Header("PopUpText Setting")]
    [SerializeField] private GameObject popUpText_Prefab;
    [SerializeField] private float upSpeed = 1.0f;
    [SerializeField] private float endTime = 3.0f;
    [SerializeField] private float interval_pop = 0.5f;
    [SerializeField] private Color color_Damage;
    [SerializeField] private Color color_GuardedDamage;
    [SerializeField] private Color color_GetGuardPoint;
    [SerializeField] private Color color_Buff;
    [SerializeField] private Color color_Debuff;
    [SerializeField] private Color color_ActionName;
    [SerializeField] private Color color_RestoreHealth;
    private List<GameObject> list_pop = new List<GameObject>();
    private int cnt_popUpActive = 0;

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
        guard_img_obj.SetActive(false);
        guard_gainAnim.SetActive(false);
        guard_breakAnim.SetActive(false);
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

    public void SetActInfo(Sprite _actSprite, int[] _powers, EnemyActType.AffectType[] _actTypes,int _ActVariationNum)
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

    private void AddPopUpList(GameObject _add)
    {
        if(cnt_popUpActive > 0)
        {
            for(int i = 0; i < list_pop.Count; i++)
            {
                int _i = i;
                if(list_pop[_i].activeSelf)
                {
                    Vector3 interval = new Vector3(0, interval_pop, 0);
                    list_pop[_i].transform.position += interval;
                }
            }
            _add.transform.localPosition = Vector3.zero;
            StartCoroutine(PopUpMove(_add));
            cnt_popUpActive += 1;
        }
        else
        {
            _add.gameObject.transform.localPosition = Vector3.zero;
            StartCoroutine(PopUpMove(_add));
            cnt_popUpActive += 1;
        }
    }

    private TextMeshProUGUI CreatePopUpText()
    {
        if(list_pop.Count == 0 || list_pop.Count == cnt_popUpActive)
        {
            return Create();
        }
        else
        {
            for(int i = 0; i < list_pop.Count; i++)
            {
                int _i = i;
                if(!list_pop[_i].gameObject.activeSelf)
                {
                    list_pop[_i].gameObject.SetActive(true);
                    list_pop[_i].gameObject.transform.localPosition = Vector3.zero;
                    return list_pop[_i].GetComponent<TextMeshProUGUI>();
                }
            }
        }
        return Create();
        TextMeshProUGUI Create()
        {
            GameObject _t = Instantiate(popUpText_Prefab, gameObject.transform);
            list_pop.Add(_t);
            return _t.GetComponent<TextMeshProUGUI>();
        }
    }

    IEnumerator PopUpMove(GameObject _target)
    {
        float startTime = Time.time;
        while (startTime + endTime >= Time.time)
        {
            yield return null;
            _target.transform.Translate(Vector3.up * upSpeed * Time.deltaTime);
        }
        _target.SetActive(false);
        cnt_popUpActive -= 1;
        yield break;
    }

    public void AddPopUpText_Damage(int _dmg)
    {
        TextMeshProUGUI _t = CreatePopUpText();
        _t.text = _dmg.ToString();
        _t.color = color_Damage;
        AddPopUpList(_t.gameObject);
    }

    public void AddPopUpText_GuardedDamage(int _dmg)
    {
        TextMeshProUGUI _t = CreatePopUpText();
        _t.text = _dmg.ToString();
        _t.color = color_GuardedDamage;
        AddPopUpList(_t.gameObject);
    }
    
    public void AddPopUpText_GetGuardPoint(int _value)
    {
        TextMeshProUGUI _t = CreatePopUpText();
        _t.text = _value.ToString();
        _t.color = color_GetGuardPoint;
        AddPopUpList(_t.gameObject);
    }

    public void AddPopUpText_Buff(string _name, int _piled)
    {
        TextMeshProUGUI _t = CreatePopUpText();
        _t.text = $"{_name} X {_piled}";
        _t.color = color_Buff;
        AddPopUpList(_t.gameObject);
    }

    public void AddPopUpText_Debuff(string _name, int _piled)
    {
        TextMeshProUGUI _t = CreatePopUpText();
        _t.text = $"{_name} X {_piled}";
        _t.color = color_Debuff;
        AddPopUpList(_t.gameObject);
    }

    public void AddPopUpText_ActionName(string _name)
    {
        TextMeshProUGUI _t = CreatePopUpText();
        _t.text = _name;
        _t.color = color_ActionName;
        AddPopUpList(_t.gameObject);
    }

    public void AddPopUpText_RestoreHealth(int _value)
    {
        TextMeshProUGUI _t = CreatePopUpText();
        _t.text = _value.ToString();
        _t.color = color_RestoreHealth;
        AddPopUpList(_t.gameObject);
    }

    public void GuardGainAnim()
    {
        if (!guard_img_obj.activeSelf)
        {
            guard_gainAnim.SetActive(true);
            StartCoroutine(DelayedUnActive(guard_gainAnim, 1.0f));
            StartCoroutine(DelayedActive(guard_img_obj, 1.0f));
        }
    }

    public void GuardBreakAnim()
    {
        guard_breakAnim.SetActive(true);
        guard_img_obj.SetActive(false);
        StartCoroutine(DelayedUnActive(guard_breakAnim, 1.0f));
    }

    public void UnActiveGuardImg()
    {
        guard_img_obj.SetActive(false);
    }

    IEnumerator DelayedUnActive(GameObject _t, float _sec)
    {
        yield return new WaitForSeconds(_sec);
        _t.SetActive(false);
        yield break;
    }

    IEnumerator DelayedActive(GameObject _t, float _sec)
    {
        yield return new WaitForSeconds(_sec);
        _t.SetActive(true);
        yield break;
    }
}
