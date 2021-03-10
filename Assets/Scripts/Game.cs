using Mirror;
using UnityEngine.UI;
using UnityEngine;

public class Game : NetworkBehaviour
{
    [SyncVar] int lScore;
    [SyncVar] int rScore;
    [SyncVar] string lName;
    [SyncVar] string rName;
    private string victor = "";
    public Text lScore_text, rScore_text;
    public Text lName_text, rName_text, result_text;
    public GameObject networkManager, webManager, lPlayer, rPlayer;
    public GameObject[] players;
    

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Game Manager is active");

        networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        webManager= GameObject.Find("WebManager");

        lName = null;
        rName = null;

        lScore = 0;
        rScore = 0;

        GetText();
        SetupPlayers();

    }

    void Update()
    {           
        if(lScore_text == null)
            GetText();
        if(networkManager == null)
            networkManager = GameObject.FindGameObjectWithTag("NetworkManager");

        if(lName != null && rName != null && lName != "" && rName != "")
        {
            RpcSyncNames(lName, rName);
        }
        else
        {
            SetupPlayers();
        }
        RpcSyncScores(lScore, rScore);

        RpcSyncWinner(victor);
    }

    public void Scored(int x)
    {
        if(x == 0) //Left scored
            lScore ++;
        else //Right scored
            rScore ++;

    }

    void RpcSyncScores(int lVar, int rVar)
    {
        lScore = lVar;
        rScore = rVar;
        lScore_text.text = (""+lScore);
        rScore_text.text = (""+rScore);
    }

    void RpcSyncWinner(string winner)
    {
        victor = winner;
        if(lScore == 1) //Left wins
        {
            victor = (lName + " wins!");
            GameOver();
        }
        else if (rScore == 1) //Right wins
        {
            victor = (rName + " wins!");
            GameOver();
        }
        result_text.text = victor;
    }

    void RpcSyncNames(string lVar, string rVar)
    {
        lName = lVar;
        rName = rVar;
        lName_text.text = lName;
        rName_text.text = rName;
    }

    [ServerCallback]
    void GameOver()
    {
        networkManager.GetComponent<MyNetworkManager>().EndGame();
    }

    public void GetText()
    {
        lScore_text = GameObject.Find("LScore").GetComponent<Text>();
        rScore_text = GameObject.Find("RScore").GetComponent<Text>();
        lName_text = GameObject.Find("LName").GetComponent<Text>();
        rName_text = GameObject.Find("RName").GetComponent<Text>();
        result_text = GameObject.Find("Results").GetComponent<Text>();
    }

    void SetupPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if(player.transform.position.x == -10)
            {
                lPlayer = player;
                lName = lPlayer.GetComponent<Player>().GetPlayerName();
                Debug.Log("L player = " + lName);
            }
            else if (player.transform.position.x == 10)
            {
                rPlayer = player;
                rName = rPlayer.GetComponent<Player>().GetPlayerName();
                Debug.Log("R player = " + rName);
            }
        }
    }

}
