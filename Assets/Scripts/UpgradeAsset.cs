using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeAsset", order =1)]
public class UpgradeAsset : ScriptableObject {
    public String assetName = "Upgrade";
    public GameObject attackObject;
    public int upgradeType = 0;
    // 0 for normal, 1 for ultimate, 2 for player stat, 3 for attack
    public Sprite upgradeIcon;
    public Color upgradeIconColor;
    public List<Upgrade> upgrades = new List<Upgrade>();
    public List<StatBuff> statbuffs = new List<StatBuff>();
}

[System.Serializable]
public class Upgrade {
    public GameManager.UpgradeType type;
    public float value;
    // public Color bulletColor;
}

[System.Serializable]
public class StatBuff {
    public GameManager.StatBuffType type;
    public float value;
    // public Color bulletColor;
}
