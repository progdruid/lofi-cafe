using System;
using UnityEngine;
using UnityEngine.Assertions;

public class GameplayController : MonoBehaviour
{
    //fields////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Header("Settings")] 
    [SerializeField] private ItemData placedItem;
    [SerializeField] private GameObject itemPrefab;
    [Header("Dependencies")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Slot orderDepositSlot;
    [SerializeField] private OrderPanel orderPanel;
    
    private Item _draggedItem = null;
    private Vector2 _smoothingVelocity;
    private Vector2 _mousePosition;

    private ItemData _currentOrder;
    
    //initialisation////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        Assert.IsNotNull(placedItem);
        Assert.IsNotNull(itemPrefab);
        Assert.IsNotNull(targetCamera);
        Assert.IsNotNull(orderDepositSlot);
        Assert.IsNotNull(orderPanel);

        _currentOrder = placedItem;
        
        orderDepositSlot.ItemChangeEvent += HandleSlotItemChange;
        
        var newItem = Instantiate(itemPrefab, new Vector3(10, 10, 0), Quaternion.identity).GetComponent<Item>();
        newItem.ItemData = placedItem;
    }

    //game events///////////////////////////////////////////////////////////////////////////////////////////////////////
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
                foreach (var slot in Slot.All)
                {
                    if (!slot.IsPointOver(_mousePosition)) continue;
                    var item = slot.Pop();
                    if (!item) continue;
                    _draggedItem = item;
                    _draggedItem.BeginDrag();
                    break;
                }

            if (!_draggedItem)
                foreach (var deposit in IngredientDeposit.All)
                {
                    if (!deposit.IsPointOver(_mousePosition)) continue;
                    deposit.DropItem();
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
    
    //private logic/////////////////////////////////////////////////////////////////////////////////////////////////////
    private void HandleSlotItemChange(Item previous, Item current)
    {
        if (_currentOrder != current.ItemData)
            return;
        
        orderDepositSlot.ItemChangeEvent -= HandleSlotItemChange;
        var item = orderDepositSlot.Pop();
        item.Delete();
        orderDepositSlot.ItemChangeEvent += HandleSlotItemChange;
        
        orderPanel.SetOrderTitleToCompleted();
        orderPanel.ChangeTo(current.ItemData);

        var newItem = Instantiate(itemPrefab, new Vector3(10, 10, 0), Quaternion.identity).GetComponent<Item>();
        newItem.ItemData = placedItem;
    }
    
}
