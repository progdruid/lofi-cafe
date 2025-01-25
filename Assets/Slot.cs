using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    //static part///////////////////////////////////////////////////////////////////////////////////////////////////////
    private static readonly HashSet<Slot> Slots = new();
    public static IReadOnlyCollection<Slot> All => Slots;
    
    
    //fields////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Header("Settings")]
    [SerializeField] private float halfBoxSize = 1f;

    private Item _item;
    
    //initialisation////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        Slots.Add(this);
    }

    private void OnDestroy()
    {
        Slots.Remove(this);
    }

    
    //public interface//////////////////////////////////////////////////////////////////////////////////////////////////
    public event System.Action<Item, Item> ItemChangeEvent;
    
    public void Put(Item item)
    {
        Item previous = null;
        if (_item)
        {
            previous = _item;
            previous.Unlock();
        }
        
        _item = item;
        _item.Lock(transform.position.To2());
        
        ItemChangeEvent?.Invoke(previous, _item);
    }

    public Item Pop()
    {
        if (!_item) return null;
        _item.Unlock();
        var item = _item;
        _item = null;
        
        ItemChangeEvent?.Invoke(item, null);
        return item;
    }
    
    public bool IsPointOver(Vector2 point) =>
        point.x.Between(transform.position.x - halfBoxSize, transform.position.x + halfBoxSize) &&
        point.y.Between(transform.position.y - halfBoxSize, transform.position.y + halfBoxSize);
}
