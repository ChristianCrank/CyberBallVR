using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownHandler : MonoBehaviour
{
    CustomizeHandler customizeHandler;

    private void Start()
    {
        customizeHandler = GameObject.FindObjectOfType<CustomizeHandler>();

        var dropdown = GetComponent<TMP_Dropdown>();
       
        dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });
    }

    void DropdownItemSelected(TMP_Dropdown dropdown)
    {
        string OptionType = dropdown.name;
        int Value = dropdown.value;
        string NameTest = dropdown.options[dropdown.value].text;
        
        Debug.Log("You have selected " + Value + " Of " + OptionType + " The Name is: " + NameTest);

        if (customizeHandler != null)
        {
            if (OptionType == "Hair Options")
            {
                customizeHandler.GetHairSelection(NameTest);
            }
        }
    }

    
}
