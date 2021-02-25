using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public Transform playerTransform;
    float accel = 0.8f;
    public Animator WeatherStage;
    enum Weather
    {
        REGULAR, RAIN, HOT, SNOW, HAIL, WIND, THUNDER
    }
    [SerializeField]Weather currentWeather = Weather.REGULAR;

    void Movement()
    {
        Debug.Log(currentWeather);
        
        if (isLocalPlayer)
        {
            WeatherEffect();
            
        }
    }


    void WeatherEffect()
    {
        //regular speed
        if (currentWeather == Weather.REGULAR)
        {
            SpeedAndWeather(accel,(int)currentWeather);
        }
        //super speed
        else if (currentWeather == Weather.HOT)
        {
            SpeedAndWeather(accel * 4, (int)currentWeather);
        }
        //speed decrease/ slip
        else if (currentWeather == Weather.RAIN)
        {
            SpeedAndWeather(accel / 4, (int)currentWeather);
        }
        //move on steps
        else if (currentWeather == Weather.SNOW)
        {
            SpeedAndWeather(accel, (int)currentWeather);
        }

        //hit by hail/ pushed down from time to time
        else if (currentWeather == Weather.HAIL)
        {
            SpeedAndWeather(accel, (int)currentWeather);
        }
        //move fast
        else if (currentWeather == Weather.WIND)
        {
            SpeedAndWeather(accel, (int)currentWeather);
        }
        //move fast on bursts
        else if (currentWeather == Weather.THUNDER)
        {
            SpeedAndWeather(accel, (int)currentWeather);
        }
    }

    void SpeedAndWeather(float accel, int Stage)
    { 
        WeatherStage.SetInteger("Weather", Stage);
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
    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }
}
