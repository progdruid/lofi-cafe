using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Item : MonoBehaviour
{
    //static part///////////////////////////////////////////////////////////////////////////////////////////////////////
    private static readonly HashSet<Item> Items = new();
    public static IReadOnlyCollection<Item> All => Items;
    
    
    //fields////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Header("Settings")]
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float drag = 0.5f;
    [SerializeField] private float smoothTime = 0.1f;
    [Header("Dependencies")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CircleCollider2D circleCollider;
    
    private Vector2 _smoothingVelocity = Vector2.zero;
    
    
    //initialisation////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        Assert.IsNotNull(rb);
        Assert.IsNotNull(circleCollider);
        
        EndDrag();
        
        Items.Add(this);
    }

    private void OnDestroy()
    {
        Items.Remove(this);
    }

    
    //public interface//////////////////////////////////////////////////////////////////////////////////////////////////
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
}
