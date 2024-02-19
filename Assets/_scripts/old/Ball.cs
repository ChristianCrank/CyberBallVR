using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private Transform currentGlove;
    private bool inGlove;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inGlove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inGlove) transform.position = currentGlove.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Glove")) return;
        currentGlove = collision.transform.GetChild(0);
    }
}
