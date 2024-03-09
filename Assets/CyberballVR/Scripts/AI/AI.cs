using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AI : MonoBehaviour
{
    GameObject ball;
    public Transform ballHolder;
    public List<Transform> playerList;
    public float launchAngle = 55f;
    public static bool dropped = true;

    private void Start()
    {
        dropped = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Ball"))
        {

            Debug.Log("AI HIT");

            ball = other.gameObject;
            ball.transform.position = ballHolder.position;

            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.isKinematic = true;

            StartCoroutine(ThrowBall());
        }
            
    }


    private void FixedUpdate()
    {
        this.transform.LookAt(playerList[0].position);
    }

    private IEnumerator ThrowBall()
    {
        yield return new WaitForSeconds(2);

        if (ball != null)
        {
            dropped = false;
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            Vector3 startPos = ball.transform.position;
            Vector3 targetPos = playerList[0].transform.position;

            // Calculate direction to target
            Vector3 direction = (targetPos - startPos).normalized;

            // Calculate distance to target
            float distance = Vector3.Distance(startPos, targetPos);

            // Calculate initial velocity magnitude for desired arc
            float initialVelocity = Mathf.Sqrt((distance * Physics.gravity.magnitude) / Mathf.Sin(2 * launchAngle * Mathf.Deg2Rad));

            // Calculate initial velocity vector
            Vector3 initialVelocityVector = direction * initialVelocity;

            // Apply initial velocity
            rb.velocity = initialVelocityVector;

            // Calculate additional upward force to achieve the desired arc
            float upwardForce = initialVelocity * Mathf.Sin(launchAngle * Mathf.Deg2Rad);


            // Apply additional upward force only once
            rb.AddForce(Vector3.up * upwardForce, ForceMode.VelocityChange);

            //// Wait for the ball to hit the ground
            //while (ball.transform.position.y > 0.1f) // Adjust 0.1f as needed for ground detection
            //{
            //    yield return null;
            //}

            ball = null;
        }
    }
}
