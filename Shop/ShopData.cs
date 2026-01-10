using NUnit.Framework;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShop", menuName = "Ithaca/Shop")]
public class ShopData : ScriptableObject
{
    [TextArea(2, 4)][SerializeField] public string dialogueOptionLabel;

    [SerializeField] public List<ItemData> items;
}
