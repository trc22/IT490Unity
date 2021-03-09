using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class Validation : MonoBehaviour
{
    public GameObject Access;
    public TextMeshProUGUI accessText;
    private bool grantAccess = false;

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
        
        //turns input fields to string after button press
        _username = username.text;
        _password = password.text;
        _location = location.text;
        _date = date.text;

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
        }




    }
    public void SignIn() //retrieves data from database through rabbit
    {
        //turns input fields to string after button press
        _username = username.text;
        _password = password.text;
        _location = location.text;
        _date = date.text;

        //debug log to check its being sent currectly
        //Debug.Log(_username + "," + _password + "," + _location + "," + _date);


        //include function to change access denied to true here after checking if
        //username is in the database-------------------------------------------------------------------





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
        }
    }


    /// <summary>
    /// Sends User to webserver specified on urlToWebsite
    /// </summary>
    public void sendUserToWebsite()
    {
        urlToWebsite = "www.google.com";
        Application.OpenURL(urlToWebsite);
    }
}
