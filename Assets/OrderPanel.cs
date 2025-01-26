using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class OrderPanel : MonoBehaviour
{
    [SerializeField] private Sprite[] frameSprites;
    [SerializeField] private float frameRate = 6f;
    [SerializeField] private float maxDescriptionHeight = 200f;

    private VisualElement _popupRoot;
    private VisualElement _popupPanel;
    private VisualElement _imageFrame;
    private Label _titleLabel;
    private Label _descriptionLabel;
    private Image _orderImage;

    private int _currentFrameIndex;
    private float _frameDuration;
    private Coroutine _animationCoroutine;

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

    public void ShowFor(ItemData itemData)
    {
        _orderImage.sprite = itemData.icon;
        _titleLabel.text = itemData.title;
        _descriptionLabel.text = itemData.description;
        _popupPanel.style.display = DisplayStyle.Flex;

        StartFrameAnimation();
    }
    
    public void Hide()
    {
        _popupPanel.style.display = DisplayStyle.None;

        StopFrameAnimation();
    }

    private void CreatePopupPanel()
    {
        _popupPanel = new VisualElement();
        _popupPanel.AddToClassList("order-popup");

        _popupPanel.style.position = Position.Absolute;
        _popupPanel.style.top = 20;
        _popupPanel.style.left = 20;
        _popupPanel.style.width = 350;
        _popupPanel.style.backgroundColor = new StyleColor(new Color(0.89f, 1.0f, 0.76f));
        _popupPanel.style.borderTopColor = new StyleColor(new Color(0.21f, 0.49f, 0.49f));
        _popupPanel.style.borderRightColor = new StyleColor(new Color(0.21f, 0.49f, 0.49f));
        _popupPanel.style.borderBottomColor = new StyleColor(new Color(0.21f, 0.49f, 0.49f));
        _popupPanel.style.borderLeftColor = new StyleColor(new Color(0.21f, 0.49f, 0.49f));
        _popupPanel.style.borderTopWidth = 8f;
        _popupPanel.style.borderRightWidth = 8f;
        _popupPanel.style.borderBottomWidth = 8f;
        _popupPanel.style.borderLeftWidth = 8f;
        _popupPanel.style.borderBottomLeftRadius = 8f;
        _popupPanel.style.borderBottomRightRadius = 8f;
        _popupPanel.style.borderTopLeftRadius = 8f;
        _popupPanel.style.borderTopRightRadius = 8f;
        _popupPanel.style.paddingLeft = 15f;
        _popupPanel.style.paddingRight = 15f;
        _popupPanel.style.paddingBottom = 5f;
        _popupPanel.style.paddingTop = 5f;
        _popupPanel.style.flexDirection = FlexDirection.Column;

        var topRow = new VisualElement
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                flexGrow = 0
            }
        };

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

        _orderImage = new Image
        {
            style =
            {
                width = 80,
                height = 80
            }
        };

        _imageFrame.Add(_orderImage);
        topRow.Add(_imageFrame);

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
                color = new Color(0.4f, 0.4f, 0.4f),
                flexWrap = Wrap.Wrap,
                whiteSpace = WhiteSpace.Normal
            }
        };
        contentContainer.Add(_descriptionLabel);

        topRow.Add(contentContainer);
        _popupPanel.Add(topRow);

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

            _imageFrame.style.backgroundImage = new StyleBackground(frameSprites[_currentFrameIndex]);
            _currentFrameIndex = (_currentFrameIndex + 1) % frameSprites.Length;
            yield return new WaitForSeconds(_frameDuration);
        }
    }
}