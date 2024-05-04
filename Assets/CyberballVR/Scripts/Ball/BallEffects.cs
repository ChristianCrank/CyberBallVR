using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

//Made by Christian
public class BallEffects : MonoBehaviour
{
    public int grabCount = 0;
    public int fxChangeInterval;
    ParticleSystem particleSys;
    Outline ballOutline;
    public AudioSource ballStreak;
    
    private float tempPitch;

    private void Start()
    {
        particleSys = GetComponentInChildren<ParticleSystem>();
        ballOutline = GetComponent<Outline>();
        if(SceneManager.GetActiveScene().ToString() != "Tutorial") tempPitch = ballStreak.pitch;

        var main = particleSys.main;
        main.startSpeed = 10;

        UpdateObjectProperties(false);
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
            UpdateObjectProperties(true);
        }
        else
        {
            ResetGrabCount();
        }
            
        
    }

    public void ResetGrabCount()
    {
        grabCount = 0;
        
        UpdateObjectProperties(false);
    }

    private void UpdateObjectProperties(bool sound)
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
            else if(grabCount == fxChangeInterval) 
            {
                main.startColor = Color.yellow;
                ballOutline.OutlineColor = Color.yellow;
                if(sound)
                {
                    ballStreak.pitch = tempPitch;
                    ballStreak.Play();
                }
                    

            }
            else if (grabCount == fxChangeInterval * 2)
            {
                main.startColor = Color.green;
                ballOutline.OutlineColor = Color.cyan;
                if (sound)
                {
                    ballStreak.pitch += .25f;
                    ballStreak.Play();
                }
                    
            }
            else if (grabCount == fxChangeInterval * 3)
            {
                main.startColor = Color.blue;
                ballOutline.OutlineColor = Color.blue;
                if (sound)
                {
                    ballStreak.pitch += .25f;
                    ballStreak.Play();
                
                }
            }
            else if (grabCount == fxChangeInterval * 4)
            {
                main.startColor = Color.magenta;
                ballOutline.OutlineColor = Color.magenta;
                if (sound)
                {
                    ballStreak.pitch += .25f;
                    ballStreak.Play();
                   
                }
            }
            
            // Change emmision rate of particle effects based on grab count
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(emission.rateOverTime.constant + (grabCount * 1.25f));   // Adjust speed based on grab count

        }
    }
}
