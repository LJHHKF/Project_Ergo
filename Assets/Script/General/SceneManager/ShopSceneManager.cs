using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StoryTurningManager.instance.SetShopStage(true);
        //무언가 구매했으면 bought, 아니면 eyeShoping 플래그 세워야 함. 알아둘 것.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BtnConfirm()
    {
        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
        LoadManager.instance.LoadStoryScene();
    }
}
