using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialPlayer : MonoBehaviour
{
    public Transform playerParent;
    public Transform jimmy;
    public CharacterController characterController;

    public void Start()
    {
        playerParent.position = transform.position;
        jimmy.position = transform.position;
        characterController.transform.position = transform.position;

        playerParent.rotation = transform.rotation;
        jimmy.rotation = transform.rotation;
        characterController.transform.rotation = transform.rotation;

    }
}
