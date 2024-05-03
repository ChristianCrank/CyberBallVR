using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.IO;
using System;

public class ResearchData : MonoBehaviour
{
    public static List<PlayerData> AIPlayers;

    [SerializeField]
    public static LevelData LevelData { get; private set; }

    public static bool isLevelDataLoaded;
    private void Awake()
    {
        AIPlayers = LoadAllPlayers();
        LevelData = LoadLevelData();
    }


    public List<PlayerData> LoadAllPlayers()
    {
        List<PlayerData> players = new List<PlayerData>();
        string directoryPath = Application.persistentDataPath;

        foreach (string file in Directory.GetFiles(directoryPath, "*.xml"))
        {
            if (Path.GetFileName(file) != "Level.xml")
            {
                players.Add(LoadPlayerData(file, Path.GetFileName(file)));
            }
            Debug.Log("player: " + Path.GetFileName(file) + ", ");
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
                Name = fileName.Split(".")[0],
                SkinColor = xmlDoc.Root.Element("SkinColor").Value,
                Hair = xmlDoc.Root.Element("Hair")?.Value,
                Clothing = xmlDoc.Root.Element("Clothing")?.Value,
                Accessory_1 = xmlDoc.Root.Element("Accessories1")?.Value,
                Accessory_2 = xmlDoc.Root.Element("Accessories2")?.Value,
                //Clothing_Accessory_1 = xmlDoc.Root.Element("ClothingAccessory1")?.Value,
                //Clothing_Accessory_2 = xmlDoc.Root.Element("HeadAccessory2")?.Value,
            };

            return player;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading player data from file " + filePath + ": " + e.Message);
            return null;
        }
    }

    private LevelData LoadLevelData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Level.xml");
        Debug.Log("Loading Level Data from file: " + filePath);

        XDocument xmlDoc;
        try
        {
            xmlDoc = XDocument.Load(filePath);
            isLevelDataLoaded = true;
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load XML Document: " + e.Message);
            isLevelDataLoaded = false;
            return null;
        }

        LevelData levelData = new LevelData();

        // Parse NoOfThrows
        XElement noOfThrowsElement = xmlDoc.Root.Element("NoOfThrows");
        if (noOfThrowsElement != null)
        {
            levelData.NoOfThrows = int.Parse(noOfThrowsElement.Value);
            Debug.Log("Parsed NoOfThrows: " + levelData.NoOfThrows);
        }
        else
        {
            Debug.LogWarning("NoOfThrows element is missing in the XML.");
        }

        // Parse ChancesToPlayer
        int cumulativeThrows = 0;
        levelData.ChancesToPlayer.Clear();
        XElement root = xmlDoc.Root.Element("ChancesToPlayer");
        foreach (var element in root.Value.Split(';'))
        {
            if (!string.IsNullOrWhiteSpace(element))
            {
                var parts = element.Split(',');
                int throwCount = int.Parse(parts[0]); // Throws 
                float chanceValue = float.Parse(parts[1]); // Chance 

                cumulativeThrows += throwCount;
                levelData.ChancesToPlayer.Add(new ChanceToPlayer { Throws = cumulativeThrows, Chance = chanceValue });

                Debug.Log($"Loaded chance {chanceValue}% for up to {cumulativeThrows} throws.");
            }
        }

        // Parse Speeds
        cumulativeThrows = 0;
        levelData.Speeds.Clear();
        root = xmlDoc.Root.Element("Speeds");

        foreach (var element in root.Value.Split(';'))
        {
            if (!string.IsNullOrWhiteSpace(element))
            {
                var parts = element.Split(',');
                int throwCount = int.Parse(parts[0]); // Throws
                float speedValue = float.Parse(parts[1]); // Speed

                cumulativeThrows += throwCount;
                levelData.Speeds.Add(new Speed { Throws = cumulativeThrows, SpeedValue = speedValue });

                Debug.Log($"Loaded speed {speedValue} for up to {cumulativeThrows} throws.");
            }
        }

        foreach (var speed in levelData.Speeds)
        {
            Debug.Log($"Speed setting: {speed.Throws} throws at {speed.SpeedValue} speed");
        }
        Debug.Log("Speed size " + levelData.Speeds.Count);
        return levelData;
    }
    
}

public class PlayerData
{
    public string Name;
    public string Hair;
    public string SkinColor;
    public string Clothing;
    public string Accessory_1;
    public string Accessory_2;
    //public string Clothing_Accessory_1;
    //public string Clothing_Accessory_2;

}

public class LevelData
{
    public int NoOfThrows { get; set; }
    public List<ChanceToPlayer> ChancesToPlayer { get; set; }
    public List<Speed> Speeds { get; set; }

    public LevelData()
    {
        ChancesToPlayer = new List<ChanceToPlayer>();
        Speeds = new List<Speed>();
    }
}

public class ChanceToPlayer
{
    public int Throws { get; set; }
    public float Chance { get; set; }
}

public class Speed
{
    public int Throws { get; set; }
    public float SpeedValue { get; set; }
}
