using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    CustomizeHandler customizeHandler;

    private Button button;

    private void Start()
    {
        customizeHandler = GameObject.FindObjectOfType<CustomizeHandler>();

        button = GetComponent<Button>();

        if (button)
        {
            button.onClick.AddListener(AddSkinColor);
        }
    }

    void AddSkinColor()
    {
        customizeHandler.GetSkinColorSelection(button.name);
        //Debug.Log("You have clicked the " + button.name + " button!");
    }
}
