using UnityEngine;
using UnityEngine.Assertions;

public class CycleAnimator : MonoBehaviour
{
    [Header("Animation")] 
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float fps = 10f;
    [SerializeField] private SpriteRenderer spriteRenderer; 
    
    private float _oneFrameTime;
    private float _frameTimer = 0f;
    private int _currentFrame = 0;
    
    
    private void Awake()
    {
        Assert.IsTrue(sprites.Length > 0);
        Assert.IsNotNull(spriteRenderer);
        
        _oneFrameTime = 1f / fps;
    }
    
    private void Update()
    {
        _frameTimer += Time.deltaTime;

        if (_frameTimer < _oneFrameTime) 
            return;
        
        _frameTimer = 0f;
        _currentFrame = (_currentFrame + 1) % sprites.Length;
        spriteRenderer.sprite = sprites[_currentFrame];
    }
}
