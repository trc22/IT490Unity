using Mirror;
using UnityEngine;
using System.Collections;

public class Ball : NetworkBehaviour
{
    public Rigidbody2D ball_rigidBody;
    public GameObject gameManager, webManager;

    public Animator WeatherStage;
    
    public SpriteRenderer mySprite;
    private bool change = false; //this change is used for all behaviors that increase or decrease things, is used so they dont infinetly do that.
    private bool check = false; //this check is used for thunder behavio(as it works differently it needs its own in tandom with the 'change',
                                // as its to check that it does not infinetly increase its speed.
    
    
    enum Weather //stages Weather into Numbers
    {
        REGULAR, RAIN, HOT, SNOW, HAIL, WIND, THUNDER
    }
    [SerializeField] Weather currentWeather = Weather.REGULAR;//sets it to the Default of Regular

    enum Stages //This is specific for Thunder as it needs to be staged so its modifiers are on and if it goes at normal or gast speed
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

        webManager= GameObject.Find("WebManager");


        SetWeather(webManager.GetComponent<Web>().GetTemp(), webManager.GetComponent<Web>().GetStatus());

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
        if(webManager == null)
            webManager= GameObject.Find("WebManager");
        
         WeatherStage.SetInteger("Weather_Ball", (int)currentWeather);  //sets the Animations for the weather according to the currentWeather
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

    void SetWeather(float temp, string status)
    {
        Debug.Log("temp: " + temp + " status: " + status);
        //Prioritize status effects over temp effects

        //Status effects:
        if(status == "rain" || status == "light rain" || status == "moderate rain" || status == "heavy intensity rain")
        {
            Debug.Log("Setting weather to rain!");
            currentWeather = Weather.RAIN;
            return;
        }

        if(status == "Snow" || status == "light snow" || status == "Heavy Snow")
        {
            Debug.Log("Setting weather to snow!");
            currentWeather = Weather.SNOW;
            return;
        }

        if(status == "freezing rain")
        {
            Debug.Log("Setting weather to hail!");
            currentWeather = Weather.HAIL;
            return;
        }
        if(status == " thunderstorm" || status == " light thunderstorm " || status == " thunderstorm with rain " || status == "heavy thunderstorm")
        {
            Debug.Log("Setting weather to thunder!");
            currentWeather = Weather.THUNDER;
            return;
        } 
        if(Random.Range(0f, 2f) > 1f)
        {
            Debug.Log("Setting weather to windy!");
            currentWeather = Weather.WIND;
            return;
        }
        if(temp > 303f)
        {
            Debug.Log("Setting weather to hot!");
            currentWeather = Weather.HOT;
            return;
        }
        if(temp < 278f)
        {
            Debug.Log("Setting weather to cold!");
            currentWeather = Weather.HAIL;
            return;
        }
            Debug.Log("Setting weather to regular");
            currentWeather = Weather.REGULAR;
            return;
        
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

    void SpeedAndWeather(float speed, Collision2D col) //speed is the increase or decrease of speed per collision, and col is just the collider
    {
        //if hit top collider
        if (currentStage != Stages.OFF) //checks if Stages for THunder are off, if on it will make it so the speed can change every time it collides
        {
            change = false;
        }

        //if collider hit top collider
        if (col.collider.offset.y == 0.08f) 
        {
            
            if (!change) //if the speed has not changed before it does, this is done so speed doesnt go infinetly up or down
            {
                ball_rigidBody.velocity += (Vector2.up) * speed; //goes up by the normal speed * speed added
                change = true; //sets change to true for next collision
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
                ball_rigidBody.velocity *= speed; //goes middle by the speed added
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
                ball_rigidBody.velocity += (Vector2.down) * speed; //goes down by the normal speed * speed added
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
