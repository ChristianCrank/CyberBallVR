using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Made by Christian
public class GameManager : MonoBehaviour
{
    public GameObject[] levelPrefabs;
    public GameObject currentLevel;
    public int currentLevelSelect;

    public GameObject AICharacter;
    public GameObject Player;
    public int playerCatchCount;
    public List<GameObject> playerList;

    public static GameObject currentBallHolder;
    public GameObject highestCatchPlayer;

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
        currentBallHolder = Player;
        playerCatchCount = 0;

        
    }

    private void Awake()
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

            InstantiateCharacters();
        }
        else if (currentLevel == null)
        {
            Debug.Log("current level is not selected in inspector...defaulting at level 1");
            currentLevel = levelPrefabs[0];

            InstantiateCharacters();

        }
        else
        {
            Debug.Log("LevelPrefabs is null");
        }
    }

    private void InstantiateCharacters()
    {
        Instantiate(currentLevel);

        foreach (Transform child in currentLevel.transform)
        {
            if (child.tag == "PlayerSpawn")
            {
                Player.transform.position = child.transform.position + new Vector3(0, 0, 0.5f);
                playerList.Add(Player);
            }
            else if (child.tag == "AISpawn")
            {
                GameObject go = Instantiate(AICharacter, child.transform.position + new Vector3(0, .75f, 0), Quaternion.identity);
                playerList.Add(go);
                

            }

        }

        foreach (GameObject go in playerList)
        {
            if (go.GetComponent<Outline>() != null)
            {
                go.GetComponent<Outline>().enabled = false;
            }
            else if(go.GetComponentInChildren<Outline>() != null)
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
        highestCatchPlayer = null; // This will hold the reference to the player/AI with the highest catchCount

        foreach(GameObject go in playerList)
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

                    
                }
                else
                {
                    if (playerCatchCount > highestCatchCount)
                    {
                        highestCatchCount = playerCatchCount;
                        highestCatchPlayer = playerList[i];
                    }
                }
         
            }
        }

        if (highestCatchPlayer != null && highestCatchPlayer.GetComponent<Outline>() != null && highestCatchCount != playerCatchCount)
        {
            playerOutline = highestCatchPlayer.GetComponent<Outline>();
            playerOutline.enabled = true;
            playerOutline.OutlineWidth = 3f;
            playerOutline.OutlineColor = Color.green;
        }
        else if(highestCatchPlayer != null && highestCatchPlayer.GetComponentInChildren<Outline>() != null && highestCatchCount == playerCatchCount)
        {
            playerOutline = highestCatchPlayer.GetComponentInChildren<Outline>();
            playerOutline.enabled = true;
            playerOutline.OutlineWidth = 5f;
            playerOutline.OutlineColor = Color.green;
        }
    }
}
