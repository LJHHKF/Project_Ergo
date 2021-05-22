using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSceneManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            LoadManager.instance.ReturnLobby_GameOver();
        }
    }
}
