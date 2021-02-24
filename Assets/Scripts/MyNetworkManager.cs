using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public Transform lSpawn, rSpawn;
    private int playerCount;
    GameObject player;


    public override void OnStartServer()
    {
        Debug.Log("Server Started!");
    }

    public override void OnStopServer()
    {
        Debug.Log("Server Stopped!");
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Connected to Server!");

    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        playerCount ++;

        if(playerCount == 1)
        {
            player = Instantiate(playerPrefab, lSpawn.position, lSpawn.rotation);
        }

        if(playerCount == 2)
        {
            player = Instantiate(playerPrefab, rSpawn.position, rSpawn.rotation);
            //Spawn ball
        }

        NetworkServer.AddPlayerForConnection(conn, player);

    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        playerCount --;
        Debug.Log("Disconnected from Server!");
    }
}
