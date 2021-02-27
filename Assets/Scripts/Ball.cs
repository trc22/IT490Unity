using Mirror;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    public Rigidbody2D ball_rigidBody;

    public override void OnStartServer()
    {
        base.OnStartServer();

        // only simulate ball physics on server
        ball_rigidBody.simulated = true;

        if(Random.Range(0f, 2f) > 1f) //Serve ball in random dir
            ball_rigidBody.velocity = Vector2.right * 3f;
        else
            ball_rigidBody.velocity = Vector2.left * 3f;    
    }

    [ServerCallback]
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collision!");
        //Paddle physics
        if (col.collider.offset.y == 0.32f) //if hit top collider
            ball_rigidBody.velocity += (Vector2.up / 2f);
        else if (col.collider.offset.y == -0.32f) //if hit bottom collider
            ball_rigidBody.velocity += (Vector2.down / 2f);
    }

    void FixedUpdate()
    {
        if (ball_rigidBody.position.x > 17f) //If left scored
        {
            ResetBall();
        }

        if(ball_rigidBody.position.x < -17f) //If right scored
        {
            ResetBall();
        }
    }

    void ResetBall()
    {
        ball_rigidBody.GetComponent<SpriteRenderer>().enabled = false; //Make ball invis

        ball_rigidBody.velocity = Vector2.zero; //Reset velocity
        ball_rigidBody.position = Vector2.zero; //Reset position

        ball_rigidBody.GetComponent<SpriteRenderer>().enabled = true; //Make ball visible

        if(Random.Range(0f, 2f) > 1f) //Serve ball in random dir
            ball_rigidBody.velocity = Vector2.right * 3f;
        else
            ball_rigidBody.velocity = Vector2.left * 3f;
    }
}
