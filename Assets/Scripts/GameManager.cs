using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private List<GameObject> upgrade_buttons = new();
    [SerializeField]
    private AudioClip battleSong;
    [SerializeField]
    private AudioClip introSong;
    [SerializeField]
    private AudioClip gameOverSong;
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private GameObject tutorial;
    public float scaling = 1f;
    public float scalingFactor = 0.02f;
    float elapsedTime = 0f;
    public float exponentialFactor = 1f;
    public int unlockCost = 100;
    public int unlockCostIncrease = 50;
    public int doorCost = 100;
    public int doorCostIncrease = 100;
    public int chestCost = 100;
    public bool pauseLock = true;
    public bool upgradeLock = false;
    public bool gameover = false;
    public bool deathAudio = false;
    // bool doorCurPurchase = false;
    // [TODO] implement later, prevent edge case where player can buy two doors at once
    [SerializeField] List<UpgradeAsset> statUpgradeList = new();
    [SerializeField] List<UpgradeAsset> attackUpgradeList = new();
    [SerializeField] List<UpgradeAsset> attackTypeUpgradeList = new();

    UnityEngine.Rendering.Universal.ChromaticAberration ChromaticAberration;

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

    public enum StatBuffType {
        health,
        speed,
        moneyIncrease,
        pickupRange,

    }

    private Player player;

    List<UpgradeAsset> selectedItems = new();

    // private bool canShake = true;

    private void Start() {
        Time.timeScale = 0;
        _menuUI.SetActive(true);
        pauseLock = false;
        gameover = false;
        UnityEngine.Rendering.VolumeProfile profile = GameObject.Find("PostProcessVolume").GetComponent<UnityEngine.Rendering.Volume>().profile;
        profile.TryGet(out ChromaticAberration);
        ChromaticAberration.intensity.Override(0.6f);
        upgrade_buttons.Add(_upgradeUI.transform.Find("upgrade_button1").gameObject);
        upgrade_buttons.Add(_upgradeUI.transform.Find("upgrade_button2").gameObject);
        upgrade_buttons.Add(_upgradeUI.transform.Find("upgrade_button3").gameObject);
        player = Player.instance;
    }

    // Update is called once per frame
    void Update() {
        // if (_menuUI.activeInHierarchy == true && canShake) {
        //     CameraController.instance.ShakeCamera(0.5f,1f);
        // }
        if (Input.GetKeyDown(KeyCode.Escape) && !gameover){
            PauseGame();
        }

        if (gameover && !deathAudio)
        {
            GameOverAudio();
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

    public void FinishChestPurchase() {
        unlockCost += unlockCostIncrease;
    }

    public void PauseGame()
    {
        if (!gameover)
        {
            if (_pauseUI.activeInHierarchy)
            {
                Time.timeScale = 1;
                pauseLock = false;
                _pauseUI.SetActive(false);
                _pauseText.SetActive(false);
            }
            else if (!_pauseUI.activeInHierarchy && !pauseLock)
            {

                Time.timeScale = 0;
                pauseLock = true;
                _pauseUI.SetActive(true);
                _pauseText.SetActive(true);
            }
        }
    }

    public void GameOverAudio()
    {
        deathAudio = true;
        StartCoroutine(GameOverSounds());
    }

    IEnumerator GameOverSounds()
    {
        musicSource.clip = gameOverSong;
        musicSource.loop = false;
        musicSource.Play();
        yield return new WaitForSeconds(5f);
        musicSource.clip = introSong;
        musicSource.loop = true;
        musicSource.Play();
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
        tutorial.gameObject.SetActive(false);
        pauseLock = false;
        musicSource.clip = battleSong;
        musicSource.Play();
        //CameraController.instance.ShakeCamera(0.1f,2f);
    }
    public void TutorialScreen () {
        ChromaticAberration.intensity.Override(0.2f);
        tutorial.gameObject.SetActive(true);
        _menuUI.SetActive(false);
        //CameraController.instance.ShakeCamera(0.1f,2f);
    }

    public void ShowUpgradeMenu() {
        Time.timeScale = 0;
        List<UpgradeAsset> validUpgrades = new();

        // Stat upgrades
        foreach (UpgradeAsset upgrade in statUpgradeList) {
            validUpgrades.Add(upgrade);
        }
        // Check attack upgrades
        foreach (UpgradeAsset upgrade in attackUpgradeList) {
            bool ult = false;
            if (upgrade.upgradeType == 1) {
                ult = true;
            }
            if (player.CheckAttackUpgrade(upgrade.attackObject, upgrade.name, ult)) {
                validUpgrades.Add(upgrade);
            }
        }

        // Check new attack types
        foreach (UpgradeAsset attackType in attackTypeUpgradeList) {
            if (!player.CheckAttack(attackType.attackObject)) {
                validUpgrades.Add(attackType);
            } 
        }

        // pick 3 at random
        if (validUpgrades != null && validUpgrades.Count >= 3) {
            selectedItems = new List<UpgradeAsset>();
            for (int i = 0; i < 3; i++){
                int randomIndex = Random.Range(0, validUpgrades.Count);
                selectedItems.Add(validUpgrades[randomIndex]); 
                validUpgrades.RemoveAt(randomIndex); // prevent double pick
            }
            for (int i=0; i<selectedItems.Count; i++) {
                upgrade_buttons[i].transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = selectedItems[i].assetName;
                upgrade_buttons[i].transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Image>().sprite = selectedItems[i].upgradeIcon;
                upgrade_buttons[i].transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Image>().color = selectedItems[i].upgradeIconColor;
            }
        } else {
            Debug.LogWarning("Less than 3 available upgrades!");
        }
        _upgradeUI.SetActive(true);
        pauseLock = true;
    }

    public void UpgradeSelected(int num) {
        Time.timeScale = 1;
        _upgradeUI.SetActive(false);
        pauseLock = false;
        UpgradeAsset selectedUpgrade = selectedItems[num];
        if (selectedUpgrade.upgradeType == 0 || selectedUpgrade.upgradeType == 1) {
            ApplyAttackUpgrade(selectedUpgrade.attackObject, selectedUpgrade);
        }
        if (selectedUpgrade.upgradeType == 2) {
            ApplyStatUpgrade(selectedUpgrade);
        }
        if (selectedUpgrade.upgradeType == 3) {
            player.AddNewAttack(selectedUpgrade.attackObject);
        }
        if (selectedUpgrade.upgradeType == 4) {
            // healing
        }
        //add selected upgrade to player
        // [TODO] add new attacks as upgrades too
        upgradeLock = false;
    }

    private void ApplyStatUpgrade(UpgradeAsset upgradeAsset) {
        foreach (StatBuff statBuff in upgradeAsset.statbuffs) {
            switch(statBuff.type) {
                case StatBuffType.health:
                    player.Heal((int)statBuff.value, true);
                    break;
                case StatBuffType.speed:
                    player.moveSpeed += (int)statBuff.value;
                    break;
                case StatBuffType.moneyIncrease:
                    player.moneyIncrease += (int)statBuff.value;
                    break;
                case StatBuffType.pickupRange:
                    player.collectRange += statBuff.value;
                    break;
            }
        }
    }

    private void ApplyAttackUpgrade(GameObject attackObject, UpgradeAsset upgradeAsset) {
        AttackScript attack = player.GetAttack(attackObject);
        if (attack is null) {
            Debug.LogWarning("Missing AttackScript for apply upgrade!");
        }
        if (upgradeAsset.upgradeType == 1) {
            attack.alreadyUlt = true;
        }
        attack.currentUpgrades.Add(upgradeAsset);
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
