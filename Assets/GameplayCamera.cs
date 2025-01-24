using UnityEngine;
using UnityEngine.Serialization;

public class GameplayCamera : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float moveRadius = 10f;
    [SerializeField] private float smoothTime = 0.1f;
    
    private Vector3 _velocity = Vector3.zero;

    private void LateUpdate()
    {
        var motionDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized;
        if (motionDirection.ApproximatelyEqual(Vector3.zero))
            _velocity = Vector3.zero;
        
        var targetPosition = transform.position + motionDirection * moveRadius;
        if (motionDirection.magnitude < 0.01f)
            Debug.Log($"{targetPosition - transform.position}");
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
    }
}
