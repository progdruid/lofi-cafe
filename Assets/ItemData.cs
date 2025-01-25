using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "Cafe/Item")]
public class ItemData : ScriptableObject
{
    public string title;
    public string description;
    public Sprite icon;
}