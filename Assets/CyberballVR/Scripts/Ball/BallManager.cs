using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BallManager : MonoBehaviour
{
    public Transform ballSpawn;
    public XRGrabInteractable ball;
    public static bool dropped;
    // Start is called before the first frame update
    void Start()
    {
        dropped = true;
        ball.transform.position = ballSpawn.position;
        SetBallKinematic(true);
        Debug.Log("script restarted");

    }

    //Respawns ball in front of player if dropped on ground
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Terrain"))
        {
            //AI
            if(GameManager.currentBallHolder != null && GameManager.currentBallHolder.GetComponent<AI>() != null)
            {
                Debug.Log("ball collided with terrain");
                ballSpawn = GameManager.currentBallHolder.GetNamedChild("BallSpawn").transform;
                ball.transform.position = ballSpawn.position;
                //SetBallKinematic(true);
                GameManager.currentBallHolder.GetComponent<AI>().AICatch(ball.gameObject);
                dropped = true;
                ball.GetComponent<BallEffects>().IncrementGrabCount();
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
