using System;
using _Scripts.Enums;
using _Scripts.Player;

namespace _Scripts.Upgrades.UpgradeEffects
{
    [Serializable]
    public class PlayerStatsUpgrade : UpgradeEffect
    {
        public PlayerStatType StatType;
        public StatModificationType Condition;
        public float Value;
        
        public override void ApplyEffect()
        {
            PlayerStatsController.Instance.ApplyStatsUpgrade(StatType, Condition, Value);
        }
    }
}