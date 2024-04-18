using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR.Interaction.Toolkit;

//Made by Christian
public class GameManager : MonoBehaviour
{
    public GameObject[] levelPrefabs;
    public BallManager ballManager;
    public GameObject currentLevel;
    public XRGrabInteractable ball;
    public int currentLevelSelect;
    public float spawnRadius;

    public GameObject AICharacter;
    public GameObject Player;
    public int playerCatchCount;
    public List<GameObject> playerList;
    public Transform houseSpawn;
    public GameObject playerMove;
    public static GameObject currentBallHolder;
    public GameObject highestCatchPlayer;
    public GameObject lowestCatchPlayer;

    Outline playerOutline;

    private void OnEnable()
    {
        EventManager.onAISuccessfulCatch += ChangePlayerOutline;

    }

    private void OnDisable()
    {
        EventManager.onAISuccessfulCatch -= ChangePlayerOutline;
    }

    private void Start()
    {
        SpawnHumanPlayerInHouse();

        if (ResearchData.AIPlayers != null || ResearchData.AIPlayers.Count != 0)
        {
            foreach (PlayerData playerData in ResearchData.AIPlayers)
            {
                SetupAIPlayers(playerData);
            }
        }
       
    }

    /// <summary>
    /// Call this to start the game after player selects server in the house
    /// </summary>
    public void StartGame()
    {
        currentBallHolder = Player;
        playerCatchCount = 0;

        if (ResearchData.AIPlayers != null || ResearchData.AIPlayers.Count != 0)
        {
            SpawnAllPlayers();
        }
        else
        {
            if (levelPrefabs != null)
            {
                switch (currentLevelSelect)
                {
                    case 0:
                        currentLevel = levelPrefabs[0];
                        break;
                    case 1:
                        currentLevel = levelPrefabs[1];
                        break;
                    case 2:
                        currentLevel = levelPrefabs[2];
                        break;
                }

                SpawnPlayersNoData();
            }
            else if (currentLevel == null)
            {
                Debug.Log("current level is not selected in inspector...defaulting at level 1");
                currentLevel = levelPrefabs[0];

                SpawnPlayersNoData();

            }
            else
            {
                Debug.Log("LevelPrefabs is null");
            }
        }
    }

    private void SetupAIPlayers(PlayerData data)
    {
        GameObject go = AICharacter;
        AICustomize customization = go.GetComponent<AICustomize>();

        if(data != null)
        {
            customization.ReadInCustomization(data);
        }
        else
        {
            Debug.Log("No customizaiton data found, randomizing appearance");
            customization.RandomizeCustomization();
        }

        playerList.Add(go);

    }

    private void SpawnHumanPlayerInHouse()
    {
        Player.transform.position = houseSpawn.position;
        playerMove.SetActive(true);
        playerList.Add(Player);
        Player.GetNamedChild("PlayerStand").SetActive(false);
    }

    private void SpawnHumanPlayerInField(Vector3 pos)
    {
        Player.transform.position = pos;
        playerMove.SetActive(false);
        Player.GetNamedChild("PlayerStand").SetActive(true);
        ballManager.SetupBall();
    }
    private void SpawnAllPlayers()
    {
        int playerCount = playerList.Count;

        if (playerCount == 2)
        {
            // Spawn players straight across from each other
            SpawnHumanPlayerInField(new Vector3(spawnRadius, 0, 0));
            Instantiate(playerList[1], new Vector3(-spawnRadius, 0.75f, 0), Quaternion.identity);
        }
        else if (playerCount > 2)
        {
            SpawnHumanPlayerInField(new Vector3(spawnRadius, 0, 0));
            for (int i = 0; i < playerCount; i++)
            {
                float angle = i * Mathf.PI * 2f / playerCount;
                Vector3 spawnPosition = new Vector3(Mathf.Cos(angle) * spawnRadius, 0, Mathf.Sin(angle) * spawnRadius);
                Quaternion spawnRotation = Quaternion.Euler(0, -angle * Mathf.Rad2Deg + 90, 0); // Orient towards center

                if (i == 0)
                {
                    SpawnHumanPlayerInField(spawnPosition);
                }
                else
                    Instantiate(playerList[i], spawnPosition, spawnRotation);
            }
        }

        foreach (GameObject go in playerList)
        {
            if (go.GetComponent<Outline>() != null)
            {
                go.GetComponent<Outline>().enabled = false;
            }
            else if (go.GetComponentInChildren<Outline>() != null)
            {
                go.GetComponentInChildren<Outline>().enabled = false;
            }
        }
    }

