using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Historical_Toggle : MonoBehaviour
{
    public GameObject inputField;
    public Toggle historicSettingToggle;
    // Start is called before the first frame update
    void Start()
    {
        inputField.SetActive(false);
    }


    public void Update()
    {
        if (historicSettingToggle.isOn)
        {
            inputField.SetActive(true);
        }
        else
        {
            inputField.SetActive(false);
        }
    }
}
