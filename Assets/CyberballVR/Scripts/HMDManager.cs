using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HMDManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Is Device Active " + XRSettings.isDeviceActive);
        Debug.Log("Device Name is : " + XRSettings.loadedDeviceName);

        if (!XRSettings.isDeviceActive)
        {
            Debug.Log("No Headset plugged");
        }
        else if (XRSettings.isDeviceActive && (XRSettings.loadedDeviceName == "MockHMD Display")) 
        {
            Debug.Log("Using Mock HMD");
        }
        else
        {
            Debug.Log("Current Headset is " + XRSettings.loadedDeviceName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
