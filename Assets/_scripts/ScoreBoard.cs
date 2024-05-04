using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    public int score;
    public TMP_Text scoreText;
    public GameObject pointsText;

    public void Start()
    {
        score = 0;
    }

    public void Update()
    {
        if (score == 0)
        {
            scoreText.text = "000";
        }
        else if (score < 10)
        {
            scoreText.text = "00" + score;
        }
        else if (score < 100)
        {
            scoreText.text = "0" + score;
        }
        else if(score >= 100 && score <= 999)
        {
            scoreText.text = "" + score;
        }
        else if (score >= 999)
        {
            scoreText.text = "999";
        }
    }
}
