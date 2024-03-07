using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeHandler : MonoBehaviour
{
    [Header("Customize Settings")]
    [SerializeField]
    private GameObject Player;

    public GameObject[] HairOptions;
   
    public void GetHairSelection(string OptionSelectedName)
    {
        foreach(var hair in HairOptions)
        {
            if (hair.name == OptionSelectedName)
            {
                // Set Object Active In Player

                // Put all Customization Options on player and turn em off and only turn the ones selected on
                Debug.Log("Found Hair!");
            }
        }
    }

    void GetClothingSelection(int Index)
    {

    }

    void GetAccessoryOneSelection(int Index)
    {

    }

    void GetAccessoryTwoSelection(int Index)
    {

    }
}
