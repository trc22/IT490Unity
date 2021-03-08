using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

// UnityWebRequest.Get example

// Access a website and use UnityWebRequest.Get to download a page.
// Also try to download a non-existing page. Display the error.

public class Web : MonoBehaviour
{
    string temp, status;

    void Awake()
    {
        StartCoroutine(GetWeatherFromServer());
    }

    IEnumerator GetWeatherFromServer()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://192.168.1.62/get-weather.php?location=westwood"))
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
        return((float)Convert.ToDouble(temp));
    }

    public string GetStatus()
    {
        return(status);
    }
 

}