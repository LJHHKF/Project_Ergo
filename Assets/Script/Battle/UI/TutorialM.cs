using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialM : MonoBehaviour
{
    [SerializeField] private BattleUIManager uiManager;
    [SerializeField] private Sprite[] tutorialImgs;
    private Image m_Image;
    private int index;
    private int max;
    // Start is called before the first frame update
    void Start()
    {
        m_Image = GetComponent<Image>();
        m_Image.sprite = tutorialImgs[0];
        index = 1;
        max = tutorialImgs.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (index < max)
                m_Image.sprite = tutorialImgs[index++];
            else
                uiManager.TutorialEnd();
        }
    }
}
