using Mirror;
using UnityEngine;
using System.Collections;

public class Ball : NetworkBehaviour
{
    public Rigidbody2D ball_rigidBody;
    
    public Animator WeatherStage;
    
    public SpriteRenderer mySprite;
    private bool change = false;
    
    private int delay =100;
    enum Weather
    {
        REGULAR, RAIN, HOT, SNOW, HAIL, WIND, THUNDER
    }
    [SerializeField] Weather currentWeather = Weather.REGULAR;

    public override void OnStartServer()
    {
        base.OnStartServer();
        
        // only simulate ball physics on server
        ball_rigidBody.simulated = true;

        // Serve the ball from left player
        ball_rigidBody.velocity = Vector2.right * 2f;
    }
    [ClientCallback]
    void OnCollisionEnter2Ds(Collision2D col)
    {
        Debug.Log("Collision!");
        //Paddle physics
        if (col.collider.offset.y != -2.88f || col.collider.offset.y != -2.88f)
        {
            if (mySprite.flipX != true) mySprite.flipX = true;

            else mySprite.flipX = false;
        }
    }
    [ServerCallback]
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collision!");
        //Paddle physics
        if(col.collider.offset.y != -2.88f || col.collider.offset.y != -2.88f)
        {
            if (mySprite.flipX != true) mySprite.flipX = true;

            else mySprite.flipX = false;
        }
        
        
        WeatherEffect(col);
    }
    private void Update()
    {
        WeatherStage.SetInteger("Weather_Ball", (int)currentWeather);  
    }

    IEnumerator Wait(Collision2D col)
    {
        change = false;
        SpeedAndWeather(2f, col);
        yield return new WaitForSeconds(1.2f);
        change = false;
        SpeedAndWeather(.5f, col);
        
        
    }

    void WeatherEffect(Collision2D col)
    {
        Debug.Log("Speed: "+ ball_rigidBody.velocity);
        //regular speed
        if (currentWeather == Weather.REGULAR)
        {
            SpeedAndWeather(1f,col);
        }
        //super speed
        else if (currentWeather == Weather.HOT)
        {
            SpeedAndWeather(3f, col);
               
            
        }
        //speed decrease/ slip
        else if (currentWeather == Weather.RAIN)
        {
            SpeedAndWeather(0.75f, col);

        }
        //move on steps
        else if (currentWeather == Weather.SNOW)
        {

            SpeedAndWeather(0.50f, col);

        }

        //hit by hail/ pushed down from time to time
        else if (currentWeather == Weather.HAIL)
        {
           
        }
        //move fast
        else if (currentWeather == Weather.WIND)
        {
            StartCoroutine(Wait(col));

        }
        //move fast on bursts
        else if (currentWeather == Weather.THUNDER)
        {
            while (delay <200)
            {
                delay += 5;
                if (delay > 60)
                {
                    SpeedAndWeather(1f, col);
                }
                if (delay == 200) delay = 0;

            }
            
        }
    }

    void SpeedAndWeather(float speed, Collision2D col)
    {
        //if hit top collider

        if (col.collider.offset.y == 0.08f)
        {
            if (!change)
            {
                ball_rigidBody.velocity += (Vector2.up) * speed;
                change = true;
            }
            else
            {
                ball_rigidBody.velocity += (Vector2.up);
            }


        }
        //if hit middle collider

        else if (col.collider.offset.y == 0.0f)
        {
            if (!change)
            {
                ball_rigidBody.velocity *= speed;
                change = true;
            }
            else
            {
                ball_rigidBody.velocity += Vector2.right;
            }
        }

        //if hit bottom collider

        else if (col.collider.offset.y == -0.08f) {
            if (!change)
            {
                ball_rigidBody.velocity += (Vector2.down) * speed;
                change = true;
            }
            else
            {
                ball_rigidBody.velocity += (Vector2.down);
            }
        }

        //hits top Boundary

        else if (col.collider.offset.y == -2.88f)
        {
            if (!change)
            {
                ball_rigidBody.velocity += (Vector2.down) * speed;
                change = true;
            }
            else
            {
                ball_rigidBody.velocity += (Vector2.down);
            }
        }
        

        //hits bottom Boundary

        else if (col.collider.offset.y == 2.88f)
        {
            if (!change)
            {
                ball_rigidBody.velocity += (Vector2.up) * speed;
                change = true;
            }
            else
            {
                ball_rigidBody.velocity += (Vector2.up);
            }
        }
        
    }

}
