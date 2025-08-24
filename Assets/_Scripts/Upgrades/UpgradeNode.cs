using System;
using System.Collections.Generic;
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
        [SerializeField] private Image lockedImage;
        [SerializeField] private Image progressImage;
        [SerializeField] private Image maxedImage;

        [ReadOnly] public int currentLevel;
        
        public UpgradeNode parentNode;
        public List<UpgradeNode> childNodes = new List<UpgradeNode>();
        public List<Image> connectionLines; 

        private void Awake()
        {
            button.onClick.AddListener(OnButtonClicked);
            UpdateVisual();
            
            if(parentNode != null && !parentNode.IsUnlocked())
                HideNode();
            else
                ShowNode();
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

            var upgradeData = upgradeNodeSo.GetUpgradeData(currentLevel + 1);
            var cost = upgradeData.costData;
            // Check if we have enough currency

            // Deduct currency

            // upgrade our level
            currentLevel++;

            // Apply the upgrade effect
            var effects = upgradeData.effects;
            foreach (var effect in effects)
                effect.ApplyEffect();

            //update visual
            UpdateVisual();
            
            // If we just unlocked the first level, show child nodes
            if (currentLevel == 1) 
            {
                foreach (var child in childNodes)
                {
                    child.ShowNode();
                }
            }
        }

        private void UpdateVisual()
        {
            var progressState = GetProgressState();
            switch (progressState)
            {
                case UpgradeNodeProgressState.Locked:
                    lockedImage.enabled = true;
                    progressImage.fillAmount = 1;
                    maxedImage.enabled = false;
                    connectionLines.ForEach(line => line.enabled = false);
                    break;
                case UpgradeNodeProgressState.Maxed:
                    lockedImage.enabled = false;
                    progressImage.fillAmount = 0;
                    maxedImage.enabled = true;
                    connectionLines.ForEach(line => line.enabled = true);
                    break;
                case UpgradeNodeProgressState.Unlocked:
                    lockedImage.enabled = false;
                    progressImage.fillAmount = 1 - ((float)currentLevel / upgradeNodeSo.MaxLevel);
                    maxedImage.enabled = false;
                    connectionLines.ForEach(line => line.enabled = true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
        
        public bool IsUnlocked()
        {
            return currentLevel > 0;
        }

        public void ShowNode()
        {
            gameObject.SetActive(true);
            connectionLines.ForEach(line => line.enabled = true);
            UpdateVisual();
        }
        public void HideNode()
        {
            gameObject.SetActive(false);
            connectionLines.ForEach(line => line.enabled = false);
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