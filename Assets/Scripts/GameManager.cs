using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public  GameObject _pauseUI;
    [SerializeField]
    private GameObject _pauseText;
    [SerializeField]
    private GameObject _menuUI;

    private void Start() {
        Time.timeScale = 0;
        _menuUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (_pauseUI.activeInHierarchy)
        {
            Time.timeScale = 1;
            _pauseUI.SetActive(false);
            _pauseText.SetActive(false);
        }
        else if (!_pauseUI.activeInHierarchy)
        {
            Time.timeScale = 0;
            _pauseUI.SetActive(true);
            _pauseText.SetActive(true);
        }
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);  // game scene
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        _menuUI.SetActive(false);
    }
}
