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

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _moneyText.text = "Money: " + 0;
        _gameOver.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _healthSlider = GameObject.FindWithTag("Health").GetComponent<Slider>();
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = "Score: " + score;
    }
    public void UpdateMoneyText(int money)
    {
        _moneyText.text = "Money: " + money;
    }

    public void UpdateHealthSlider(int amount)
    {
        _healthSlider.value = amount;
    }

    public void GameOverSequence()
    {
        StartCoroutine(GameOverFlickerRoutine());
        _gameOver.SetActive(true);
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
