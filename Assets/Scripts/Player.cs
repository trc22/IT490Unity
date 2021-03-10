using Mirror;
using UnityEngine;
using System.Collections;

public class Player : NetworkBehaviour
{
    public Transform playerTransform;
    public GameObject webManager;
    float accel = 0.8f;

    public Animator WeatherStage; //Handles the Animations
    int delay; //handles delays for effects
    [SerializeField]int threshold = 200; //Handles the max amount of Delay
    enum Weather
    {
        REGULAR, RAIN, HOT, SNOW, HAIL, WIND, THUNDER
    }
    [SerializeField]Weather currentWeather = Weather.REGULAR;

    void Awake()
    {
        webManager= GameObject.Find("WebManager");
        SetWeather(webManager.GetComponent<Web>().GetTemp(), webManager.GetComponent<Web>().GetStatus());
    }

    void Movement()
    {
        
        
        if (isLocalPlayer)
        {
            //Handles Weather effects
            WeatherEffect();
            
        }
    }


    void WeatherEffect()
    {
        WeatherStage.SetInteger("Weather", (int) currentWeather);
        //regular speed
        if (currentWeather == Weather.REGULAR)
        {
            SpeedAndWeather(accel);
        }
        //super speed
        else if (currentWeather == Weather.HOT)
        {
            SpeedAndWeather(accel * 4);
        }
        //speed decrease/ slip
        else if (currentWeather == Weather.RAIN)
        {
            SpeedAndWeather(accel / 4);
        }
        //move on steps
        else if (currentWeather == Weather.SNOW)
        {
            if (delay != 100)
            {
                delay += 10;
                if (delay < 50)
                {
                    SpeedAndWeather(accel);
                }
            }
            else delay = 0;
            
            
        }

        //hit by hail/ pushed down from time to time
        else if (currentWeather == Weather.HAIL)
        {
            SpeedAndWeather(accel);
        }
        //move fast at randomtimes due to wind, but then slow back down
        else if (currentWeather == Weather.WIND)
        {
            //Debug.Log(delay);
            if (delay < 200){
                if (delay > 100)
                {
                    SpeedAndWeather(accel*3);
                }
                else
                {
                    SpeedAndWeather(accel/2);
                }
                delay += 5;
            }
            else  delay = 0;


            
        }
        //move fast on bursts then studder due to the lightning
        else if (currentWeather == Weather.THUNDER)
        {
            
            Speed_DelayAndWeather(accel*3);

        }
    }

    void SpeedAndWeather(float accel)
    { 
        
        if (Input.GetKey("up"))
        {
            if (playerTransform.position.y < 7f) //Upper limit
                playerTransform.position += Vector3.up * accel;
        }
        if (Input.GetKey("down"))
        {
            if (playerTransform.position.y > -4.5f) //Lower limit
                playerTransform.position += Vector3.down * accel;
        }
    }

    void Speed_DelayAndWeather(float accel)
    {

        if (Input.GetKey("up"))
        {
            if (delay < threshold)
            {
                delay += 5;
                if (delay > 60)
                {
                    if (playerTransform.position.y < 7f) //Upper limit
                        playerTransform.position += Vector3.up * accel;
                }
                if (delay == threshold) delay = 0;

            }
        }
        if (Input.GetKey("down"))
        {
            if (delay < threshold)
            {
                delay += 5;
                if (delay > 60)
                {
                    if (playerTransform.position.y > -4.5f) //Lower limit
                    playerTransform.position += Vector3.down * accel;
            }
            if (delay == threshold) delay = 0;

            }
        }
    }
    // Update is called once per frame

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

        if(status == "snow" || status == "light snow" || status == "heavy snow")
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
        if(temp > 80f)
        {
            Debug.Log("Setting weather to hot!");
            currentWeather = Weather.HOT;
            return;
        }
        if(temp < 30)
        {
            Debug.Log("Setting weather to cold!");
            currentWeather = Weather.HAIL;
            return;
        }
            Debug.Log("Setting weather to regular");
            currentWeather = Weather.REGULAR;
            return;
        
    }


    void FixedUpdate()
    {
        Movement();
    }
}
