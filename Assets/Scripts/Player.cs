using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public Transform playerTransform;
    float accel = 0.8f;
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
        if (currentWeather == Weather.REGULAR) Speed(accel);
        //super speed
        else if (currentWeather == Weather.HOT) Speed(accel*4);
        //speed decrease/ slip
        else if (currentWeather == Weather.RAIN) Speed(accel/4);

        //move on steps
        else if (currentWeather == Weather.SNOW)
        {
            Speed(accel);
        }

        //hit by hail/ pushed down from time to time
        else if (currentWeather == Weather.HAIL)
        {
            Speed(accel);
        }
        //move fast
        else if (currentWeather == Weather.WIND)
        {
            Speed(accel);
        }
        //move fast on bursts
        else if (currentWeather == Weather.THUNDER)
        {
            Speed(accel);
        }
    }

    void Speed(float accel)
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
    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }
}
