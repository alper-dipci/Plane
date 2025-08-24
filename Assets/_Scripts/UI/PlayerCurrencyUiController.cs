using System;
using System.Collections.Generic;
using _Scripts.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace _Scripts.UI
{
    public class PlayerCurrencyUiController : MonoBehaviour
    {
        [SerializeField] private PlayerStatsSO playerStats;

        [SerializeField] private CurrencyDB currencyDB;

        [SerializeField] private CurrencyUi playerCurrencyUiPrefab;

        private List<CurrencyUi> currencyUiInstances = new List<CurrencyUi>();

        private void Start()
        {
            UpdateList();
        }

        [Button]
        public void UpdateList()
        {
            // check unlocked currencies
            var currentCurrencies = playerStats.Currencies;
            foreach (var currency in currentCurrencies)
            {
                if (currency.isUnlocked)
                {
                    var existingUi = currencyUiInstances.Find(ui => ui.currencyType == currency.currencyType);
                    if (existingUi == null)
                    {
                        var currencyUiInstance = Instantiate(playerCurrencyUiPrefab, transform);
                        currencyUiInstance.currencyText.text = currency.amount.ToString();
                        currencyUiInstance.currencyImage.sprite =
                            currencyDB.GetCurrencySprite(currency.currencyType);
                        currencyUiInstance.currencyType = currency.currencyType;

                        currencyUiInstances.Add(currencyUiInstance);
                    }
                    else
                    {
                        existingUi.currencyText.text = currency.amount.ToString();
                    }
                }
                else
                {
                    var existingUi = currencyUiInstances.Find(ui => ui.currencyType == currency.currencyType);
                    if (existingUi != null)
                    {
                        currencyUiInstances.Remove(existingUi);
                        Destroy(existingUi.gameObject);
                    }
                }
            }
        }
    }
}
