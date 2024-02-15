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
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }

        SceneManager.LoadScene(0);  // game scene
    }
}
