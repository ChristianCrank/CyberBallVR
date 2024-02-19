using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PitchingMachine : MonoBehaviour
{
    public Transform ballSpawn;
    public Transform ball;
    public Transform emptyRotator;

    public float timeToTurn;
    private float passedTime;

    public float timeBetweenPitch;

    private bool isPitching;
    private bool rotating;

    private Quaternion rotateTo;

    private int currentPitch;

    [Serializable]
    private class PitchingInstructions
    {
        [Range(20f, 45f)]
        public float rotation;
        [Range(2f, 2.5f)]
        public float force;

        //5 rotation - 5 force
        //23.5 rotation - force
        //25 roation - 2.25 force
        //35 rotation - force
        //45 rotation - 2 force
    }

    [SerializeField] private PitchingInstructions[] pitches;

    private void OnEnable()
    {
        EventManager.onTogglePitch += togglePitch;
    }

    private void OnDisable()
    {
        EventManager.onTogglePitch -= togglePitch;
    }

    private void Start()
    {
        isPitching = false;
        currentPitch = 0;
        rotating = false;
        emptyRotator.position = transform.position;
        emptyRotator.rotation = transform.rotation;
        passedTime = 0f;
    }

    void togglePitch()
    {
        isPitching = !isPitching;
        //Log("Toggling to: " + isPitching);
        if (isPitching) StartCoroutine(SetAim());
    }

    IEnumerator SetAim()
    {
        //Debug.Log("Setting Aim");
        float rotation = pitches[currentPitch].rotation;
        emptyRotator.Rotate(rotation, 0f, 0f);
        rotateTo = emptyRotator.rotation;
        rotating = true;
        yield return new WaitForSeconds(timeToTurn); //wait till at angle
        rotating = false;
       
        Pitch(); //pitch

        //Debug.Log("Resetting Aim");
        emptyRotator.Rotate(-rotation, 0f, 0f);
        rotateTo = emptyRotator.rotation;
        rotating = true;
        yield return new WaitForSeconds(timeToTurn); //wait till at angle
        rotating = false;
        currentPitch = (currentPitch + 1) % pitches.Length;
        if (isPitching) StartCoroutine(SetAim());
    }

    private void Pitch()
    {
        ball.gameObject.SetActive(false);
        ball.position = ballSpawn.position;
        ball.up = transform.forward;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.gameObject.SetActive(true);
        ball.GetComponent<Rigidbody>().AddForce(transform.up * pitches[currentPitch].force, ForceMode.Impulse);


        Debug.Log("Pitching at angle: " + pitches[currentPitch].rotation + " with force: " + pitches[currentPitch].force);
    }

    private void Update()
    {
        if (rotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, (pitches[currentPitch].rotation/timeToTurn) * Time.deltaTime);
        }
    }
}
