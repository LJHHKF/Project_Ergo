using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusSceneManager : MonoBehaviour
{
    [Header("Object Registration")]
    [SerializeField] private Text txt_remain;
    [SerializeField] private Text txt_endu;
    [SerializeField] private Text txt_str;
    [SerializeField] private Text txt_solid;
    [SerializeField] private Text txt_int;
    [SerializeField] private GameObject[] errorEffects;
    [SerializeField] private GameObject warningWindow;
    [Header("Base Setting")]
    [SerializeField] private int max_remain;

    private int cur_remain;
    private int add_endu = 0;
    private int add_str = 0;
    private int add_solid = 0;
    private int add_int = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        cur_remain = max_remain;
        txt_remain.text = cur_remain.ToString();

        txt_endu.text = CStatManager.instance.endurance.ToString();
        txt_str.text = CStatManager.instance.strength.ToString();
        txt_solid.text = CStatManager.instance.solid.ToString();
        txt_int.text = CStatManager.instance.intelligent.ToString();

        for (int i = 0; i < errorEffects.Length; i++)
            errorEffects[i].SetActive(false);
        warningWindow.SetActive(false);
    }

    public void OnBtnApply()
    {
        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
        if (cur_remain > 0)
        {
            warningWindow.SetActive(true);
        }
        else
        {
            CStatManager.instance.SetStatChange_Init(add_endu, add_str, add_solid, add_int);
            LoadManager.instance.LoadFirst_Init();
        }
    }

    public void OnBtnRevert()
    {
        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
        cur_remain = max_remain;
        add_endu = 0;
        add_str = 0;
        add_solid = 0;
        add_int = 0;

        txt_remain.text = cur_remain.ToString();
        txt_endu.text = CStatManager.instance.endurance.ToString();
        txt_str.text = CStatManager.instance.strength.ToString();
        txt_solid.text = CStatManager.instance.solid.ToString();
        txt_int.text = CStatManager.instance.intelligent.ToString();
    }

    public void OnBtnWarningYes()
    {
        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
        CStatManager.instance.SetStatChange_Init(add_endu, add_str, add_solid, add_int);
        LoadManager.instance.LoadFirst_Init();
    }

    public void OnBtnWarningNo()
    {
        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
        warningWindow.SetActive(false);
    }

    public void OnBtnEnduranceUp()
    {
        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
        if (cur_remain > 0)
        {
            cur_remain -= 1;
            add_endu += 1;

            txt_remain.text = cur_remain.ToString();
            txt_endu.text = (CStatManager.instance.endurance + add_endu).ToString();
        }
        else
        {
            ActiveErrorEffect(0);
            ActiveErrorEffect(1);
        }
    }

    public void OnBtnStrengthUp()
    {
        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
        if (cur_remain > 0)
        {
            cur_remain -= 1;
            add_str += 1;

            txt_remain.text = cur_remain.ToString();
            txt_str.text = (CStatManager.instance.strength + add_str).ToString();
        }
        else
        {
            ActiveErrorEffect(0);
            ActiveErrorEffect(2);
        }
    }

    public void OnBtnSolidUp()
    {
        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
        if (cur_remain > 0)
        {
            cur_remain -= 1;
            add_solid += 1;

            txt_remain.text = cur_remain.ToString();
            txt_solid.text = (CStatManager.instance.solid + add_solid).ToString();
        }
        else
        {
            ActiveErrorEffect(0);
            ActiveErrorEffect(3);
        }
    }

    public void OnBtnIntUp()
    {
        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
        if (cur_remain > 0)
        {
            cur_remain -= 1;
            add_int += 1;

            txt_remain.text = cur_remain.ToString();
            txt_int.text = (CStatManager.instance.intelligent + add_int).ToString();
        }
        else
        {
            ActiveErrorEffect(0);
            ActiveErrorEffect(4);
        }
    }

    private void ActiveErrorEffect(int _id)
    {
        errorEffects[_id].SetActive(true);
        StartCoroutine(DelayedUnActive(errorEffects[_id]));
    }

    IEnumerator DelayedUnActive(GameObject _target)
    {
        yield return new WaitForSeconds(1.0f);
        _target.SetActive(false);
        yield break;
    }
}
