using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnTogglePitch(); //Will be triggered by floating UI button
    public static OnTogglePitch onTogglePitch;
}
