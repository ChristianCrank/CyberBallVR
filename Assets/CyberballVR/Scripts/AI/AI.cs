using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Linq;


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
        //targetingPreference = TargetingPreference.Random;
        catchCount = 0;
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Debug.Log("RD " + ResearchData.isLevelDataLoaded);
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
        if (GameManager.currentBallHolder != gameObject)
        {
            string name = transform.GetComponent<AICustomize>().UIName.text;
            ResearchData.catchList.Add(" to " + name);
            ResearchData.throwList.Add(name + " threw the ball");
            catchCount++;
        }

        Debug.Log("AI is ball parent");
        ball = b;
        ball.transform.position = ballSpawn.position;
        ball.transform.SetParent(this.gameObject.transform);
        GameManager.currentBallHolder = this.gameObject;

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

        yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 3f));

        if (ResearchData.AIPlayers != null && ResearchData.AIPlayers.Count != 0)
        {
            if (gameManager.TrackAllCatches() < ResearchData.LevelData.NoOfThrows)
            {
                if (ball != null)
                {
                    ball.transform.SetParent(null);
                    BallManager.dropped = false;
                    Rigidbody rb = ball.GetComponent<Rigidbody>();
                    rb.isKinematic = false;

                    float speedMultiplier = GetCurrentThrowSpeed();

                    if (speedMultiplier <= .5)
                        launchAngle = 75f;
                    else if (speedMultiplier > .5 && speedMultiplier <= .75)
                        launchAngle = 55f;
                    else if (speedMultiplier > .75 && speedMultiplier <= 1)
                        launchAngle = 25f;
                    else if (speedMultiplier > 1 && speedMultiplier <= 1.25)
                        launchAngle = 10f;
                    else
                        launchAngle = 2f;



                    if (target != null)
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


                        initialVelocity *= speedMultiplier;


                        // Calculate velocity components
                        Vector3 velocity = new Vector3(targetPos.x - startPos.x, 0, targetPos.z - startPos.z).normalized * initialVelocity;
                        velocity.y = initialVelocity * Mathf.Sin(launchAngle * Mathf.Deg2Rad);

                        // Introduce error to the throw
                        float errorMagnitude = 1f; //Change this to increase or decrease throw error
                        if (UnityEngine.Random.value < 0.15f) //Change the to increase or decrease percent chance of an error happening
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
            else
            {
                ResearchData.saveLog();
                gameManager.StartCoroutine("returnPlayerToHouse"); //starts a coroutine in the Game Manager that sends the player home 
            }
        }
     
        else
        {

            if (ball != null)
            {
                ball.transform.SetParent(null);
                BallManager.dropped = false;
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                rb.isKinematic = false;

                if (target != null)
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
    }
    // Helper method to calculate the launch velocity based on distance, height difference, gravity, and launch angle
    float CalculateLaunchVelocity(float distance, float yOffset, float gravity, float launchAngle)
    {
        float angleRad = launchAngle * Mathf.Deg2Rad;
        float vSquared = (gravity * distance * distance) / (2 * (yOffset - distance * Mathf.Tan(angleRad)) * Mathf.Pow(Mathf.Cos(angleRad), 2));
        return Mathf.Sqrt(Mathf.Abs(vSquared));
    }

    public float GetCurrentThrowSpeed()
    {

        int totalCatches = gameManager.TrackAllCatches();
        float currentSpeed = 1f; // Default speed

        foreach (var speed in ResearchData.LevelData.Speeds)
        {
            if (totalCatches <= speed.Throws)
            {
                currentSpeed = speed.SpeedValue;
                break; // Break once the right speed segment is found
            }
        }

        Debug.Log($"Current speed set to {currentSpeed} for {totalCatches} total catches.");
        return currentSpeed;
    }

    private GameObject ChooseThrowTarget()
    {
        if(ResearchData.AIPlayers != null && ResearchData.AIPlayers.Count != 0)
        {
            int totalCatches = gameManager.TrackAllCatches();
            List<GameObject> potentialTargets = new List<GameObject>(gameManager.playerList);
            potentialTargets.Remove(this.gameObject); // Exclude this AI from potential targets.

            // Calculate the current chance to target the player from the XML data
            float accumulatedChance = 0f;
            foreach (var chance in ResearchData.LevelData.ChancesToPlayer)
            {
                if (totalCatches <= chance.Throws)
                {
                    accumulatedChance = chance.Chance;
                    break;
                }
            }

            // Roll a random number to compare against the calculated chance
            float roll = UnityEngine.Random.Range(0f, 100f);
            Debug.Log($"Rolled {roll} against chance of {accumulatedChance}% for targeting the player at {totalCatches} catches.");

            // Decide based on the chance whether to target the player or a random AI
            if (roll < accumulatedChance && gameManager.Player)
            {
                Debug.Log("Targeting Player due to chance settings");
                foreach(Transform child in gameManager.Player.transform)
                {
                    // Target the player based on XML defined chance
                    if (child.tag.Equals("PlayerPos"))
                        return child.gameObject;
                        
                }
                return gameManager.Player;
            }
            else
            {
                // Choose a random AI 
                return potentialTargets[UnityEngine.Random.Range(1, potentialTargets.Count)];
            }
        }
        else
        {
            float chanceThreshold = 0.8f; // 80%
            float roll = UnityEngine.Random.Range(0f, 1f); // Random roll between 0 and 1
            List<GameObject> potentialTargets = new List<GameObject>(gameManager.playerList);
            potentialTargets.Remove(this.gameObject); // Exclude this AI from potential targets.

            switch (targetingPreference)
            {
                case TargetingPreference.Random:
                    // Return a random player GameObject
                    if (potentialTargets.Count > 0)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, potentialTargets.Count);

                        if (randomIndex== 0)
                        {
                            foreach (Transform child in gameManager.Player.transform)
                            {
                                // Target the player based on XML defined chance
                                if (child.tag.Equals("PlayerPos"))
                                    return child.gameObject;

                            }
                            
                        }
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
                            foreach (Transform child in gameManager.Player.transform)
                            {
                                // Target the player based on XML defined chance
                                if (child.tag.Equals("PlayerPos"))
                                    return child.gameObject;

                            }
                            return gameManager.Player;
                        }
                    }
                    break;
                case TargetingPreference.FavorPlayer:
                    // Similar to FavorAI, but primarily targets the player, with a 20% chance for a throw to the AI
                    if (potentialTargets.Count > 0)
                    {
                        if (roll < chanceThreshold)
                        {
                            foreach (Transform child in gameManager.Player.transform)
                            {
                                // Target the player based on XML defined chance
                                if (child.tag.Equals("PlayerPos"))
                                    return child.gameObject;

                            }
                            return gameManager.Player;
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

                


    }
