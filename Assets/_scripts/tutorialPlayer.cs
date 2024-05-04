using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialPlayer : MonoBehaviour
{
    public Transform player;

    public void Start()
    {
        player.position = transform.position;
        player.rotation = transform.rotation;
    }
}
