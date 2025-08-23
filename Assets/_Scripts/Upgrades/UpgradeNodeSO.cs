using System;
using UnityEngine;
using System.Collections.Generic;
using _Scripts.Upgrades.UpgradeEffects;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Upgrade Node", menuName = "Upgrade System/Upgrade Node")]
public class UpgradeNodeSO : SerializedScriptableObject
{
    [Header("Basic Info")] public string upgradeName;
    [TextArea(3, 5)] public string description;
    public Sprite upgradeIcon;

    [Header("Effects")]
    [ShowInInspector] // Bu satırı ekle
    [ListDrawerSettings(ShowItemCount = true)]
    public List<UpgradeData> effectsPerLevel = new List<UpgradeData>();
}

[Serializable]
public class UpgradeData
{
    [SerializeField] public List<UpgradeCostData> costData;

    [ShowInInspector] public List<UpgradeEffect> effects = new List<UpgradeEffect>();
}