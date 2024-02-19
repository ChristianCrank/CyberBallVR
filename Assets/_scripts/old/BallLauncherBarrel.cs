using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncherBarrel : MonoBehaviour
{
    //references
    public Transform player;
    public Transform ball;
    public Transform ballSpawn;

    //control aiming
    public float aimSpeed;
    private float aimTimeCount;
    private Quaternion angleAxis;

    //variables for equation
    private float theta = 0f;
    public float heightAtCatch; //y - should replace this with y position of something attatched to the player





    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, angleAxis, aimTimeCount/aimSpeed);
        if(aimTimeCount < aimSpeed) aimTimeCount += Time.deltaTime;

    }

    public void startPitch()
    {
        StartCoroutine(setAngleRoutine());
    }

    IEnumerator setAngleRoutine()
    {
        angleAxis = Quaternion.Euler(theta, 0f, 0f);
        yield return new WaitForSeconds(aimSpeed);
        aimTimeCount = 0.0f;
        //theta = Random.Range(35f, 45f);
        theta = 35f;
        angleAxis = Quaternion.Euler(-theta, 0f, 0f);
        yield return new WaitForSeconds(aimSpeed);
        pitch();
    }

    private void pitch()
    {
        Debug.Log("Pictch");
        ball.position = ballSpawn.position;
        ball.forward = ballSpawn.forward;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().AddForce(transform.forward * getForce(), ForceMode.Impulse);

    }

    private float getForce()
    {
        float distance = Vector3.Distance(ballSpawn.position, player.position);
        float ballMass = ball.GetComponent<Rigidbody>().mass;
        float cosSquaerdTheta = (1f + Mathf.Cos(2f * theta)) / 2f;
        //float u = Mathf.Sqrt(Mathf.Abs(((distance * Mathf.Tan(theta)) - (-1 * Physics.gravity.y * distance * distance))/(2f * heightAtCatch * cosSquaerdTheta)));
        float u = Mathf.Sqrt(Mathf.Abs((-1 * Physics.gravity.y * distance * distance)/(((distance * Mathf.Tan(theta)) - heightAtCatch) * cosSquaerdTheta * 2)));

        Debug.Log("Force Applied" + (u * ballMass));
        Debug.Log("With Theta: " + theta);
        return (u*ballMass);
    }
}
