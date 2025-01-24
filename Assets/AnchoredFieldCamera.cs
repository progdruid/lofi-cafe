using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class AnchoredFieldCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float zoomTime = 0.01f;
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float zoomInThreshold = 0.2f;
    [SerializeField] private float zoomOutThreshold = 0.8f;
    [SerializeField] private float groundY = -4f;
    
    [Header("Dependencies")]
    [SerializeField] private Transform anchoredTransform;
    [SerializeField] private Camera targetCamera;

    private float _maxOffset;
    private float _zoomVelocity;
    
    private void Awake()
    {
        Assert.IsNotNull(anchoredTransform);
        Assert.IsNotNull(targetCamera);
        
        _maxOffset = maxZoom * targetCamera.aspect;
    }

    private void LateUpdate()
    {
        Vector2 viewportAnchor = targetCamera.WorldToViewportPoint(anchoredTransform.position);
        Vector2 viewportPoint = targetCamera.ScreenToViewportPoint(Input.mousePosition);
        var distanceInField = (viewportPoint - viewportAnchor).magnitude;
        if (distanceInField < zoomInThreshold)
            targetCamera.orthographicSize = Mathf.SmoothDamp(targetCamera.orthographicSize, minZoom, ref _zoomVelocity, zoomTime);
            //targetCamera.orthographicSize = targetCamera.orthographicSize.MoveTo(minZoom, zoomSpeed * Time.deltaTime);
        else if (distanceInField > zoomOutThreshold)
            targetCamera.orthographicSize = Mathf.SmoothDamp(targetCamera.orthographicSize, maxZoom, ref _zoomVelocity, zoomTime);
            //targetCamera.orthographicSize = targetCamera.orthographicSize.MoveTo(maxZoom, zoomSpeed * Time.deltaTime);
        
        var middle = targetCamera.ViewportToWorldPoint((viewportPoint + viewportAnchor) * 0.5f);
        middle.y = (middle.y - targetCamera.orthographicSize).LowerBound(groundY) + targetCamera.orthographicSize;
        targetCamera.transform.SetWorldXY(middle);
    }
}
