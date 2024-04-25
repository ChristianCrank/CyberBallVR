using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class FadeToBlack : MonoBehaviour
{

    public Image fadeToBlackImg;
    public TMP_Text fadeText;
    [Header("Time spent at completely opaque")]
    public float holdOnBlack;
    [Header("Time from transparent to opaque")]
    private float fadeTime; //total time of fade to black is this * 2
    private bool fadeIn;
    private bool fadeOut;
    private float timer;
    private Color Opaque;
    private Color Transparent;
    private Color opaqueWhite;
    private Color transparentWhite;

    private void Start()
    {
        fadeIn = false;
        fadeOut = false;
        timer = 0f;
        Opaque = new Color(0, 0, 0, 1);
        Transparent = new Color(0, 0, 0, 0);
        opaqueWhite = new Color(1, 1, 1, 1);
        transparentWhite = new Color(1, 1, 1, 0);
        fadeToBlackImg.color = Transparent;
        fadeText.color = transparentWhite;
    }


    // Start is called before the first frame update
    private void Update()
    {
        if(fadeIn)
        {
            timer += Time.deltaTime;
            if(timer/fadeTime >= 1)
            {
                fadeIn = false;
                fadeOut = true;
                timer += holdOnBlack; 
            }
        }

        if(fadeOut)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = 0;
                fadeOut = false;
            }
        }

        fadeToBlackImg.color = Color.Lerp(Transparent, Opaque, (timer / fadeTime));
        fadeText.color = Color.Lerp(transparentWhite, opaqueWhite, (timer / fadeTime));

    }

    public void cancel()
    {
        fadeIn = false;
        fadeOut = false;
        timer = 0;
        fadeToBlackImg.color = Transparent;
        fadeText.color = Transparent;
    }
    /// <summary>
    /// Starts a fade to black animation that displays msg and takes newFadeTime to go from transparent to opaque
    /// </summary>
    /// <param name="msg">message displayed on animation</param>
    /// <param name="newFadeTime">time from transparent to opaque. Total time = this * 2</param>
    public void fadeToBlack(string msg, float newFadeTime)
    {
        fadeIn = true;
        fadeText.text = msg;
        fadeTime = newFadeTime;
    }

}
