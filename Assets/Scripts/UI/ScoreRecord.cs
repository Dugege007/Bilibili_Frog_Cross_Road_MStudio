using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRecord : MonoBehaviour
{
    public Text scoreText;

    public void SetScoreText(int point)
    {
        scoreText.text = point.ToString();
    }
}