using Mirror;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    public Rigidbody2D ball_rigidBody;
    public Animator WeatherStage;
    public SpriteRenderer mySprite;
    private bool change;
    
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

    [ServerCallback]
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collision!");
        //Paddle physics
        if (mySprite.flipX != true)
        {
            mySprite.flipX = true;
        }
        else
        {
            mySprite.flipX = false;
        }
        
        WeatherEffect(col);
    }
    private void Update()
    {
        WeatherStage.SetInteger("Weather_Ball", (int)currentWeather);  
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
            SpeedAndWeather(2f, col);
               
            
        }
        //speed decrease/ slip
        else if (currentWeather == Weather.RAIN)
        {
            SpeedAndWeather(0.5f, col);

        }
        //move on steps
        else if (currentWeather == Weather.SNOW)
        {
            if (delay != 100)
            {
                delay += 10;
                if (delay < 50)
                {
                    SpeedAndWeather(1f, col);
                }
            }
            else delay = 0;
        }

        //hit by hail/ pushed down from time to time
        else if (currentWeather == Weather.HAIL)
        {
           
        }
        //move fast
        else if (currentWeather == Weather.WIND)
        {
            SpeedAndWeather(1f, col);
            if (delay < 200)
            {
                if (delay > 100)
                {
                    SpeedAndWeather(2f, col);
                }
                else
                {
                    SpeedAndWeather(0.5f, col);
                }
                delay += 5;
            }
            else delay = 0;

        }
        //move fast on bursts
        else if (currentWeather == Weather.THUNDER)
        {
            if (delay <200)
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
        
        if (col.collider.offset.y == 0.08f) //if hit top collider
            ball_rigidBody.velocity += (Vector2.up)*speed;
        if (col.collider.offset.y == 0.0f)
        {
            if (change)
            {
                ball_rigidBody.velocity *= speed;
            }
            

        }
        else if (col.collider.offset.y == -0.08f) //if hit bottom collider
            ball_rigidBody.velocity += (Vector2.down)*speed;
    }

}
