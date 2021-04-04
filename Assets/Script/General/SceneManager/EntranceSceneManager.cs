using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceSceneManager : MonoBehaviour
{
    //버튼에 등록되어 있음.
    public void GameStart(int ID)
    {
        GameMaster.instance.GameStart(ID);
    }
}
