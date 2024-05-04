using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clown : MonoBehaviour
{
    //private Rigidbody m_Rigidbody;
    //public int points;

    //public float fallDelay;

    //public Transform stepManager;

    //private bool hasCollided;

    public Transform enablePosition;
    public ScoreBoard scoreBoard;
    public int points;
    

    void Start()
    {
       //m_Rigidbody = GetComponent<Rigidbody>();
        //hasCollided = false;
    }

    private void OnEnable()
    {
        transform.position = enablePosition.position;
        transform.rotation = enablePosition.rotation;
        //hasCollided = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Ground"))
        {
            scoreBoard.score += points;
        }
    }

    /*public void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("ball"))
        {
            if (!hasCollided)
            {
                hasCollided = true;
                StartCoroutine(invokeAtDelay());
            }
            hasCollided = true;
        }
    }*/

    /*public void throwBall()
    {
        StartCoroutine(invokeAtDelay());
    }

    IEnumerator invokeAtDelay()
    {
        yield return new WaitForSeconds(fallDelay);

        if (TutorialManager.isTutorial)
        {
            if (hasCollided)
            {
                stepManager.SendMessage("Next");
                EventManager.onChangeState?.Invoke(0);
            }
            else
            {
                stepManager.SendMessage("jumpTo", 1);
            }

        }
        else
        {
            EventManager.onChangeState?.Invoke(1);
        }
        hasCollided = false;
    }*/
}