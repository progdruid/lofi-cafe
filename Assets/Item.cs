using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class Item : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float drag = 0.5f;
    [SerializeField] private float smoothTime = 0.1f;
    [Header("Dependencies")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CircleCollider2D circleCollider;
    
    private Camera _mainCamera;
    private bool _isDragging = false;
    private Vector2 _offset;
    private Vector2 _smoothingVelocity = Vector2.zero;

    private void Awake()
    {
        Assert.IsNotNull(rb);
        Assert.IsNotNull(circleCollider);
        
        rb.gravityScale = gravityScale;
        rb.linearDamping = drag;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;

        _mainCamera = Camera.main;
    }

    private void Update()
    {
        var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition).To2();
        if (Input.GetMouseButtonDown(0))
        {
            if (circleCollider.OverlapPoint(mousePosition))
            {
                _isDragging = true;
                _offset = (Vector2)transform.position - mousePosition;
                rb.gravityScale = 0;
                rb.linearVelocity = Vector2.zero;
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            rb.gravityScale = gravityScale;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
            rb.linearVelocity = _smoothingVelocity;
        }

        if (!_isDragging)
            return;
        
        var targetPosition = mousePosition + _offset;
        var smoothedPosition = rb.position.SmoothDamp(targetPosition, ref _smoothingVelocity, smoothTime);
        rb.MovePosition(smoothedPosition);
    }
}
