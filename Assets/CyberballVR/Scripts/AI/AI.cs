using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


//Made by Christian
public class AI : MonoBehaviour
{
    GameManager gameManager;
    GameObject ball;
    [SerializeField]
    GameObject target;
    Quaternion targetRotation;
    public Transform ballSpawn;

    public float launchAngle;
    public float rotationSpeed = 5.0f;

    public int catchCount;
    public enum TargetingPreference
    {
        Random, 
        FavorAI,
        FavorPlayer
    }
    public TargetingPreference targetingPreference;

    private void Start()
    {
        launchAngle = 25f;
        targetingPreference = TargetingPreference.Random;
        catchCount = 0;
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Ball"))
        {
            ball = other.gameObject;
            AICatch(ball);
        }
    }

    public void AICatch(GameObject b)
    {
        
        Debug.Log("AI is ball parent");
        ball = b;
        ball.transform.position = ballSpawn.position;
        ball.transform.SetParent(this.gameObject.transform);
        GameManager.currentBallHolder = this.gameObject;
        catchCount++;

        EventManager.onAISuccessfulCatch?.Invoke();

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        StartCoroutine(ThrowBall());
    }

    private void FixedUpdate()
    {
        

        if (GameManager.currentBallHolder != null)
        {
            // Look at the current ball holder if it's not this AI
            if (GameManager.currentBallHolder != this.gameObject)
            {
                targetRotation = Quaternion.LookRotation(GameManager.currentBallHolder.transform.position - this.transform.position);
                this.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                //Instant rotation:
                //this.transform.LookAt(GameManager.currentBallHolder.transform);
            }
            else if (target != null) // If this AI has the ball and a target is set, look at the target
            {
                targetRotation = Quaternion.LookRotation(target.transform.position - this.transform.position);
                this.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                //Instant rotation:
                //this.transform.LookAt(target.transform);
            }
        }
        else
        {
            // Fallback: Look at a default, e.g., the first player in the list, if no ball holder is defined
            targetRotation = Quaternion.LookRotation(gameManager.playerList[0].transform.position - this.transform.position);
            this.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            //Instant rotation:
            //this.transform.LookAt(gameManager.playerList[0].transform);
        }
    }

    private IEnumerator ThrowBall()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(.25f, .75f));

        target = ChooseThrowTarget();
        
        yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 2f));

        if (ball != null)
        {
            ball.transform.SetParent(null);
            BallManager.dropped = false;
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            

            if(target != null)
            { 

                Vector3 startPos = ball.transform.position;
                Vector3 targetPos = target.transform.position;
                
                // Calculate the distance to the target in the horizontal plane
                float distance = Vector3.Distance(new Vector3(startPos.x, 0, startPos.z), new Vector3(targetPos.x, 0, targetPos.z));

                // Calculate the difference in height between the start position and the target position
                float yOffset = targetPos.y - startPos.y;

                float gravity = Physics.gravity.magnitude;

                // Calculate initial velocity needed to achieve the desired launch angle
                float initialVelocity = CalculateLaunchVelocity(distance, yOffset, gravity, launchAngle);

                // Calculate velocity components
                Vector3 velocity = new Vector3(targetPos.x - startPos.x, 0, targetPos.z - startPos.z).normalized * initialVelocity;
                velocity.y = initialVelocity * Mathf.Sin(launchAngle * Mathf.Deg2Rad);

                // Introduce error to the throw
                float errorMagnitude = 1f; //Change this to increase or decrease throw error
                if (UnityEngine.Random.value < 0.2f) //Change the to increase or decrease percent chance of an error happening
                {
                    Vector3 error = new Vector3(UnityEngine.Random.Range(-errorMagnitude, errorMagnitude),
                                                 UnityEngine.Random.Range(-errorMagnitude, errorMagnitude),
                                                 UnityEngine.Random.Range(-errorMagnitude, errorMagnitude));

                    velocity += error;
                }

                rb.velocity = velocity;


            }

            ball = null;
        }
    }
    // Helper method to calculate the launch velocity based on distance, height difference, gravity, and launch angle
    float CalculateLaunchVelocity(float distance, float yOffset, float gravity, float launchAngle)
    {
        float angleRad = launchAngle * Mathf.Deg2Rad;
        float vSquared = (gravity * distance * distance) / (2 * (yOffset - distance * Mathf.Tan(angleRad)) * Mathf.Pow(Mathf.Cos(angleRad), 2));
        return Mathf.Sqrt(Mathf.Abs(vSquared));
    }


    private GameObject ChooseThrowTarget()
    {
        
        float chanceThreshold = 0.8f; // 80%
        float roll = UnityEngine.Random.Range(0f, 1f); // Random roll between 0 and 1

        List<GameObject> potentialTargets = new List<GameObject>(gameManager.playerList);
        potentialTargets.Remove(this.gameObject); // Remove this AI from the list of potential targets

        switch (targetingPreference)
        {
            case TargetingPreference.Random:
                // Return a random player GameObject
                if (potentialTargets.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, potentialTargets.Count);
                    return potentialTargets[randomIndex];
                }
                break;
            case TargetingPreference.FavorAI:
                // Implement logic to favor AI, but with a 20% chance to throw to the player
                if (potentialTargets.Count > 0)
                {
                    if (roll < chanceThreshold)
                    {
                        // Favor throw to AI 80%
                        int randomIndex = UnityEngine.Random.Range(1, potentialTargets.Count);
                            return potentialTargets[randomIndex];
                        
                    }
                    else
                    {
                        // With 20% chance, select the player target instead
                        return potentialTargets[0];
                    }
                }
                break;
            case TargetingPreference.FavorPlayer:
                // Similar to FavorAI, but primarily targets the player, with a 20% chance for a throw to the AI
                if (potentialTargets.Count > 0)
                {
                    if (roll < chanceThreshold)
                    {
                        // Directly target the player
                        return potentialTargets[0];
                    }
                    else
                    {
                        // With 20% chance, excludes playerList[0]
                        int randomIndex = UnityEngine.Random.Range(1, potentialTargets.Count);
                            return potentialTargets[randomIndex];
                    }
                }
                break;
        }

        return null; 
    }
}
