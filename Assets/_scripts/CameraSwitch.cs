using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject thirdPersonCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam.SetActive(true);
        thirdPersonCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
            //mainCam.SetActive(!mainCam.activeInHierarchy);
            //thirdPersonCam.SetActive(!thirdPersonCam.activeInHierarchy);
        //}
    }
}
