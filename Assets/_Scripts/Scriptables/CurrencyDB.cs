using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "CurrencyDB", menuName = "ScriptableObjects/CurrencyDB", order = 1)]
public class CurrencyDB : ScriptableObject
{
    [SerializeField] 
    private List<CurrencySO> currencies = new List<CurrencySO>();

    // ---- Geçici form alanları ----
    [Title("New Currency Form")]
    public string newCurrencyName = "New Currency";

    [EnumToggleButtons]
    public CurrencyType newCurrencyType;

    [TextArea]
    public string newCurrencyDescription;

    [PreviewField(80)]
    public Sprite newCurrencyIcon;

#if UNITY_EDITOR
    [Button("Create Currency")]
    private void CreateCurrency()
    {
        string folderPath = "Assets/Data/Currencies";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/Data", "Currencies");
        }

        // Yeni asset oluştur
        var newCurrency = ScriptableObject.CreateInstance<CurrencySO>();
        newCurrency.currencyName = newCurrencyName;
        newCurrency.type = newCurrencyType;
        newCurrency.currencyDescription = newCurrencyDescription;
        newCurrency.currencyIcon = newCurrencyIcon;

        string path = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{newCurrencyName}.asset");
        AssetDatabase.CreateAsset(newCurrency, path);
        AssetDatabase.SaveAssets();

        // Listeye ekle
        currencies.Add(newCurrency);
        EditorUtility.SetDirty(this);

        Debug.Log($"Currency created: {newCurrencyName}");
    }
#endif
    
    public CurrencySO GetCurrencyByType(CurrencyType type)
    {
        return currencies.Find(c => c.type == type);
    }

    public List<CurrencySO> GetAllCurrencies()
    {
        return currencies;
    }
    public Sprite GetCurrencySprite(CurrencyType type)
    {
        var currency = currencies.Find(c => c.type == type);
        return currency != null ? currency.currencyIcon != null ? currency.currencyIcon : null : null;
    }
}