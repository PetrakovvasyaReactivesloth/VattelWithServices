using System;
using UnityEngine;

public class ScaleJumping : MonoBehaviour
{
    public float minScale = 1, maxScale = 2;
    public float scalingSpeed = 1;
    private Transform _transform;
    private Vector3 _startingScale;
    
    private void Awake()
    {
        _transform = transform;
        _startingScale = transform.localScale;
    }

    private void Update()
    {
        Vector3 localScale = Vector3.one * (1 + Mathf.PingPong(Time.unscaledTime * scalingSpeed, maxScale - 1));
        _transform.localScale = localScale;
    }

    private void OnDisable()
    {
        _transform.localScale = _startingScale;
    }
}