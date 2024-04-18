using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.IO;

public class ResearchData : MonoBehaviour
{
    public static List<PlayerData> AIPlayers;
    private void Awake()
    {
         AIPlayers = LoadAllPlayers();
    }


    public List<PlayerData> LoadAllPlayers()
    {
        List<PlayerData> players = new List<PlayerData>();
        string directoryPath = Application.persistentDataPath;

        foreach (string file in Directory.GetFiles(directoryPath, "*.xml"))
        {
            if (Path.GetFileName(file) != "Levels.xml")
            {
                players.Add(LoadPlayerData(file, Path.GetFileName(file)));
            }
        }

        return players;
    }

    private PlayerData LoadPlayerData(string filePath, string fileName)
    {
        try
        {
            XDocument xmlDoc = XDocument.Load(filePath);
            PlayerData player = new PlayerData
            {
                Name = fileName,
                SkinColor = xmlDoc.Root.Element("SkinColor").Value,
                Hair = xmlDoc.Root.Element("Hair")?.Value,
                Clothing = xmlDoc.Root.Element("Clothing")?.Value,
                Head_Accessory_1 = xmlDoc.Root.Element("HeadAccessory1")?.Value,
                Head_Accessory_2 = xmlDoc.Root.Element("HeadAccessory2")?.Value,
                Clothing_Accessory_1 = xmlDoc.Root.Element("ClothingAccessory1")?.Value,
                Clothing_Accessory_2 = xmlDoc.Root.Element("HeadAccessory2")?.Value,
            };

            return player;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading player data from file " + filePath + ": " + e.Message);
            return null;
        }
    }
    
}

public class PlayerData
{
    public string Name;
    public string Hair;
    public string SkinColor;
    public string Clothing;
    public string Head_Accessory_1;
    public string Head_Accessory_2;
    public string Clothing_Accessory_1;
    public string Clothing_Accessory_2;

}
