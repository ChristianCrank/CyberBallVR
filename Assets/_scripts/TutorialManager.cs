using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject pitchingMachine;
    [SerializeField] GameObject clownGame;



    public static bool isTutorial;

    public int score;

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

    public static State tutorialState;

    //Start is called before the first frame update
    void Start()
    {
        tutorialState = State.Paused;
        isTutorial = true;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialState == State.Paused)
        {
            
        }
        else if (tutorialState == State.Catching)
        {
            pitchingMachine.SetActive(true);
            clownGame.SetActive(false);
        }
        else if (tutorialState == State.Throwing)
        {
            pitchingMachine.SetActive(false);
            clownGame.SetActive(true);
        }
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
        if (x == 1)
        {
            pitchingMachine.SetActive(true);
            tutorialState = State.Catching;
        }
        if (x == 2) tutorialState = State.Throwing;
        doChange();
    }

    public void setTutorial(bool b)
    {
        isTutorial = b;
    }

    public void exit()
    {
        SceneManager.LoadScene("CyberballVR");
    }

    /*public void OnDestroy()
    {
        EventManager.onChangeState = null;
    }*/
}
