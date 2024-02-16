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
    [SerializeField] List<UpgradeAsset> statUpgradeList = new();
    [SerializeField] List<UpgradeAsset> attackUpgradeList = new();

    public enum UpgradeType {
        burstTime,
        burstCount,
        fireRate,
        force,
        spread,
        damage,
        bulletSpeed,
        penetration,
        angleBurst,
        projInShot,
        bulletColor
    }

    // private bool canShake = true;

    private void Start() {
        Time.timeScale = 0;
        _menuUI.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        // if (_menuUI.activeInHierarchy == true && canShake) {
        //     CameraController.instance.ShakeCamera(0.5f,1f);
        // }
        if (Input.GetKeyDown(KeyCode.Escape)){
            PauseGame();
        }
    }

    // IEnumerator ScreenShake() {
    //     canShake = false;
    //     CameraController.instance.ShakeCamera(0.5f,1f);
    //     yield return new WaitForSeconds(Random.Range(0.1f,2f));
    //     canShake = true;
    // }

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
        List<UpgradeAsset> validUpgrades = new();
        // [TODO] Stat upgrades
         foreach (UpgradeAsset upgrade in statUpgradeList) {
         }
        // Check attack upgrades
        foreach (UpgradeAsset upgrade in attackUpgradeList) {
            if (Player.instance.CheckAttack(upgrade.attackName, upgrade.name)) {
                validUpgrades.Add(upgrade);
            }
        }

        // pick 3 at random
        if (validUpgrades != null && validUpgrades.Count >= 3) {
            List<UpgradeAsset> selectedItems = new List<UpgradeAsset>();
            for (int i = 0; i < 3; i++){
                int randomIndex = Random.Range(0, validUpgrades.Count);
                selectedItems.Add(validUpgrades[randomIndex]); 
                validUpgrades.RemoveAt(randomIndex); // prevent double pick
            }
            foreach (UpgradeAsset item in selectedItems) {
                Debug.Log(item.name);
            }
        } else {
            Debug.LogWarning("Less than 3 available upgrades!");
        }
        _upgradeUI.SetActive(true);
    }

    public void UpgradeSelected() {
        Time.timeScale = 1;
        _upgradeUI.SetActive(false);
        //add selected upgrade to player
        // [TODO] add new attacks as upgrades too
    }

    public void ApplyAttackUpgrade(AttackScript attack, UpgradeAsset upgradeAsset) {
        foreach (Upgrade upgrade in upgradeAsset.upgrades) {
            switch (upgrade.type) {
                case UpgradeType.burstTime:
                    attack.burstTime *= 1f + upgrade.value;
                    break;
                case UpgradeType.burstCount:
                    attack.burstCount += (int)upgrade.value;
                    break;
                case UpgradeType.fireRate:
                    attack.fireRate *= 1f + upgrade.value;
                    break;
                case UpgradeType.force:
                    attack.force *= 1f + upgrade.value;
                    break;
                case UpgradeType.spread:
                    attack.spread *= 1f + upgrade.value;
                    break;
                case UpgradeType.damage:
                    attack.damage *= 1f + upgrade.value;
                    break;
                case UpgradeType.bulletSpeed:
                    attack.bulletSpeed *= 1f + upgrade.value;
                    break;
                case UpgradeType.penetration:
                    attack.penetration += (int)upgrade.value;
                    break;
                case UpgradeType.angleBurst:
                    attack.angleBurst += upgrade.value;
                    break;
                case UpgradeType.projInShot:
                    attack.projInShot += (int)upgrade.value;
                    break;
                // case UpgradeType.bulletColor:
                //     attack.bulletColor = upgrade.bulletColor;
                //     break;
            }
        }
    }
}
