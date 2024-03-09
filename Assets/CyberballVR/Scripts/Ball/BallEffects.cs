using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//Made by Christian
public class BallEffects : MonoBehaviour
{
    public int grabCount = 0;
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

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Tab)) 
        //{
            
        //    EventManager.onSuccessfulCatch?.Invoke();
            
        //}
    }

    //private void OnEnable()
    //{
    //    EventManager.onSuccessfulCatch += IncrementGrabCount;
    //    EventManager.onBallDropped += ResetGrabCount;
    //}

    //private void OnDisable()
    //{
    //    EventManager.onSuccessfulCatch -= IncrementGrabCount;
    //    EventManager.onBallDropped -= ResetGrabCount;
    //}
    public void IncrementGrabCount()
    {
        if (AI.dropped == false)
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

            if (grabCount < 2)
            {
                main.startColor = Color.red;
                ballOutline.OutlineColor = Color.red;
            }
            else if(grabCount >= 2 && grabCount < 4) 
            {
                main.startColor = Color.yellow;
                ballOutline.OutlineColor = Color.yellow;
            }
            else if (grabCount >= 4 && grabCount < 6)
            {
                main.startColor = Color.green;
                ballOutline.OutlineColor = Color.green;
            }
            else if (grabCount >= 6 && grabCount < 8)
            {
                main.startColor = Color.blue;
                ballOutline.OutlineColor = Color.blue;
            }
            else if (grabCount >= 8)
            {
                main.startColor = Color.magenta;
                ballOutline.OutlineColor = Color.magenta;
            }
            
            // Change emmision rate of particle effects based on grab count
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(emission.rateOverTime.constant + (grabCount * 1.25f));   // Adjust speed based on grab count

        }
    }
}
