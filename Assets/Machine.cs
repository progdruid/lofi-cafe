using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Machine : MonoBehaviour
{
    //fields////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] private int workPeriodInBeats;
    [SerializeField] private Recipe[] recipes;
    [SerializeField] private Slot[] slots;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private ItemData wasteItemData;
    
    private int _lastDropBeat = int.MinValue;
    
    private IReadOnlyCollection<ItemData> _productsToDrop = null;
    
    
    //initialisation////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        foreach (var slot in slots) 
            slot.ItemChangeEvent += HandleSlotChange;
    }

    private void Start() => Beat.Global.BeatEvent += HandleBeat;
    private void OnDestroy() => Beat.Global.BeatEvent -= HandleBeat;

    
    //game events///////////////////////////////////////////////////////////////////////////////////////////////////////
    private void HandleBeat(int beat)
    {
        if (beat < _lastDropBeat + workPeriodInBeats)
            return;
        
        Recipe foundRecipe = null;
        _productsToDrop = null;
        var emptySlots = 0;
        
        foreach (var slot in slots)
        {
            var item = slot.Get();
            if (!item)
                emptySlots++;
        }
        
        foreach (var recipe in recipes)
        {
            if (emptySlots != recipe.EmptySlots)
                continue;
            
            var unusedIngredients = new Dictionary<ItemData, int>( recipe.Ingredients);
            foreach (var slot in slots)
            {
                var item = slot.Get();
                if (!item) continue;
                
                Assert.IsNotNull(item.ItemData);

                if (!unusedIngredients.ContainsKey(item.ItemData)) 
                    break;
                
                unusedIngredients[item.ItemData]--;
                
                if (unusedIngredients[item.ItemData] <= 0)
                    unusedIngredients.Remove(item.ItemData);
            }
            
            if (unusedIngredients.Count != 0)
                continue;
            
            foundRecipe = recipe;
            break;
        }

        _productsToDrop = foundRecipe?.Products ?? (emptySlots < slots.Length ? new[] { wasteItemData } : null);
        if (_productsToDrop == null)
            return;
        
        foreach (var slot in slots)
        {
            slot.ItemChangeEvent -= HandleSlotChange;
            var item = slot.Pop();
            slot.ItemChangeEvent += HandleSlotChange;
            if (!item) continue;
            item.Delete();
        }
        
        foreach (var product in _productsToDrop)
        {
            var item = Instantiate(itemPrefab, dropPoint.position, Quaternion.identity).GetComponent<Item>();
            item.transform.SetWorldZ(0);
            item.ItemData = product;
        }
        
        _productsToDrop = null;
        _lastDropBeat = beat;
    }
    
    
    //private logic/////////////////////////////////////////////////////////////////////////////////////////////////////
    private void HandleSlotChange(Item previous, Item next)
    {
        _lastDropBeat = Beat.Global.CurrentBeat;
    }
}
