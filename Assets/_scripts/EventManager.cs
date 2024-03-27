using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EventManager : MonoBehaviour
{
    public delegate void OnTogglePitch(bool b); //Will be triggered by floating UI button
    public static OnTogglePitch onTogglePitch;

    public delegate void OnSuccessfulCatch(); //Triggered by catching the ball
    public static OnSuccessfulCatch onSuccessfulCatch;

    //Temporaryily simulates ball catch
    public static void CatchBall()
    {
        onSuccessfulCatch?.Invoke();
    }

    public delegate void OnBallDropped(); //Triggered on missed catch
    public static OnBallDropped onBallDropped;

    //Temporarily simulates ball drop
    public static void DropBall()
    {
        onBallDropped?.Invoke();
    }

    public delegate void OnChangeState(int x);
    public static OnChangeState onChangeState;


    public delegate void OnBallGrabbed(); //Triggered when grabbing ball, not from catch
    public static OnBallGrabbed onBallGrabbed;


    public delegate void OnAISuccessfulCatch(); //Triggered by catching the ball
    public static OnAISuccessfulCatch onAISuccessfulCatch;

    //Temporaryily simulates ball catch
    public static void AICatchBall()
    {
        onSuccessfulCatch?.Invoke();
    }

}
