using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBall : MonoBehaviour
{
    //private Rigidbody m_Rigidbody;
    //public int points;

    public float fallDelay;

    public Transform stepManager;

    private bool hasCollided;


    void Start()
    {
        //m_Rigidbody = GetComponent<Rigidbody>();
        hasCollided = false;
    }

    private void OnEnable()
    {
        hasCollided = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("clown"))
        {
            /*if (!hasCollided)
            {
                hasCollided = true;
                StartCoroutine(invokeAtDelay());
            }*/
            hasCollided = true;
        } 
        else if(collision.transform.CompareTag("Ground") && TutorialManager.tutorialState != TutorialManager.State.Throwing)
        {
            EventManager.onBallDropped?.Invoke();
        }
    }

    public void throwBall()
    {
        StartCoroutine(invokeAtDelay());
    }

    public void caughtBall()
    {
        if(TutorialManager.isTutorial)
        {
            stepManager.SendMessage("Next");
        }
        EventManager.onChangeState?.Invoke(2);
        EventManager.onSuccessfulCatch?.Invoke();
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
                EventManager.onSuccessfulCatch?.Invoke();
            }
            else
            {
                stepManager.SendMessage("jumpTo", 1);
                EventManager.onChangeState?.Invoke(1);
            }

        }
        else
        {
            EventManager.onChangeState?.Invoke(1);
            if(hasCollided)
            {
                EventManager.onSuccessfulCatch?.Invoke();
            } 
        }
        hasCollided = false;
    }
}
