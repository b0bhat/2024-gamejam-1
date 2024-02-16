using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour {
    public enum UpgradeType {
        reloadTime,
        burstCount,
        timeBetweenBurst,
        force,
        spread,
        damage,
        bulletSpeed,
        penetration,
        angleBurst,
        projInShot,
        bulletColor
    }
    public List<UpgradeAsset> upgrades;

    // Apply upgrades to the weapon
    public void ApplyUpgradesToWeapon(AttackScript attack)
    {
        foreach (UpgradeAsset upgrade in upgrades)
        {
            switch (upgrade.type)
            {
                case UpgradeType.reloadTime:
                    attack.reloadTime *= 1f + upgrade.value;
                    break;
                case UpgradeType.burstCount:
                    attack.burstCount += (int)upgrade.value;
                    break;
                case UpgradeType.timeBetweenBurst:
                    attack.timeBetweenBurst *= 1f + upgrade.value;
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
                    attack.bulletSpeed *= 1f + upgrade.value;
                    break;
                case UpgradeType.projInShot:
                    attack.projInShot += (int)upgrade.value;
                    break;
                case UpgradeType.bulletColor:
                    attack.bulletColor = upgrade.bulletColor;
                    break;
            }
        }
    }
}
