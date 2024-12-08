using System;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _gameOverScreneScore;
    [SerializeField] private TextMeshProUGUI _gameOverScreneHighScore;
        
    private int _score = 0;
    private int _pointsPerLargeAsteroid = 50;
    private int _pointsPerMediumAsteroid = 75;
    private int _pointsPerSmallAsteroid = 100;
    private int _pointsPerUFO = 200;

    private int _highScore;

    private void Awake()
    {
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScore();
    }

    private void OnEnable()
    {
        Asteroid.OnObjectDestroyed += CalculateScoreAsteroid;
        UFO.OnDead += CalculateScoreUFO;
    }

    private void CalculateScoreUFO()
    {
        _score += _pointsPerUFO;
    }

    private void OnDisable()
    {
        Asteroid.OnObjectDestroyed -= CalculateScoreAsteroid;
        UFO.OnDead -= CalculateScoreUFO;
    }

    private void CalculateScoreAsteroid(Asteroid asteroid)
    {
        if (asteroid.Size > asteroid.Medium)
        {
            _score += _pointsPerLargeAsteroid;
        }
        else if (asteroid.Size < asteroid.Medium && asteroid.Size > asteroid.Small)
        {
            _score += _pointsPerMediumAsteroid;
        }
        else
        {
            _score += _pointsPerSmallAsteroid;
        }

        UpdateScore();
    }

    private void UpdateScore()
    {
        if (_score > _highScore)
        {
            _highScore = _score;

            PlayerPrefs.SetInt("HighScore", _highScore);
            PlayerPrefs.Save();
        }

        _scoreText.text = $"{_score}";
        _gameOverScreneScore.text = $"Score: {_score}";
        _gameOverScreneHighScore.text = $"High Score: {_highScore}";
    }
}
