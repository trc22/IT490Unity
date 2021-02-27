using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public Transform lSpawn, rSpawn;
    private int playerCount;
    GameObject player, ball;


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
            ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
            NetworkServer.Spawn(ball);
        }

        NetworkServer.AddPlayerForConnection(conn, player);

    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        playerCount --;
        Debug.Log("Disconnected from Server!");
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        // destroy ball
        if (ball != null)
        NetworkServer.Destroy(ball);

        // call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);
    }

}
