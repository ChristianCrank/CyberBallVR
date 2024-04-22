using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerBrowser : MonoBehaviour
{
    private int currentLevel = -1;
    public GameManager gameManager;
    public GameObject startButton;
    public GameObject findingLobby;
    public GameObject foundLobby;
    public GameObject cancelButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartButton()
    {
        StartCoroutine(start());
    }

    public void CancelButton()
    {
        StopCoroutine(start());
        currentLevel--;
    }

    public IEnumerator start()
    {
        currentLevel++;
        startButton.SetActive(false);
        findingLobby.SetActive(true);
        cancelButton.SetActive(true);
        yield return new WaitForSeconds(Random.Range(2, 5));
        findingLobby.SetActive(false);
        foundLobby.SetActive(true);
        yield return new WaitForSeconds(Random.Range(1, 3));
        gameManager.StartGame();
        foundLobby.SetActive(false);
        cancelButton.SetActive(false);
        startButton.SetActive(true);
        yield return null;
    }
}
