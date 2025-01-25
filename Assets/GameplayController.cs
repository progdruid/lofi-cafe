using System;
using UnityEngine;
using UnityEngine.Assertions;

public class GameplayController : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    private Item _draggedItem = null;
    private Vector2 _smoothingVelocity;
    private Vector2 _mousePosition;
    
    private void Awake()
    {
        Assert.IsNotNull(targetCamera);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }        
        
        _mousePosition = targetCamera.ScreenToWorldPoint(Input.mousePosition).To2();
        if (Input.GetMouseButtonDown(0) && !_draggedItem)
        {
            foreach (var item in Item.All)
            {
                if (!item.IsPointOver(_mousePosition)) continue;
                _draggedItem = item;
                _draggedItem.BeginDrag();
                break;
            }

            if (!_draggedItem)
            {
                foreach (var slot in Slot.All)
                {
                    if (!slot.IsPointOver(_mousePosition)) continue;
                    var item = slot.Pop();
                    if (!item) continue;
                    _draggedItem = item;
                    _draggedItem.BeginDrag();
                    break;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && _draggedItem)
        {
            _draggedItem.EndDrag();
            foreach (var slot in Slot.All)
            {
                if (!slot.IsPointOver(_mousePosition)) continue;
                slot.Put(_draggedItem); 
                break;
            }
            _draggedItem = null;
        }
    }

    private void FixedUpdate()
    {
        _draggedItem?.DragTo(_mousePosition);
    }
}
