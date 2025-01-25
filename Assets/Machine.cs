using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "Recipe", menuName = "Cafe/Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    [SerializeField] private SerializableMap<ItemData, int> ingredients;
    [SerializeField] private ItemData[] products;
    [SerializeField] private int emptySlots;
    
    public IReadOnlyCollection<ItemData> Products => products;
    public IReadOnlyDictionary<ItemData, int> Ingredients => ingredients;
    public int EmptySlots => emptySlots;
}

public class Machine : MonoBehaviour
{
    //fields////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] private Recipe[] recipes;
    [SerializeField] private Slot[] slots;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private GameObject itemPrefab;
    
    [Header("Animation")] 
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float fps = 10f;
    [SerializeField] private SpriteRenderer spriteRenderer; 
    
    private float _frameTimer = 0f;
    private int _currentFrame = 0;
    
    
    //initialisation////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        foreach (var slot in slots) 
            slot.ItemChangeEvent += HandleSlotChange;
    }
    
    //game events///////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        // Animation logic
        if (sprites.Length <= 0) 
            return;
        
        _frameTimer += Time.deltaTime;

        if (_frameTimer < 1f / fps) 
            return;
        
        _frameTimer = 0f;
        _currentFrame = (_currentFrame + 1) % sprites.Length; // Loop back to 0 when the end of the array is reached
        spriteRenderer.sprite = sprites[_currentFrame]; // Set the current sprite
    }
    
    //private logic/////////////////////////////////////////////////////////////////////////////////////////////////////
    private void HandleSlotChange(Item previous, Item next)
    {
        Recipe foundRecipe = null;
        
        foreach (var recipe in recipes)
        {
            var ingredients = new Dictionary<ItemData, int>( recipe.Ingredients);
            var emptySlots = 0;
            foreach (var slot in slots)
            {
                var item = slot.Get();
                if (!item)
                {
                    emptySlots++;
                    continue;
                }
                
                Assert.IsNotNull(item.ItemData);

                if (!ingredients.ContainsKey(item.ItemData)) 
                    break;
                
                ingredients[item.ItemData]--;
                
                if (ingredients[item.ItemData] <= 0)
                    ingredients.Remove(item.ItemData);
            }

            if (emptySlots != recipe.EmptySlots)
                continue;
            
            foundRecipe = recipe;
            break;
        }

        if (!foundRecipe)
            return;
        
        foreach (var slot in slots)
        {
            slot.ItemChangeEvent -= HandleSlotChange;
            var item = slot.Pop();
            slot.ItemChangeEvent += HandleSlotChange;
            if (!item) continue;
            item.Delete();
        }
        
        foreach (var product in foundRecipe.Products)
        {
            var item = Instantiate(itemPrefab, dropPoint.position, Quaternion.identity).GetComponent<Item>();
            item.ItemData = product;
        }
    }
}
