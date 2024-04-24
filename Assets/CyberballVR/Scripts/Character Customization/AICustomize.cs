using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AICustomize : MonoBehaviour
{
    public PlayerData playerData;

    public Material[] SkinColorOptions;
    public GameObject Body;
    public GameObject Head;

    public TextMeshProUGUI UIName;

    [Header("Empty GameObj Folders for Customization Option")]
    public GameObject HairOptions;
    public GameObject ClothingOptions;
    public GameObject HeadAccessory1Option;
    public GameObject HeadAccessory2Option;


    public void RandomizeCustomization()
    {
        Debug.Log("RandomizeCustomization");
        int HairCount = HairOptions.transform.childCount;
        int ClothingCount = ClothingOptions.transform.childCount;
        int HeadAccessory1Count = HeadAccessory1Option.transform.childCount;
      //  int BodyAccessory2Count = BodyAccessory2Options.transform.childCount;
        
        int RandomHair = Random.Range(0, HairCount);
        int RandomClothing = Random.Range(0, ClothingCount);
        int RandomHeadAccessory1 = Random.Range(0, HeadAccessory1Count);
      //  int RandomBodyAccessory2 = Random.Range(0, BodyAccessory2Count);
        int RandomSkinColor = Random.Range(0, SkinColorOptions.Length);

       

        if (ClothingOptions.transform.GetChild(RandomClothing))
        {
            ClothingOptions.transform.GetChild(RandomClothing).gameObject.SetActive(true);
        }
       
        if (HeadAccessory1Option.transform.GetChild(RandomHeadAccessory1))
        {
            HeadAccessory1Option.transform.GetChild(RandomHeadAccessory1).gameObject.SetActive(true);
        }

        if (HairOptions.transform.GetChild(RandomHair) && !HeadAccessory1Option.transform.GetChild(RandomHeadAccessory1).name.Contains("Hat"))
        {
            HairOptions.transform.GetChild(RandomHair).gameObject.SetActive(true);
        }

       
        /*
        if (BodyAccessory2Options.transform.GetChild(RandomBodyAccessory2))
        {
            BodyAccessory2Options.transform.GetChild(RandomBodyAccessory2).gameObject.SetActive(true);

        }
        */

        if (Body && Head)
        {
            Body.GetComponent<Renderer>().material = SkinColorOptions[RandomSkinColor];
            Head.GetComponent<Renderer>().material = SkinColorOptions[RandomSkinColor];
        }
    }

    public void ReadInCustomization(PlayerData data)
    {
        playerData = data;

        SetActiveOption(ClothingOptions.transform, data.Clothing);

        bool hasHeadAccessory = SetActiveOption(HeadAccessory1Option.transform, data.Accessory_1);
        // If there is a head accessory, does not activate hair. Otherwise, activates hair.
        if (!hasHeadAccessory)
        {
            SetActiveOption(HairOptions.transform, data.Hair);
        }
        SetActiveOption(HeadAccessory2Option.transform, data.Accessory_2);

        Material skinMaterial = GetSkinMaterialByName(data.SkinColor);
        if (skinMaterial != null && Body && Head)
        {
            Body.GetComponent<Renderer>().material = skinMaterial;
            Head.GetComponent<Renderer>().material = skinMaterial;
        }

        UIName.text += data.Name;
        
    }

    private bool SetActiveOption(Transform optionsParent, string optionName)
    {

        foreach (Transform child in optionsParent)
        {
            child.gameObject.SetActive(false);
        }

        if (!optionName.Equals("None"))
        {
            Transform foundOption = optionsParent.Find(optionName);
            if (foundOption)
            {
                foundOption.gameObject.SetActive(true);
                return true; 
            }
        }

        return false; 
    }

    private Material GetSkinMaterialByName(string skinColorName)
    {
        foreach (var material in SkinColorOptions)
        {

            if (material.name == skinColorName)
            {
                return material;
            }
            else if(skinColorName.Equals("Gray"))
            {
                return SkinColorOptions[0];
            }
        }
        Debug.LogWarning("No skin color material found for: " + skinColorName);
        return null;
    }


}
