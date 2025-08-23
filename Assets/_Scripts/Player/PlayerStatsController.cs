using _Scripts.Enums;
using _Scripts.Upgrades.UpgradeEffects;
using Helpers;
using UnityEngine;

namespace _Scripts.Player
{
    //test icin eklendi silinicek
    public class PlayerStatsController : SingletonMonoBehaviour<PlayerStatsController>
    {
        public void ApplyStatsUpgrade(PlayerStatType stat, StatModificationType type, float value)
        {
            Debug.Log($"Applied {stat} upgrade with {type} of {value}");
        }
    }
}