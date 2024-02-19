using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchedBall : MonoBehaviour
{
    private Rigidbody rbBall;
    public Vector3 launchTrajectory;

    // Start is called before the first frame update
    void Awake()
    {
        rbBall = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        rbBall.AddForce(-transform.forward * 4f, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            rbBall.velocity = Vector3.zero;
            //Debug.Log("caught");
        }
    }
}
