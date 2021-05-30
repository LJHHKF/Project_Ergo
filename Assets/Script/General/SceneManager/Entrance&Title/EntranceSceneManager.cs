using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceSceneManager : MonoBehaviour
{
    //버튼에 등록되어 있음.
    public void GameStart_Continue(int ID)
    {
        GameMaster.instance.GameStart(ID, false);
    }

    public void GameStart_New(int ID)
    {
        GameMaster.instance.GameStart(ID, true);
    }

    public void GameEnd()
    {
        GameMaster.instance.OnGameStop();
        Application.Quit();
    }
}
