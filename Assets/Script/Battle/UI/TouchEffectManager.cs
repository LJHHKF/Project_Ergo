using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffectManager : MonoBehaviour
{
    [SerializeField] private GameObject touch_Prefab;
    private List<GameObject> t_list = new List<GameObject>();
    private int cnt_unactive = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SetTouchEffect(Input.mousePosition);
        }
    }

    private void SetTouchEffect(Vector3 _camPos)
    {
        GameObject _target = null;
        if(cnt_unactive > 0)
        {
            for(int i = 0; i < t_list.Count; i++)
            {
                int _i = i;
                if(!t_list[_i].activeSelf)
                {
                    t_list[_i].SetActive(true);
                    _target = t_list[_i];
                    cnt_unactive -= 1;
                    break;
                }
            }
        }
        else
            _target = CreateTouchEffect();

        _target.GetComponent<RectTransform>().position = _camPos;
        StartCoroutine(DelayedUnActive(_target));

        GameObject CreateTouchEffect()
        {
            GameObject ef = Instantiate(touch_Prefab, gameObject.transform);
            ef.SetActive(true);
            t_list.Add(ef);
            return ef;
        }
    }

    IEnumerator DelayedUnActive(GameObject _t)
    {
        yield return new WaitForSeconds(1.0f);
        _t.SetActive(false);
        cnt_unactive += 1;
        yield break;
    }
}
