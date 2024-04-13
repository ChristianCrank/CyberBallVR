using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] ballCatch;
    public AudioClip[] ballThrow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playBallCatch()
    {
        audioSource.PlayOneShot(ballCatch[Random.Range(0, ballCatch.Length)]);
    }

    public void playBallThrow()
    {
        audioSource.PlayOneShot(ballThrow[Random.Range(0, ballThrow.Length)]);
    }
}
