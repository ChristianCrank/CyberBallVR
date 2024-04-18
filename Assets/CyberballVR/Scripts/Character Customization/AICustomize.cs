using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;

public class AICustomize : MonoBehaviour
{
    public PlayerData playerData;

    public Material[] SkinColorOptions;
    public GameObject Body;
    public GameObject Head;

    [Header("Empty GameObj Folders for Customization Option")]
    public GameObject HairOptions;
    public GameObject ClothingOptions;
    public GameObject HeadAccessory1Option;
    public GameObject BodyAccessory2Options;


    public void RandomizeCustomization()
    {
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

        ClothingOptions.transform.Find(data.Clothing)?.gameObject.SetActive(true);
        HeadAccessory1Option.transform.Find(data.Head_Accessory_1)?.gameObject.SetActive(true);
        HairOptions.transform.Find(data.Hair)?.gameObject.SetActive(true);

        Material skinMaterial = GetSkinMaterialByName(data.SkinColor);
        if(skinMaterial != null && Body && Head)
        {
            Body.GetComponent<Renderer>().material = skinMaterial;
            Head.GetComponent<Renderer>().material = skinMaterial;
        }
       
    }

    private Material GetSkinMaterialByName(string skinColorName)
    {
        foreach (var material in SkinColorOptions)
        {
            if (material.name == skinColorName)
            {
                return material;
            }
        }
        Debug.LogWarning("No skin color material found for: " + skinColorName);
        return null;
    }


}
