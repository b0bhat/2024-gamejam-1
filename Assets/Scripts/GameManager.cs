using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    void Awake() {
        instance = this;
    }
    #endregion

    [SerializeField]
    public  GameObject _pauseUI;
    [SerializeField]
    private GameObject _pauseText;
    [SerializeField]
    private GameObject _menuUI;
    [SerializeField]
    private GameObject _upgradeUI;
    [SerializeField]
    private AudioClip battleSong;
    [SerializeField]
    private AudioSource musicSource;
    public float scaling = 1f;
    public float scalingFactor = 0.02f;
    float elapsedTime = 0f;
    public float exponentialFactor = 1f;
    public int unlockCost = 100;
    public int doorCost = 200;
    public int doorCostIncrease = 100;
    // bool doorCurPurchase = false;
    // [TODO] implement later, prevent edge case where player can buy two doors at once


    private void Start() {
        Time.timeScale = 0;
        _menuUI.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)){
            PauseGame();
        }
    }

    public void Scale() {
        elapsedTime += 1f;
        GameManager.instance.scaling = Mathf.Pow(1 + scalingFactor, elapsedTime*exponentialFactor);
    }

    public void FinishDoorPurchase() {
        doorCost += doorCostIncrease;
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
        musicSource.clip = battleSong;
        musicSource.Play();
    }

    public void ShowUpgradeMenu()
    {
        Time.timeScale = 0;
        _upgradeUI.SetActive(true);
    }

    public void UpgradeSelected()
    {
        Time.timeScale = 1;
        _upgradeUI.SetActive(false);
        //add selected upgrade to player
    }
}
