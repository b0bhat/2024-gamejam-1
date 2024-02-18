using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private TMP_Text _moneyText;
    [SerializeField]
    private Slider _healthSlider;
    [SerializeField]
    private GameObject _gameOver;
    [SerializeField]
    private TMP_Text _gameOverText;
    [SerializeField]
    private TMP_Text _gameOverScore;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "SCORE: " + 0;
        _moneyText.text = "MONEY: " + 0;
        _gameOver.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _gameOverScore.gameObject.SetActive(false);
        _healthSlider = GameObject.FindWithTag("Health").GetComponent<Slider>();
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = "SCORE: " + score;
        _gameOverScore.text = "FINAL SCORE: " + score;
    }
    public void UpdateMoneyText(int money)
    {
        _moneyText.text = "MONEY: " + money;
    }

    public void UpdateHealthSlider(float amount)
    {
        _healthSlider.value = amount;
    }

    public void UpdateHealthSliderMax(float amount)
    {
        _healthSlider.maxValue = amount;
    }

    public void GameOverSequence()
    {
        GameManager.instance.pauseLock = true;
        StartCoroutine(GameOverFlickerRoutine());
        _gameOver.SetActive(true);
        _gameOverScore.gameObject.SetActive(true);
    }

    IEnumerator GameOverFlickerRoutine()
    {
        // can also just change text to "GAME OVER" and nothing as well
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
    }
}
