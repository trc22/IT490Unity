using Mirror;
using UnityEngine.UI;
using UnityEngine;

public class Game : NetworkBehaviour
{
    [SyncVar] int lScore;
    [SyncVar] int rScore;
    private string lName, rName;
    public Text lScore_text, rScore_text;
    public Text lName_text, rName_text;
    

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Game Manager is active");

        GetText();
        lScore = 0;
        rScore = 0;
    }

    void Update()
    {
        if(lScore_text == null)
            GetText();
        RpcSyncScores(lScore, rScore);
    }

    public void Scored(int x)
    {
        if(x == 0) //Left scored
        {
            lScore ++;
        }
        else //Right scored
        {
            rScore ++;
        }

        if(lScore == 7) //Left wins
        {

        }

        if (rScore == 7) //Right wins
        {

        }

    }

    void RpcSyncScores(int lVar, int rVar)
    {
        lScore = lVar;
        rScore = rVar;
        lScore_text.text = (""+lScore);
        rScore_text.text = (""+rScore);
    }

    

    void UpdateNames()
    {
        lName_text.text = lName;
        rName_text.text = rName;
    }

    public void UpdatePlayers(int target, string name)
    {
        if (target == 0)
            lName = name;
        else
            rName = name;
    
        UpdateNames();

    }

    void GameOver(int x)
    {
        if(x == 0) //Left wins
        {
            
        }
        else //Right wins
        {

        }
    }

    public void GetText()
    {
        lScore_text = GameObject.Find("LScore").GetComponent<Text>();
        rScore_text = GameObject.Find("RScore").GetComponent<Text>();
        lName_text = GameObject.Find("LName").GetComponent<Text>();
        rName_text = GameObject.Find("RName").GetComponent<Text>();
    }
}
