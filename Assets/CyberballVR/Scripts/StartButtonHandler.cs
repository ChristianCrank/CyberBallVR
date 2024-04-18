using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonHandler : MonoBehaviour
{
    GameManager gameManager;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        button = GetComponent<Button>();

        if (button)
        {
            button.onClick.AddListener(gameManager.StartGame);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
