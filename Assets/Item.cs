using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "Cafe/Item")]
public class ItemData : ScriptableObject
{
    public string title;
    public string description;
    public Sprite icon;
}

public class Item : MonoBehaviour
{
    //static part///////////////////////////////////////////////////////////////////////////////////////////////////////
    private static readonly HashSet<Item> Items = new();
    public static IReadOnlyCollection<Item> All => Items;
    
    
    //fields////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Header("Movement")]
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float drag = 0.5f;
    [SerializeField] private float smoothTime = 0.1f;
    [Header("Dependencies")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private Vector2 _smoothingVelocity = Vector2.zero;
    
    private ItemData _itemData;
    
    //initialisation////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        Assert.IsNotNull(rb);
        Assert.IsNotNull(circleCollider);
        Assert.IsNotNull(spriteRenderer);
        
        EndDrag();
        
        Items.Add(this);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Items.Remove(this);
    }

    
    //public interface//////////////////////////////////////////////////////////////////////////////////////////////////
    public ItemData ItemData
    {
        get => _itemData;
        set
        {
            _itemData = value;
            if (_itemData)
            {
                spriteRenderer.sprite = _itemData.icon;
                gameObject.SetActive(true);
            }
            else
            {
                
                spriteRenderer.sprite = null;
                gameObject.SetActive(false);
            }
        }
    }

    public bool IsPointOver(Vector2 point) => circleCollider.OverlapPoint(point);

    public void BeginDrag()
    {
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _smoothingVelocity = Vector2.zero;
    }

    public void EndDrag()
    {
        rb.gravityScale = gravityScale;
        rb.linearDamping = drag;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        //rb.linearVelocity = _smoothingVelocity;
    }

    public void DragTo(Vector2 point)
    {
        var smoothedPosition = rb.position.SmoothDamp(point, ref _smoothingVelocity, smoothTime);
        rb.MovePosition(smoothedPosition);
    }

    public void Lock(Vector2 anchor)
    {
        Items.Remove(this);

        transform.SetWorldXY(anchor);
        rb.simulated = false;
        circleCollider.enabled = false;
    }

    public void Unlock()
    {
        circleCollider.enabled = true;
        rb.simulated = true;
        
        Items.Add(this);
    }

    public void Delete()
    {
        Items.Remove(this);
        Destroy(rb.gameObject);
        if (this)
            Destroy(this);
    }
}
