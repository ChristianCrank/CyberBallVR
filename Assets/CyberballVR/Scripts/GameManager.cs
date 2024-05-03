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
    [Range(-1, 2)] public int currentLevelSelect;
    public float spawnRadius;
    public Transform AllSpawnPosition;

    public GameObject AICharacter;
    public GameObject Player;
    private CharacterController characterController;

    public int playerCatchCount;
    public List<GameObject> playerList;
    public Transform houseSpawn;
    public GameObject playerMove;
    public GameObject[] handRays;
    public static GameObject currentBallHolder;
    public GameObject highestCatchPlayer;
    public GameObject lowestCatchPlayer;

    public FadeToBlack fadeScript;


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
        characterController = Player.GetComponentInChildren<CharacterController>();
        SpawnHumanPlayerInHouse();
        
        if (ResearchData.AIPlayers != null && ResearchData.AIPlayers.Count != 0)
        {
            foreach (PlayerData playerData in ResearchData.AIPlayers)
            {
                SetupAIPlayers(playerData);
            }
        }

        if (ResearchData.AIPlayers == null) Debug.Log("ReaseaschData.AIPlayers is null");
        Debug.Log(ResearchData.AIPlayers.Count);
       
    }

    /// <summary>
    /// Call this to start the game after player selects server in the house
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Starting Game");
        currentBallHolder = Player;
        playerCatchCount = 0;

        if (ResearchData.AIPlayers != null && ResearchData.AIPlayers.Count != 0)
        {
            SpawnAllPlayers();
        }
        else
        {
            if (levelPrefabs != null)
            {
                Debug.Log("Level Prefabs is null");
          
                if (currentLevelSelect != -1) currentLevel = levelPrefabs[currentLevelSelect];

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
        Debug.Log("Setting Up AI Players");
        GameObject aiPlayerInstance = Instantiate(AICharacter);

        aiPlayerInstance.SetActive(false);

        AICustomize customization = aiPlayerInstance.GetComponent<AICustomize>();

        if(data != null)
        {
            customization.ReadInCustomization(data);
        }
        else
        {
            Debug.Log("No customizaiton data found, randomizing appearance");
            customization.RandomizeCustomization();
        }

        playerList.Add(aiPlayerInstance);

    }

    public IEnumerable returnPlayerToHouse() //called from the ThrowBall() in AI.cs
    {
        Debug.Log("ReturnedPlayerToHouse");
        fadeScript.fadeToBlack("Lobby Closing...", 3f);
        yield return new WaitForSeconds(3f);
        Player.transform.position = houseSpawn.position;
        playerMove.SetActive(true);
        //playerList.Add(Player); Just disabled this on a hunch
        Player.transform.Find("PlayerStand").gameObject.SetActive(false);
    }

    private void SpawnHumanPlayerInHouse()
    {
        Debug.Log("SpawnHumanPlayerInHouse");
        Player.transform.position = houseSpawn.position;
        playerMove.SetActive(true);
        playerList.Add(Player);
        Player.transform.Find("PlayerStand").gameObject.SetActive(false);
    }

    private void SpawnHumanPlayerInField(Vector3 pos, Quaternion rot)
    {
        Debug.Log("SpawnHumanPlayerInField");
        Player.transform.position = pos;
        characterController.transform.position = pos;
        Player.GetNamedChild("Jimmy").transform.position = pos;

        //foreach(Transform t in Player.GetComponentsInChildren<Transform>())
        //{
        //    if(t.tag != "BallSpawn")
        //    {
        //        t.position = pos;
        //    }
        //}
        Player.transform.rotation = rot;
        characterController.transform.rotation = rot;
        playerMove.SetActive(false);
        foreach(GameObject hr in handRays)
        {
            hr.gameObject.SetActive(false);
        }
        Player.transform.Find("PlayerStand").gameObject.SetActive(true);   //.GetNamedChild("PlayerStand").SetActive(true);
        StartCoroutine(SetupBallGM());
    }
   
    public void DisableHandRays()
    {
        foreach (GameObject hr in handRays)
        {
            hr.gameObject.SetActive(false);
        }
    }

    IEnumerator SetupBallGM()
    {
        yield return new WaitForSeconds(1f);
        ballManager.SetupBall();
    }

    private void SpawnAllPlayers()
    {
        int playerCount = playerList.Count;
        Debug.Log("SpawnAllPlayers with player count: " + playerCount);

        // Loop to instantiate and place all AI players
        for (int i = 0; i < playerCount; i++)
        {
            float angle = i * Mathf.PI * 2f / playerCount;
            Vector3 spawnPosition = AllSpawnPosition.position + new Vector3(Mathf.Cos(angle) * spawnRadius, 0, Mathf.Sin(angle) * spawnRadius);
            
            
            //Quaternion spawnRotation = Quaternion.Euler(0, -angle * Mathf.Rad2Deg + 90, 0); // Orient towards center
            Quaternion spawnRotation;
            if (i == 0) 
            {
                spawnRotation = Quaternion.Euler(0, -angle * Mathf.Rad2Deg - 90, 0);
            }
            else
            {
                spawnRotation = Quaternion.Euler(0, -angle * Mathf.Rad2Deg + 90, 0);
            }
            if (i == 0)
            {
                // handling for human player
                SpawnHumanPlayerInField(spawnPosition, spawnRotation);
            }
            else
            {
                //Spawns the AI players 
                GameObject aiPlayerInstance = playerList[i];
                aiPlayerInstance.transform.position = spawnPosition + new Vector3(0, .75f, 0);
                aiPlayerInstance.transform.rotation = spawnRotation;
                aiPlayerInstance.SetActive(true);
            }
        }

        DisableOutlines();

    }
    private void SpawnPlayersNoData()
    {
        Debug.Log("SpawnPlayersNoData");
        Instantiate(currentLevel);

        foreach (Transform child in currentLevel.transform)
        {
            if (child.tag == "PlayerSpawn")
            {
                Debug.Log("player spawned with no data");
                //characterController.transform.position = child.transform.position + new Vector3(0, 0, 0); //Player podium
                Player.transform.position = child.transform.position + new Vector3(0, 0, 0);
                playerList.Add(Player);
                playerMove.SetActive(false);
                Player.transform.Find("PlayerStand").gameObject.SetActive(true);

            }
            else if (child.tag == "AISpawn")
            {
                GameObject go = Instantiate(AICharacter, child.transform.position + new Vector3(0, .75f, 0), Quaternion.identity);
                ballManager.SetupBall();
                playerList.Add(go);
                go.SendMessage("RandomizeCustomization");
            }

        }

        DisableOutlines();
        
    }

    private void DisableOutlines()
    {
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

    public int TrackAllCatches()
    {
        int count = 0;
        foreach(GameObject go in playerList) 
        {
            if(go != Player)
            {
                count += go.GetComponent<AI>().catchCount;
            }
        }

        count += playerCatchCount;

        return count;
    }
    public void ChangePlayerOutline()
    {
        int highestCatchCount = 0;
        int lowestCatchCount = int.MaxValue;
        highestCatchPlayer = null; 
        lowestCatchPlayer = null; 

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

        foreach (GameObject go in playerList)
        {
            AI aiComponent = go.GetComponent<AI>();

            //int catchCount = aiComponent != null ? aiComponent.catchCount : playerCatchCount;
            int catchCount = (go == Player) ? playerCatchCount : go.GetComponent<AI>().catchCount;

            if (catchCount > highestCatchCount)
            {
                highestCatchCount = catchCount;
                highestCatchPlayer = go;
            }

            if (catchCount < lowestCatchCount)
            {
                lowestCatchCount = catchCount;
                lowestCatchPlayer = go;
            }
        }

        ApplyOutline(highestCatchPlayer, Color.blue);
        if (highestCatchPlayer != lowestCatchPlayer)
        {
            ApplyOutline(lowestCatchPlayer, Color.red);
        }
    }

    private void ApplyOutline(GameObject player, Color color)
    {
        if (player != null)
        {
            Outline outline = player.GetComponent<Outline>() ?? player.GetComponentInChildren<Outline>();
            if (outline != null)
            {
                Debug.Log("outline parent " + outline.gameObject.name);
                outline.enabled = true;
                outline.OutlineColor = color;
                outline.OutlineWidth = player == Player ? 5f : 2f; // Wider outline for the human player
            }
        }
    }


}
    


