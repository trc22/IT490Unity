using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;


public class Web : MonoBehaviour
{
    string temp, status, url;

    void Awake()
    {
        StartCoroutine(GetWeatherFromServer());
    }

    IEnumerator GetWeatherFromServer()
    {
        if(GameValues.date == null || GameValues.date == "null" || GameValues.date == "")
        {
            url = ("http://192.168.1.62/get-weather.php?location="+GameValues.location);
        }
        else
        {
            url = ("http://192.168.1.62/get-weather-historical.php?location="+GameValues.location+"&date="+GameValues.date);
            Debug.Log(url);
        }
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                string[] weather = webRequest.downloadHandler.text.Split('|');
                //Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                temp = weather[1];
                status = weather[2];
            }
        }

        
    }

    public float GetTemp()
    {
        if(GameValues.date == null || GameValues.date == "null" || GameValues.date == "")
                return((float)((Convert.ToDouble(temp) - 273) * 9 / 5 + 32)); //Convert from kelvin to farenheit

        return((float)Convert.ToDouble(temp));
    }

    public string GetStatus()
    {
        status = status.ToLower();
        return status; //Make
    }
    

}