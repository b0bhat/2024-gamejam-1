using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeAsset", order =1)]
public class UpgradeAsset : ScriptableObject {
    public String assetName = "Upgrade";
    public GameObject attackScript;
    public int upgradeType = 0;
    public Sprite upgradeIcon;
    public Color upgradeIconColor;
    // 0 for normal, 1 for ultimate
    public List<Upgrade> upgrades = new List<Upgrade>();
}

[System.Serializable]
public class Upgrade {
    public GameManager.UpgradeType type;
    public float value;
    // public Color bulletColor;
}
