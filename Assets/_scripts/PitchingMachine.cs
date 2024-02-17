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
        [Range(0f, 90f)]
        public float rotation;
        [Range(0f, 100f)]
        public float force;
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
        Debug.Log("Toggling to: " + isPitching);
        if (isPitching) StartCoroutine(SetAim());
    }

    IEnumerator SetAim()
    {
        Debug.Log("Setting Aim");
        float rotation = pitches[currentPitch].rotation;
        emptyRotator.Rotate(rotation, 0f, 0f);
        rotateTo = emptyRotator.rotation;
        rotating = true;
        yield return new WaitForSeconds(timeToTurn); //wait till at angle
        rotating = false;
       
        Pitch(); //pitch

        Debug.Log("Resetting Aim");
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
        Debug.Log("Pitching at angle: " + pitches[currentPitch].rotation + " with force: " + pitches[currentPitch].force);
    }

    private void Update()
    {
        if (rotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, (pitches[currentPitch].rotation/timeToTurn) * Time.deltaTime);
            if (passedTime < timeToTurn)
            {
                passedTime += Time.deltaTime;
            } 
            else
            {
                Debug.Log("That took" + passedTime);
                passedTime = 0f;
            }
        }
    }
}
