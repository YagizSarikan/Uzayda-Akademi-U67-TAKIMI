using System;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] Sprite[] _sprites;
    [SerializeField] float _frameDelay = 0.25f;

    float _nextFrameTime;
    int _frame = 0;

    void OnEnable()
    {
        _nextFrameTime = Time.time + _frameDelay;
        _frame = 0;
    }

    void LateUpdate()
    {
        if (Time.time >= _nextFrameTime)
        {
            AdvanceToNextFrame();
            _nextFrameTime = Time.time + _frameDelay;
        }
    }

    void AdvanceToNextFrame()
    {
        _frame = ++_frame % _sprites.Length;
        _renderer.sprite = _sprites[_frame];
    }
}
