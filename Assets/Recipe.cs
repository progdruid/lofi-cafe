using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Cafe/Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    [SerializeField] private SerializableMap<ItemData, int> ingredients;
    [SerializeField] private int emptySlots;
    [SerializeField] private ItemData[] products;
    
    public IReadOnlyCollection<ItemData> Products => products;
    public IReadOnlyDictionary<ItemData, int> Ingredients => ingredients;
    public int EmptySlots => emptySlots;
}