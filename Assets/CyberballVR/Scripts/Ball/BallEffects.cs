using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//Made by Christian
public class BallEffects : MonoBehaviour
{
    public int grabCount = 0;
    public int fxChangeInterval;
    ParticleSystem particleSys;
    Outline ballOutline;

    private void Start()
    {
        particleSys = GetComponentInChildren<ParticleSystem>();
        ballOutline = GetComponent<Outline>();

        var main = particleSys.main;
        main.startSpeed = 10;

        UpdateObjectProperties();
    }

    private void OnEnable()
    {
        EventManager.onAISuccessfulCatch += IncrementGrabCount;
        EventManager.onBallDropped += ResetGrabCount;
    }

    private void OnDisable()
    {
        EventManager.onAISuccessfulCatch -= IncrementGrabCount;
        EventManager.onBallDropped -= ResetGrabCount;
    }
    public void IncrementGrabCount()
    {
        if (BallManager.dropped == false)
        {
            // Increment the grab count when the object is grabbed
            grabCount++;

            //(change color, speed of particle effects, etc.)
            UpdateObjectProperties();
        }
        else
            ResetGrabCount();
        
    }

    private void ResetGrabCount()
    {
        grabCount = 0;
        UpdateObjectProperties();
    }

    private void UpdateObjectProperties()
    {
        if (particleSys != null)
        {

            var main = particleSys.main;
            var emission = particleSys.emission;

            if (grabCount < fxChangeInterval)
            {
                main.startColor = Color.red;
                ballOutline.OutlineColor = Color.red;
            }
            else if(grabCount >= fxChangeInterval && grabCount < fxChangeInterval * 2) 
            {
                main.startColor = Color.yellow;
                ballOutline.OutlineColor = Color.yellow;
            }
            else if (grabCount >= fxChangeInterval * 2 && grabCount < fxChangeInterval * 3)
            {
                main.startColor = Color.green;
                ballOutline.OutlineColor = Color.green;
            }
            else if (grabCount >= fxChangeInterval * 3 && grabCount < fxChangeInterval * 4)
            {
                main.startColor = Color.blue;
                ballOutline.OutlineColor = Color.blue;
            }
            else if (grabCount >= fxChangeInterval * 4)
            {
                main.startColor = Color.magenta;
                ballOutline.OutlineColor = Color.magenta;
            }
            
            // Change emmision rate of particle effects based on grab count
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(emission.rateOverTime.constant + (grabCount * 1.25f));   // Adjust speed based on grab count

        }
    }
}
