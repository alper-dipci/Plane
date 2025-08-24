using System;
using _Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Upgrades
{
    public class UpgradeNode : MonoBehaviour
    {
        [SerializeField] private UpgradeNodeSO upgradeNodeSo;
        [SerializeField] private Button button;

        [ReadOnly]
        public int currentLevel;

        private void Awake()
        {
            button.onClick.AddListener(OnButtonClicked);
        }
        private void OnButtonClicked()
        {
            TryUpgrade();
        }

        private void TryUpgrade()
        {
            // Check if we can upgrade
            if (currentLevel >= upgradeNodeSo.MaxLevel)
                return;
            
            var upgradeData = upgradeNodeSo.GetUpgradeData(currentLevel +1);
            var cost = upgradeData.costData;
            // Check if we have enough currency
            
            // Deduct currency
            
            // upgrade our level
            currentLevel++;
            
            // Apply the upgrade effect
            var effects = upgradeData.effects;
            foreach (var effect in effects)
                effect.ApplyEffect();
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }

        public UpgradeNodeProgressState GetProgressState()
        {
            if (currentLevel == 0)
            {
                return UpgradeNodeProgressState.Locked;
            }
            else if (currentLevel >= upgradeNodeSo.MaxLevel)
            {
                return UpgradeNodeProgressState.Maxed;
            }
            else
            {
                return UpgradeNodeProgressState.Unlocked;
            }
        }
    }
}