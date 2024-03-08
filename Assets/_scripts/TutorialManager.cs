using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public enum State
    {
        Paused,
        Catching,
        Throwing,
    }

    private void OnEnable()
    {
        EventManager.onChangeState += setState;
    }

    private void OnDisable()
    {
        EventManager.onChangeState -= setState;
    }

    private State tutorialState;
    //Start is called before the first frame update
    void Start()
    {
        tutorialState = State.Paused;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void doChange()
    {
        if (tutorialState == State.Paused)
        {
            EventManager.onTogglePitch?.Invoke(false);
        }
        else if (tutorialState == State.Catching)
        {
            EventManager.onTogglePitch?.Invoke(true);
        }
        else if (tutorialState == State.Throwing)
        {
            EventManager.onTogglePitch?.Invoke(false);
        }
    }

    public void setState(int x)
    {
        if (x == 0) tutorialState = State.Paused;
        if (x == 1) tutorialState = State.Catching;
        if (x == 2) tutorialState = State.Throwing;
        doChange();
    }
}
