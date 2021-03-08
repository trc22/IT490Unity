using Mirror;
using UnityEngine;
using System.Collections;

public class Ball : NetworkBehaviour
{
    public Rigidbody2D ball_rigidBody;
    public GameObject gameManager;

    public Animator WeatherStage;
    
    public SpriteRenderer mySprite;
    private bool change = false; //this change is used for all behaviors that increase or decrease things, is used so they dont infinetly do that.
    private bool check = false; //this check is used for thunder behavio(as it works differently it needs its own in tandom with the 'change',
                                // as its to check that it does not infinetly increase its speed.
    
    
    enum Weather
    {
        REGULAR, RAIN, HOT, SNOW, HAIL, WIND, THUNDER
    }
    [SerializeField] Weather currentWeather = Weather.REGULAR;
    enum Stages
    {
        OFF, NORMAL, FAST
    }
    Stages currentStage = Stages.OFF;

    public override void OnStartServer()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");

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

        //Flips the sprite only when hiting the paddles
        if(col.collider.offset.y != -2.88f || col.collider.offset.y != -2.88f)
        {
            if (mySprite.flipX != true) mySprite.flipX = true;

            else mySprite.flipX = false;
        }
        
        //Function that handles weather effects
        WeatherEffect(col);
    }

    void Update()
    {
        if(gameManager == null)
                gameManager = GameObject.FindGameObjectWithTag("GameManager");
        
         WeatherStage.SetInteger("Weather_Ball", (int)currentWeather);  
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (ball_rigidBody.position.x > 17f) //If left scored
        {
            Debug.Log("Scored!");
            gameManager.GetComponent<Game>().Scored(0);
            ResetBall();
        }

        if(ball_rigidBody.position.x < -17f) //If right scored
        {
            Debug.Log("Scored!");
            gameManager.GetComponent<Game>().Scored(1);
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

    IEnumerator Wait(Collision2D col,float start, float end, float secs)
    {
        change = false;
        if(check == false && currentWeather == Weather.THUNDER)
        {
            SpeedAndWeather(1f, col);
            check = true;
        }
        else
        {
            SpeedAndWeather(start, col);
        }
        yield return new WaitForSeconds(secs);
        change = false;
        SpeedAndWeather(end, col);
        
        
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
           
           if(currentStage == Stages.FAST || currentStage == Stages.OFF)
            {
                SpeedAndWeather(2f, col);
                currentStage = Stages.NORMAL;
            }
            else
            {
                SpeedAndWeather(.5f, col);
                currentStage = Stages.FAST;
            }
        }
        //move fast
        else if (currentWeather == Weather.WIND)
        {
            StartCoroutine(Wait(col,4f,.25f,2f));

        }
        //move fast on bursts
        else if (currentWeather == Weather.THUNDER)
        {
            StartCoroutine(Wait(col, .5f, 2f,1.2f));

        }
    }

    void SpeedAndWeather(float speed, Collision2D col)
    {
        //if hit top collider
        if (currentStage != Stages.OFF)
        {
            change = false;
        }
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
