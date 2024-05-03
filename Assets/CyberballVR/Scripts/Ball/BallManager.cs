using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//Made by Christian
public class BallManager : MonoBehaviour
{
    public Transform ballSpawn;
    public XRGrabInteractable ball;
    public static bool dropped;
    public AudioSource ballDrop;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("script restarted");
    }

    public void SetupBall()
    {
        this.gameObject.SetActive(true);
        dropped = true;
        ball.transform.position = ballSpawn.position;
        SetBallKinematic(true);
    }

    //Respawns ball in front of player if dropped on ground
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Terrain"))
        {
            //AI
            if(GameManager.currentBallHolder != null && GameManager.currentBallHolder.GetComponent<AI>() != null)
            {
                dropped = true;
                Debug.Log("ball collided with terrain");
                ballSpawn = GameManager.currentBallHolder.GetNamedChild("BallSpawn").transform;
                ball.transform.position = ballSpawn.position;
                //SetBallKinematic(true);
                GameManager.currentBallHolder.GetComponent<AI>().AICatch(ball.gameObject);
                ball.GetComponent<BallEffects>().ResetGrabCount();
            }
            //Player
            else if(GameManager.currentBallHolder != null && GameManager.currentBallHolder.GetComponent<AI>() == null)
            {
                Debug.Log("ball collided with terrain");
                ballSpawn = GameManager.currentBallHolder.GetNamedChild("BallSpawn").transform;
                ball.transform.position = ballSpawn.position;
                SetBallKinematic(true);
                dropped = true;
            }

            ballDrop.Play();
        }
    }



    public void DisableKinematicOnGrab()
    {
        SetBallKinematic(false);
    }

    void SetBallKinematic(bool isKinematic)
    {
        if (ball != null && ball.GetComponent<Rigidbody>() != null)
        {
            ball.GetComponent<Rigidbody>().isKinematic = isKinematic;
        }
    }
}
