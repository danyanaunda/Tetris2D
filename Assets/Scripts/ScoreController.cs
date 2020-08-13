using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public static int lastScore;
    public static int highScore;

    private const string HIGH_SCORE_PREFIX = "highscore";

    [SerializeField] private List<Text> highScoreTexts;
    [SerializeField] private List<Text> lastScoreTexts;

    void Start()
    {
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_PREFIX);
        foreach (var text in highScoreTexts) text.text = highScore.ToString();
        foreach (var text in lastScoreTexts) text.text = lastScore.ToString();
    }

    public void AddScore(int count)
    {
        lastScore += count;
        foreach (var text in lastScoreTexts) text.text = lastScore.ToString();

        if(lastScore > highScore)
        {
            highScore = lastScore;
            foreach (var text in highScoreTexts) text.text = highScore.ToString();
            PlayerPrefs.SetInt(HIGH_SCORE_PREFIX, highScore);
        }
    }
}
