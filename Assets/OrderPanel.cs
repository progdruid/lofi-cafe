using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class OrderPanel : MonoBehaviour
{
    //fields////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] private Sprite[] frameSprites;
    [SerializeField] private float frameRate = 6f;

    private VisualElement _popupRoot;
    private VisualElement _popupPanel;
    private VisualElement _imageFrame; // Frame container
    private Label _titleLabel;
    private Label _descriptionLabel;
    private Image _orderImage;

    private int _currentFrameIndex;
    private float _frameDuration;
    private Coroutine _animationCoroutine;

    //initialisation////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        Assert.IsTrue(frameSprites.Length > 0);
    }

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        _popupRoot = uiDocument.rootVisualElement;

        CreatePopupPanel();

        Hide();
    }

    //public interface//////////////////////////////////////////////////////////////////////////////////////////////////
    public void ShowFor(ItemData itemData)
    {
        _orderImage.image = itemData.icon.texture;
        _orderImage.sourceRect = itemData.icon.rect;
        _titleLabel.text = itemData.title;
        _descriptionLabel.text = itemData.description;
        _popupPanel.style.display = DisplayStyle.Flex;

        StartFrameAnimation();
    }
    
    public void Hide()
    {
        _popupPanel.style.display = DisplayStyle.None;

        StopFrameAnimation(); // Stop animating the frame
    }

    //private logic/////////////////////////////////////////////////////////////////////////////////////////////////////
    private void CreatePopupPanel()
    {
        // Main panel container
        _popupPanel = new VisualElement();
        _popupPanel.AddToClassList("order-popup");

        _popupPanel.style.position = Position.Absolute;
        _popupPanel.style.top = 20;
        _popupPanel.style.left = 20;
        _popupPanel.style.width = 350;
        _popupPanel.style.backgroundColor = new StyleColor(new Color(0.89f, 1.0f, 0.76f)); // E3FFC3
        _popupPanel.style.borderTopColor = new StyleColor(new Color(0.21f, 0.49f, 0.49f)); // 357C7D
        _popupPanel.style.borderRightColor = new StyleColor(new Color(0.21f, 0.49f, 0.49f)); // 357C7D
        _popupPanel.style.borderBottomColor = new StyleColor(new Color(0.21f, 0.49f, 0.49f)); // 357C7D
        _popupPanel.style.borderLeftColor = new StyleColor(new Color(0.21f, 0.49f, 0.49f)); // 357C7D
        _popupPanel.style.borderTopWidth = 2f;
        _popupPanel.style.borderRightWidth = 2f;
        _popupPanel.style.borderBottomWidth = 2f;
        _popupPanel.style.borderLeftWidth = 2f;
        _popupPanel.style.borderBottomLeftRadius = 8f;
        _popupPanel.style.borderBottomRightRadius = 8f;
        _popupPanel.style.borderTopLeftRadius = 8f;
        _popupPanel.style.borderTopRightRadius = 8f;
        _popupPanel.style.paddingLeft = 15f;
        _popupPanel.style.flexDirection = FlexDirection.Row;

        // Create the frame element
        _imageFrame = new VisualElement
        {
            style =
            {
                width = 100,
                height = 100,
                marginRight = 15f,
                marginTop = 10f,
                marginBottom = 10f,
                justifyContent = Justify.Center,
                alignItems = Align.Center
            }
        };

        // Create the image inside the frame
        _orderImage = new Image
        {
            style =
            {
                width = 80,
                height = 80
            }
        };

        _imageFrame.Add(_orderImage);
        _popupPanel.Add(_imageFrame); // Add the frame element

        // Create labels
        var contentContainer = new VisualElement
        {
            style =
            {
                flexGrow = 1,
                justifyContent = Justify.Center,
                alignItems = Align.FlexStart
            }
        };

        _titleLabel = new Label("Order Details")
        {
            style =
            {
                fontSize = 20,
                unityFontStyleAndWeight = FontStyle.Bold
            }
        };
        contentContainer.Add(_titleLabel);

        _descriptionLabel = new Label("No order selected")
        {
            style =
            {
                fontSize = 16,
                color = new Color(0.4f, 0.4f, 0.4f)
            }
        };
        contentContainer.Add(_descriptionLabel);

        _popupPanel.Add(contentContainer);

        _popupRoot.Add(_popupPanel);

        Hide();
    }

    private void StartFrameAnimation()
    {
        if (_animationCoroutine == null)
        {
            _frameDuration = 1f / frameRate;
            _animationCoroutine = StartCoroutine(AnimateFrame());
        }
    }

    private void StopFrameAnimation()
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
            _animationCoroutine = null;
        }
    }

    private IEnumerator AnimateFrame()
    {
        while (true)
        {
            if (frameSprites.Length == 0) yield break;

            // Update the frame's background image
            _imageFrame.style.backgroundImage = new StyleBackground(frameSprites[_currentFrameIndex]);
            _currentFrameIndex = (_currentFrameIndex + 1) % frameSprites.Length;
            yield return new WaitForSeconds(_frameDuration);
        }
    }
}
