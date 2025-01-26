using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Intro : MonoBehaviour
{
    private VisualElement _backgroundElement;
    private Coroutine _fadeCoroutine;

    private const float FadeDuration = 2.0f; // Duration of the fade (in seconds)

    private void OnEnable()
    {
        // Access the UI Document's root visual element
        var uiDocument = GetComponent<UIDocument>();
        var rootElement = uiDocument.rootVisualElement;

        CreateBackground(rootElement);
        StartFadeIn(); // Start the fade-in effect on enable
    }

    private void CreateBackground(VisualElement rootElement)
    {
        // Create the background element that will transition
        _backgroundElement = new VisualElement
        {
            style =
            {
                position = Position.Absolute,
                top = 0,
                left = 0,
                width = Screen.width,
                height = Screen.height,
                backgroundColor = new Color(1, 1, 1, 1), // Start with white background
                display = DisplayStyle.Flex
            }
        };

        // Add the background element to the root visual element
        rootElement.Add(_backgroundElement);
    }

    private void StartFadeIn()
    {
        // Stop any ongoing fade animation and start a new one
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        float elapsedTime = 0;
        Color startColor = _backgroundElement.style.backgroundColor.value;
        Color targetColor = new Color(1, 1, 1, 0); // Transparent color

        // Fade from white to transparent
        while (elapsedTime < FadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / FadeDuration;

            // Lerp between the start color and the target color
            _backgroundElement.style.backgroundColor = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        // Ensure the background is fully transparent at the end
        _backgroundElement.style.backgroundColor = targetColor;

        // Destroy the background element after the fade
        Destroy(gameObject);
    }
}
