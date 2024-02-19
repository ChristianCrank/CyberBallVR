using System.Collections;
using UnityEngine;

public class BallLauncherBase : MonoBehaviour
{
    //reference variables
    public Transform player;
    public BallLauncherBarrel barrelScript;

    //Variables for controlling facing direction
    public float timeBetweenPitch;
    public float turnSpeed;
    private float turnTimeCount;
    private Quaternion lookAt;


    // Start is called before the first frame update
    void Start()
    {
        if (timeBetweenPitch < turnSpeed) timeBetweenPitch = turnSpeed;
        StartCoroutine(pitchRoutine());
        turnTimeCount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, turnTimeCount/turnSpeed);
        turnTimeCount += Time.deltaTime;
    }

    IEnumerator pitchRoutine()
    {
        while (true)
        {
            Vector3 target = new Vector3(player.position.x - transform.position.x, player.position.y, player.position.z - transform.position.z);
            lookAt = Quaternion.LookRotation(target);
            turnTimeCount = 0f;
            //Debug.Log("updating position");
            yield return new WaitForSeconds(turnSpeed);
            barrelScript.startPitch();
            yield return new WaitForSeconds(timeBetweenPitch);

        }
    }
}
