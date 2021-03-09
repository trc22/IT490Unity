using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Validation : MonoBehaviour
{
    public InputField username, password, location, date; //gets Input fields in the scene
    string _username, _password, _location, _date; //used to turn input fields into string
    // Start is called before the first frame update

    //function for after button press (currently connected to signUp)
    public void submit()
    {
        //turns input fields to string after button press
        _username = username.text;
        _password = password.text;
        _location = location.text;
        _date = date.text;

        //debug log to check its being sent currectly
        Debug.Log(_username+","+ _password + "," + _location + "," + _date);

        
    }
}
