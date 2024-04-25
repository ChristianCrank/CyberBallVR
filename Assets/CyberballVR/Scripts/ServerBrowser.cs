using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ServerBrowser : MonoBehaviour
{
    private int currentLevel = -1;
    public GameManager gameManager;
    public GameObject startButton;
    public GameObject findingLobby;
    public GameObject foundLobby;
    public GameObject cancelButton;

    public FadeToBlack fadeScript;

    public void StartButton()
    {
        StartCoroutine(start());
    }

    public void CancelButton()
    {
        StopCoroutine(start());
        fadeScript.cancel();
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
        int wait = Random.Range(4, 10);
        fadeScript.fadeToBlack("Joining Lobby", wait);
        yield return new WaitForSeconds(wait);

        gameManager.StartGame();
        foundLobby.SetActive(false);
        cancelButton.SetActive(false);
        startButton.SetActive(true);
        yield return null;
    }
}
