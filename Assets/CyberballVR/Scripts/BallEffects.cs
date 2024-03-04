using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//Made by Christian
public class BallEffects : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;


    public int grabCount = 0;
    ParticleSystem particleSystem;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        particleSystem = GetComponentInChildren<ParticleSystem>();

        var main = particleSystem.main;
        main.startSpeed = 10;

        UpdateObjectProperties();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Plus)) 
        {
            EventManager.onSuccessfulCatch += IncrementGrabCount;
            EventManager.onSuccessfulCatch.Invoke();
            EventManager.onSuccessfulCatch -= IncrementGrabCount;
        }
    }
    private void IncrementGrabCount()
    {
        // Increment the grab count when the object is grabbed
        grabCount++;

        // Do something with the grab count (change color, speed of particle effects, etc.)
        UpdateObjectProperties();
    }

    private void UpdateObjectProperties()
    {
        if (particleSystem != null)
        {

            var main = particleSystem.main;

            if(grabCount < 2)
            { 
                main.startColor = Color.white;
            }
            else if(grabCount >= 2 && grabCount < 4) 
            { 
                main.startColor = Color.red;
            }
            else if (grabCount >= 4 && grabCount < 6)
            {
                main.startColor = Color.yellow;
            }
            else if (grabCount >= 6 && grabCount < 8)
            {
                main.startColor = Color.green;
            }
            else if (grabCount >= 8)
            {
                main.startColor = Color.blue;
            }
            // Example: Change speed of particle effects based on grab count

          
            main.startSpeed = main.startSpeed.constant + (grabCount ); // Adjust speed based on grab count

        }
    }
}
