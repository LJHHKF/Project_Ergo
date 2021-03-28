using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public LoadManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<LoadManager>();
            return m_instance;
        }
    }
    private static LoadManager m_instance;

    //[SerializeField] private int saveID = 0;
    private bool isBattleReady = false;

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    public void GameStart(int ID)
    {
        GameMaster.GameStart(ID);
        isBattleReady = true;
        LoadingSceneManager.LoadScene("Battle");
    }

    public static void ChkAndPlayDelayOn()
    {
        if(m_instance.isBattleReady)
        {
            m_instance.isBattleReady = false;
            m_instance.StartCoroutine(m_instance.OnDelayedTurnStart());
        }
    }

    IEnumerator OnDelayedTurnStart()
    {
        yield return new WaitForSeconds(2.0f);
        TurnManager.OnFirstTurn();
        yield break;

    }
}
