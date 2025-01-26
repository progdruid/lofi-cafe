using System;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class GameplayController : MonoBehaviour
{
    //fields////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Header("Settings")] 
    [SerializeField] private ItemData[] tutorialItems;
    [SerializeField] private ItemData[] items;
    [Header("Dependencies")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Slot orderDepositSlot;
    [SerializeField] private OrderPanel orderPanel;
    
    private Item _draggedItem = null;
    private Vector2 _smoothingVelocity;
    private Vector2 _mousePosition;

    private ItemData _currentOrder;
    private int _tutorialCounter = 0;
    
    //initialisation////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        Assert.IsNotNull(targetCamera);
        Assert.IsNotNull(orderDepositSlot);
        Assert.IsNotNull(orderPanel);
        
        orderDepositSlot.ItemChangeEvent += HandleSlotItemChange;
    }

    private void Start()
    {
        PlaceNextOrder();
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
                    var item = deposit.DropItem();
                    if (!item) continue;
                    _draggedItem = item;
                    _draggedItem.BeginDrag();
                    break;
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
        if (!current || _currentOrder != current.ItemData)
            return;
        
        orderDepositSlot.ItemChangeEvent -= HandleSlotItemChange;
        var item = orderDepositSlot.Pop();
        item.Delete();
        orderDepositSlot.ItemChangeEvent += HandleSlotItemChange;

        orderPanel.SetOrderTitleToCompleted();
        PlaceNextOrder();
    }

    private void PlaceNextOrder()
    {

        if (_tutorialCounter < tutorialItems.Length)
        {
            _currentOrder = tutorialItems[_tutorialCounter];
            _tutorialCounter++;
        }
        else
        {
            var randomIndex = Random.Range(0, items.Length);
            _currentOrder = items[randomIndex];
        }
        orderPanel.ChangeTo(_currentOrder);
    }
}
