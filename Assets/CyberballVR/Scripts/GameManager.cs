using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Made by Christian
public class GameManager : MonoBehaviour
{
    public GameObject[] levelPrefabs;
    public GameObject currentLevel;
    public int currentLevelSelect;

    public GameObject AICharacter;
    public GameObject Player;
    public List<GameObject> playerList;

    public static GameObject currentBallHolder;

    private void Start()
    {
        currentBallHolder = Player;
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
                Player.transform.position = child.transform.position;
                playerList.Add(Player);
            }
            else if (child.tag == "AISpawn")
            {
                GameObject go = Instantiate(AICharacter, child.transform.position + new Vector3(0, .75f, 0), Quaternion.identity);
                playerList.Add(go);

            }

        }
    }

    public void OnPlayerCatch()
    {
        currentBallHolder = Player;
    }
}
