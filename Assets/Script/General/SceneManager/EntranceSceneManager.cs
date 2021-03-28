using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceSceneManager : MonoBehaviour
{
    private LoadManager loadM;
    // Start is called before the first frame update
    void Start()
    {
        loadM = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<LoadManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart(int ID)
    {
        loadM.GameStart(ID);
    }
}
