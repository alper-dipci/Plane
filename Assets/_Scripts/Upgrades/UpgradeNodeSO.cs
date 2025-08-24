using System;
using UnityEngine;
using System.Collections.Generic;
using _Scripts.Upgrades.UpgradeEffects;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[CreateAssetMenu(fileName = "New Upgrade Node", menuName = "Upgrade System/Upgrade Node")]
public class UpgradeNodeSO : SerializedScriptableObject
{
    [Header("Basic Info")] public string upgradeName;
    [TextArea(3, 5)] public string description;
    
    [PreviewField(80, ObjectFieldAlignment.Center), HideLabel]
    public Sprite upgradeIcon;

    [Header("Effects")]
    [ShowInInspector] 
    [ListDrawerSettings(ShowItemCount = true)]
    public List<UpgradeData> effectsPerLevel = new List<UpgradeData>();
    
    public int MaxLevel => effectsPerLevel.Count;
    
    public UpgradeData GetUpgradeData(int level) => 
        level > 0 && level <= effectsPerLevel.Count ? effectsPerLevel[level -1] : null;
}

[Serializable]
public class UpgradeData
{
    [SerializeField] public List<UpgradeCostData> costData;
    [SerializeReference] public List<UpgradeEffect> effects = new List<UpgradeEffect>();
}