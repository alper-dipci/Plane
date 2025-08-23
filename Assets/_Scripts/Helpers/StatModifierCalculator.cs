using _Scripts.Enums;
using _Scripts.Upgrades.UpgradeEffects;

namespace _Scripts.Helpers
{
    public static class StatModifierCalculator
    {
        public static float ApplyCondition(StatModificationType condition, float baseValue, float modifier)
        {
            return condition switch
            {
                StatModificationType.Add => baseValue + modifier,
                StatModificationType.Multiply => baseValue * modifier,
                _ => baseValue
            };
        }
    }
}