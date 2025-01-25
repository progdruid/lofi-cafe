using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class IngredientDeposit : MonoBehaviour
{
    // static part///////////////////////////////////////////////////////////////////////////////////////////////////////
    private static readonly HashSet<IngredientDeposit> Deposits = new();
    public static IReadOnlyCollection<IngredientDeposit> All => Deposits;
    
    
    //fields////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Header("Settings")]
    [SerializeField] private ItemData itemData;
    [Header("Dependencies")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private GameObject itemPrefab;
    
    private float _halfWidth, _halfHeight;
    
    
    //initialisation////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        Assert.IsNotNull(itemData);
        Assert.IsNotNull(spriteRenderer);
        Assert.IsNotNull(dropPoint);
        Assert.IsNotNull(itemPrefab);

        _halfWidth = spriteRenderer.bounds.size.x / 2f;
        _halfHeight = spriteRenderer.bounds.size.y / 2f;
        
        Deposits.Add(this);
    }

    private void OnDestroy()
    {
        Deposits.Remove(this);
    }


    //public interface//////////////////////////////////////////////////////////////////////////////////////////////////
    public bool IsPointOver(Vector2 point) =>
        point.x.Between(transform.position.x - _halfWidth, transform.position.x + _halfWidth) &&
        point.y.Between(transform.position.y - _halfHeight, transform.position.y + _halfHeight);

    public void DropItem()
    {
        var item = Instantiate(itemPrefab, dropPoint.position, Quaternion.identity).GetComponent<Item>();
        item.transform.SetWorldZ(0);
        item.ItemData = itemData;
    }
}
