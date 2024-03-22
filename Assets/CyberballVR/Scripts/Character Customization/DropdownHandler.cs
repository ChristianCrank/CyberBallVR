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
        int OptionValue = dropdown.value;
        string OptionName = dropdown.options[dropdown.value].text;
        
        Debug.Log("You have selected " + OptionValue + " Of " + OptionType + " The Name is: " + OptionName);

        if (customizeHandler != null)
        {
            if (OptionType == "Hair Options")
            {
                customizeHandler.GetHairSelection(OptionName);
            } else if (OptionType == "Clothing Options")
            {
                customizeHandler.GetClothingSelection(OptionName);
            }
            else if (OptionType == "Accessories 1 Options")
            {
                customizeHandler.GetAccessoryOneSelection(OptionName);
            }
            else if (OptionType == "Accessories 2 Options")
            {
                customizeHandler.GetAccessoryTwoSelection(OptionName);
            }
        }
    }

    
}
