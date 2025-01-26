using System;
using System.Collections;
using UnityEngine;

public class Beat : MonoBehaviour
{
    public static Beat Global { get; private set; }
    [SerializeField] private float beatInterval;
    
    private int _currentBeat = 0;
    private float _intervalTimer = 0;
    
    private void Awake()
    {
        Global = this;
    }

    private void Start()
    {
        BeatEvent?.Invoke(_currentBeat);
        StartCoroutine(BeatCoroutine());
    }

    //public interface//////////////////////////////////////////////////////////////////////////////////////////////////
    public event System.Action<int> BeatEvent;
    public int CurrentBeat => _currentBeat;
    
    //game events///////////////////////////////////////////////////////////////////////////////////////////////////////
    private IEnumerator BeatCoroutine()
    {
        while (true)
        { 
            _intervalTimer += Time.deltaTime;

            if (_intervalTimer < beatInterval)
            {
                yield return null;
                continue;
            }
            _intervalTimer = 0;
        
            _currentBeat++;
            Debug.Log($"Beat: {_currentBeat}");
            BeatEvent?.Invoke(_currentBeat);
        }
    }
}
