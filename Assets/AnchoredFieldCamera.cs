using UnityEngine;
using UnityEngine.Assertions;

public class AnchoredFieldCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float zoomTime = 0.01f;
    [SerializeField] private float motionTime = 0.01f;
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float zoomInThreshold = 0.2f;
    [SerializeField] private float zoomOutThreshold = 0.8f;
    [SerializeField] private float groundY = -4f;
    
    [Header("Dependencies")]
    [SerializeField] private Transform anchoredTransform;
    [SerializeField] private Camera targetCamera;

    private float _zoomVelocity;
    private Vector2 _motionVelocity;
    
    private void Awake()
    {
        Assert.IsNotNull(anchoredTransform);
        Assert.IsNotNull(targetCamera);
    }

    private void LateUpdate()
    {
        Vector2 viewportAnchor = targetCamera.WorldToViewportPoint(anchoredTransform.position);
        Vector2 viewportPoint = targetCamera.ScreenToViewportPoint(Input.mousePosition);
        var distanceInField = (viewportPoint - viewportAnchor).magnitude;
        if (distanceInField < zoomInThreshold)
            targetCamera.orthographicSize = targetCamera.orthographicSize.SmoothDamp(minZoom, ref _zoomVelocity, zoomTime);
        else if (distanceInField > zoomOutThreshold)
            targetCamera.orthographicSize = targetCamera.orthographicSize.SmoothDamp(maxZoom, ref _zoomVelocity, zoomTime);

        var middle = targetCamera.ViewportToWorldPoint((viewportAnchor + viewportPoint) * 0.5f).To2();
        var targetPos = targetCamera.transform.position.To2().SmoothDamp(middle, ref _motionVelocity, motionTime);
        targetPos.y = (targetPos.y - targetCamera.orthographicSize).ClampBottom(groundY) + targetCamera.orthographicSize;
        targetCamera.transform.SetWorldXY(targetPos);
    }
}
