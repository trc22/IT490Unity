﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// UnityWebRequest.Get example

// Access a website and use UnityWebRequest.Get to download a page.
// Also try to download a non-existing page. Display the error.

public class Web : MonoBehaviour
{

    void Awake()
    {
        StartCoroutine(GetWeatherFromServer());
    }
    public IEnumerator GetWeatherFromServer()
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
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }

    

    

}