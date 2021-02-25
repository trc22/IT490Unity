using Mirror;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    public Rigidbody2D ball_rigidBody;
    enum Weather
    {
        REGULAR, RAIN, HOT, SNOW, HAIL, WIND, THUNDER
    }
    Weather currentWeather = Weather.REGULAR;
    public override void OnStartServer()
    {
        base.OnStartServer();

        // only simulate ball physics on server
        ball_rigidBody.simulated = true;

        // Serve the ball from left player
        ball_rigidBody.velocity = Vector2.right * 2f;
    }

    [ServerCallback]
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collision!");
        //Paddle physics
        if (col.collider.offset.y == 0.32f) //if hit top collider
            ball_rigidBody.velocity += (Vector2.up);
        else if (col.collider.offset.y == -0.32f) //if hit bottom collider
            ball_rigidBody.velocity += (Vector2.down);
    }

}
