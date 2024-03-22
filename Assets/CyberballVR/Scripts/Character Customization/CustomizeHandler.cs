using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomizeHandler : MonoBehaviour
{
    [Header("Customize Settings")]
    [SerializeField]
    private GameObject Player;

    public Material[] SkinColorOptions;

    public GameObject LeftHandController, RightHandController;
    [Header("Empty GameObj Folders for Customization Option")]
    public GameObject HairOptions;
    public GameObject ClothingOptions;
    public GameObject HeadAccessory1Option;
    public GameObject HeadAccessory2Options;
    public GameObject BodyAccessory1Options;
    public GameObject BodyAccessory2Options;

    string Accessory1_Selection;
    string Accessory2_Selection;

    [Header("Accessory Dropdowns")]
    public TMP_Dropdown Accessory1_dropdown;
    public TMP_Dropdown Accessory2_dropdown;

    public void GetHairSelection(string OptionSelectedName)
    {
        foreach (Transform child in HairOptions.transform)
        {
            if (child.name == OptionSelectedName)
            {
                child.gameObject.SetActive(true);
                Debug.Log("Found Hair!");
            }
            else
            {
                if (child.name != "Left Eye" && child.name != "Right Eye" && child.name != "TopOfHead")
                {
                    child.gameObject.SetActive(false);
                }
            }
        }  
    }

    public void GetSkinColorSelection(string SkinColor)
    {
        var Body = Player.transform.GetChild(0);
        var Neck = Body.transform.GetChild(0);
        var Head = Neck.transform.GetChild(0);

        foreach (var skin in SkinColorOptions)
        {
            if (skin.name == SkinColor)
            {
              //  Debug.Log("Applying Skin Color to Player");
                Body.GetComponent<Renderer>().material = skin;
                Head.GetComponent<Renderer>().material = skin;

                if (LeftHandController && RightHandController)
                {
                    LeftHandController.GetComponent<Renderer>().material = skin;
                    RightHandController.GetComponent<Renderer>().material = skin;
                }
            }
        }
    }

    public void GetClothingSelection(string OptionSelectedName)
    {
        foreach (Transform child in ClothingOptions.transform)
        {
            if (child.name == OptionSelectedName)
            {
                child.gameObject.SetActive(true);
                Debug.Log("Found Clothing Selection!");
            }
            else
            {
                if (child.name != "Left Eye" && child.name != "Right Eye" && child.name != "TopOfHead")
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    public void GetAccessoryOneSelection(string OptionSelectedName)
    {
        if (OptionSelectedName == Accessory2_Selection)
        {
            Accessory2_dropdown.value = 0;

            foreach (Transform child in HeadAccessory2Options.transform)
            {
                child.gameObject.SetActive(false);
            }
            foreach (Transform child in BodyAccessory2Options.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        foreach (Transform child in HeadAccessory1Option.transform)
        {
            if (child.name == OptionSelectedName)
            {
                child.gameObject.SetActive(true);
                Accessory1_Selection = child.name;
                Debug.Log("Found Accessory 1 Selection!");
            }
            else
            {
                if (child.name != "Left Eye" && child.name != "Right Eye" && child.name != "TopOfHead")
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        foreach (Transform child in BodyAccessory1Options.transform)
        {
            if (child.name == OptionSelectedName)
            {
                child.gameObject.SetActive(true);
                Accessory1_Selection = child.name;
                Debug.Log("Found Accessory 1 Selection!");
            }
            else
            {
                if (child.name != "Left Eye" && child.name != "Right Eye" && child.name != "TopOfHead")
                {
                    child.gameObject.SetActive(false);
                }
            }
        }


    }

    public void GetAccessoryTwoSelection(string OptionSelectedName)
    {
        if (OptionSelectedName == Accessory1_Selection)
        {
            Accessory1_dropdown.value = 0;

            foreach (Transform child in HeadAccessory1Option.transform)
            {
                child.gameObject.SetActive(false);
            }
            foreach (Transform child in BodyAccessory1Options.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        foreach (Transform child in HeadAccessory2Options.transform)
        {
            if (child.name == OptionSelectedName)
            {
                child.gameObject.SetActive(true);
                Accessory2_Selection = child.name;
                Debug.Log("Found Accessory 2 Selection!");
            }
            else
            {
                if (child.name != "Left Eye" && child.name != "Right Eye" && child.name != "TopOfHead")
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        foreach (Transform child in BodyAccessory2Options.transform)
        {
            if (child.name == OptionSelectedName)
            {
                child.gameObject.SetActive(true);
                Accessory2_Selection = child.name;
                Debug.Log("Found Accessory 2 Selection!");
            }
            else
            {
                if (child.name != "Left Eye" && child.name != "Right Eye" && child.name != "TopOfHead")
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}
