using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "ScriptableObjects/PlayerStatsSO", order = 1)]
    public class PlayerStatsSO : SerializedScriptableObject
    {
        [Header("Player Stats")] public float maxHealth = 100f;
        public float maxFuel = 100f;
        public float fuelConsumptionRate = 1f; // Fuel consumption rate per second
        public float fuelRegenRate = 5f; // Fuel regeneration rate per second when not consuming fuel

        [Header("Currency")] [ListDrawerSettings(ShowItemCount = true)]
        public List<PlayerCurrency> Currencies = new List<PlayerCurrency>();

        public void AddCurrency(CurrencyType currencyType, float amount)
        {
            var currency = Currencies.Find(c => c.currencyType == currencyType);
            if (currency != null)
            {
                currency.amount += (int)amount;
            }
            else
            {
                Currencies.Add(new PlayerCurrency
                {
                    currencyType = currencyType,
                    amount = (int)amount,
                    isUnlocked = true
                });
            }
        }

        public bool SpendCurrency(CurrencyType currencyType, float amount)
        {
            var currency = Currencies.Find(c => c.currencyType == currencyType);
            if (currency != null && currency.amount >= amount)
            {
                currency.amount -= (int)amount;
                return true;
            }
            return false;
        }
        
        public int GetCurrencyAmount(CurrencyType currencyType)
        {
            var currency = Currencies.Find(c => c.currencyType == currencyType);
            return currency != null ? currency.amount : 0;
        }
        
        public bool IsCurrencyUnlocked(CurrencyType currencyType)
        {
            var currency = Currencies.Find(c => c.currencyType == currencyType);
            return currency != null && currency.isUnlocked;
        }
        
        public void UnlockCurrency(CurrencyType currencyType)
        {
            var currency = Currencies.Find(c => c.currencyType == currencyType);
            if (currency != null)
            {
                currency.isUnlocked = true;
            }
            else
            {
                Currencies.Add(new PlayerCurrency
                {
                    currencyType = currencyType,
                    amount = 0,
                    isUnlocked = true
                });
            }
        }
        
        
    }

    [Serializable]
    public class PlayerCurrency
    {
        public CurrencyType currencyType;
        public int amount;
        public bool isUnlocked;
    }
}