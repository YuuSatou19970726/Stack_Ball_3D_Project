using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private Text scoreText;
    public int score = 10;

    void Awake()
    {
        scoreText = GameObject.Find(FindTags.SCORE_TEXT).GetComponent<Text>();
        MakeSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        AddScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find(FindTags.SCORE_TEXT).GetComponent<Text>();
            scoreText.text = scoreText.ToString();
        }
    }

    private void MakeSingleton()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddScore(int _amount)
    {
        score += _amount;
        if (score > PlayerPrefs.GetInt(PlayerPrefsTags.HIGH_SCORE, 0))
            PlayerPrefs.SetInt(PlayerPrefsTags.HIGH_SCORE, score);

        scoreText.text = score.ToString();
    }


    public void ResetScore()
    {
        score = 0;
    }
}
