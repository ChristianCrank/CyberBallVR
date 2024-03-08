using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchingBtn : MonoBehaviour
{
    public void TogglePractice()
    {
        if(EventManager.onTogglePitch != null)
        {
            //EventManager.onTogglePitch.Invoke();
        }
    }
}
