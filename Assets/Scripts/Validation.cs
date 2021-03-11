using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Validation : MonoBehaviour
{
    public GameObject Access;
    public TextMeshProUGUI accessText;
    //private bool grantAccess = false;

    public InputField username, password, location, date; //gets Input fields in the scene
    string _username, _password, _location, _date; //used to turn input fields into string

    private string urlToWebsite;

    //function for after button press (currently connected to signUp)
    private void Start()
    {
        Access.SetActive(false);
    }

    private IEnumerator displayAcessDenied(float seconds)
    {
        Access.SetActive(true);
        yield return new WaitForSeconds(seconds);
        Access.SetActive(false);
    }

    public void SignUp() //adds a data to database through rabbit
    {
        
        /*//turns input fields to string after button press
        _username = username.text;
        _password = password.text;
        _location = location.text;
        _date = date.text;

        */Application.OpenURL("http://192.168.1.62/account_create.php");/*

        //debug log to check its being sent currectly
        //Debug.Log(_username+","+ _password + "," + _location + "," + _date);

        //include function to change access denied to true here after checking if
        //username is taken-------------------------------------------------------------------------


        grantAccess = true;
        
        
        
        //--------------------------------------------------------------------------------------------
        //Checks if grant access still false and displays a window stating such
        //Cleans up username and password
        
        ///<remarks>
        ///Need to add If username is taken
        /// </remarks>
        if (grantAccess == false)
        {
            accessText.text = "Username is Taken";
            StartCoroutine(displayAcessDenied(1f));

            username.text = null;
            password.text = null;
        }
        else
        {
            SceneManager.LoadScene("Main");
        }*/




    }
    public void SignIn() //retrieves data from database through rabbit
    {
        //turns input fields to string after button press
        _username = username.text;
        _password = password.text;
        _location = location.text;
        _date = date.text;

        if(_username == "" || _username == null)
        {
                accessText.text = "Username is blank";
                StartCoroutine(displayAcessDenied(1f));
                username.text = null;
                return;
        }

        if(_password == "" || password == null)
        {
                accessText.text = "Password is blank";
                StartCoroutine(displayAcessDenied(1f));
                password.text = null;
                return;
        }

        if(_location == "" || _location == null)
        {
                accessText.text = "location is blank";
                StartCoroutine(displayAcessDenied(1f));
                location.text = null;
                return;
        }

        //debug log to check its being sent currectly
        //Debug.Log(_username + "," + _password + "," + _location + "," + _date);


        //include function to change access denied to true here after checking if
        //username is in the database-------------------------------------------------------------------



        StartCoroutine(SendLoginRequest());
        /*

        //----------------------------------------------------------------------------------------------
        //Checks if grant access still false and displays a window stating why
        //Cleans up username and password

        /// <remarks>
        /// Need to add if username is in database
        /// </remarks>
        if (grantAccess == false) 
        {
            accessText.text = "Access Denied";
            StartCoroutine(displayAcessDenied(1f));

            username.text = null;
            password.text = null;
        }
        else
        {
            SceneManager.LoadScene("Main");
        }*/
    }


    /// <summary>
    /// Sends User to webserver specified on urlToWebsite
    /// </summary>
    public void sendUserToWebsite()
    {
        urlToWebsite = "http://25.14.165.46";
        Application.OpenURL(urlToWebsite);
    }

    IEnumerator SendLoginRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", _username);
        form.AddField("password", _password);


        UnityWebRequest webRequest = UnityWebRequest.Post("http://25.14.165.46/unity-login.php", form);
        
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            string[] response = webRequest.downloadHandler.text.Split('|');
            //Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
            if(response[1] == "login success")
            {
                Debug.Log("Success!");
                GameValues.username = _username;
                GameValues.location = _location;
                if(_date != null)
                    GameValues.date = _date;
                Debug.Log(GameValues.username + GameValues.location + GameValues.date);

                SceneManager.LoadScene("Main");
            }
            else
            {    
                accessText.text = "Access Denied";
                StartCoroutine(displayAcessDenied(1f));
                username.text = null;
                password.text = null;
            }
        }
    }
}
