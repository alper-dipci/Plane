using UnityEngine;
using Sirenix.OdinInspector;

public class CurrencySO : ScriptableObject
{
    [HorizontalGroup("Row", Width = 300)] 
    [VerticalGroup("Row/Left")]
    [LabelText("Name")]
    public string currencyName;

    [VerticalGroup("Row/Left")]
    [EnumToggleButtons, LabelText("Type")]
    public CurrencyType type;

    [VerticalGroup("Row/Left")]
    [Multiline(3), LabelText("Description")]
    public string currencyDescription;

    [VerticalGroup("Row/Right")]
    [PreviewField(80, ObjectFieldAlignment.Center), HideLabel]
    public Sprite currencyIcon;
}