    private void SpawnPlayersNoData()
    {
        Instantiate(currentLevel);

        foreach (Transform child in currentLevel.transform)
        {
            if (child.tag == "PlayerSpawn")
            {
                Player.transform.position = child.transform.position + new Vector3(0, 0, 0); //Player podium
                playerList.Add(Player);
                playerMove.SetActive(true);
            }
            else if (child.tag == "AISpawn")
            {
                GameObject go = Instantiate(AICharacter, child.transform.position + new Vector3(0, .75f, 0), Quaternion.identity);
                ballManager.SetupBall();
                playerList.Add(go);
            }

        }

        foreach (GameObject go in playerList)
        {
            if (go.GetComponent<Outline>() != null)
            {
                go.GetComponent<Outline>().enabled = false;
            }
            else if (go.GetComponentInChildren<Outline>() != null)
            {
                go.GetComponentInChildren<Outline>().enabled = false;
            }
        }
    }

    public void OnPlayerCatch()
    {
        currentBallHolder = Player;
        if(BallManager.dropped == false)
        {
            playerCatchCount++;
            ChangePlayerOutline();
        }
        
    }

    public void ChangePlayerOutline()
    {
        int highestCatchCount = 0;
        int lowestCatchCount = 1;
        highestCatchPlayer = null; // This will hold the reference to the player/AI with the highest catchCount
        lowestCatchPlayer = null; // This will hold the reference to the player/AI with the lowest catchCount

        foreach (GameObject go in playerList)
        {
            if(go.GetComponent<Outline>() != null)
            {
                go.GetComponent<Outline>().enabled = false;
            }
            else
            {
                if (go.name == "Player" && go.GetComponentInChildren<Outline>() != null)
                {
                    go.GetComponentInChildren<Outline>().enabled = false;
                }
            }
        }

        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i] != null)
            {
                AI aiComponent = playerList[i].GetComponent<AI>();

                if (aiComponent != null)
                {
                    // Update tempCatch if the current AI's catchCount is greater
                    if (aiComponent.catchCount > highestCatchCount)
                    {
                        highestCatchCount = aiComponent.catchCount;
                        highestCatchPlayer = playerList[i];
                    }

                    if(aiComponent.catchCount < lowestCatchCount)
                    {
                        lowestCatchCount = aiComponent.catchCount;
                        lowestCatchPlayer = playerList[i];
                    }

                }
                else
                {
                    if (playerCatchCount > highestCatchCount)
                    {
                        highestCatchCount = playerCatchCount;
                        highestCatchPlayer = playerList[i];
                    }

                    if(playerCatchCount < lowestCatchCount)
                    {
                        lowestCatchCount= playerCatchCount;
                        lowestCatchPlayer = playerList[i];
                    }
                }
         
            }
        }

        if (highestCatchPlayer != null && highestCatchPlayer.GetComponent<Outline>() != null && highestCatchCount != playerCatchCount)
            CatchOutline(false, true);
        else if(highestCatchPlayer != null && highestCatchPlayer.GetComponentInChildren<Outline>() != null && highestCatchCount == playerCatchCount)
            CatchOutline(true, true);
        if(lowestCatchPlayer != null && lowestCatchPlayer.GetComponent<Outline>() != null && lowestCatchCount != playerCatchCount)
            CatchOutline(false, false);
        else if (lowestCatchPlayer != null && lowestCatchPlayer.GetComponentInChildren<Outline>() != null && lowestCatchCount == playerCatchCount)
            CatchOutline(true, false);

    }

    /// <summary>
    /// If p is true, outlines player
    /// If p is false, outlines AI
    /// If c is true, outlines blue
    /// If c is false, outlines red
    /// </summary>
    /// <param name="p"></param>
    void CatchOutline(bool p, bool c)
    {
        if(p)
        {
            playerOutline = highestCatchPlayer.GetComponentInChildren<Outline>();
            playerOutline.enabled = true;
            playerOutline.OutlineWidth = 5f;

            if(c)
                playerOutline.OutlineColor = Color.blue;
            else 
                playerOutline.OutlineColor = Color.red;
        }
        else
        {
            playerOutline = highestCatchPlayer.GetComponent<Outline>();
            playerOutline.enabled = true;
            playerOutline.OutlineWidth = 3f;

            if(c)
                playerOutline.OutlineColor = Color.blue;
            else 
                playerOutline.OutlineColor = Color.red;
        }
    }

    
}
    


