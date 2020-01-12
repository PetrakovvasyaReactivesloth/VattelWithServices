using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnEnabled : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _animator.Rebind();
        transform.rotation = Quaternion.identity;
    }

    private void OnDisable()
    {
    
        _animator.Rebind();
        transform.rotation = Quaternion.identity;
    }
}
