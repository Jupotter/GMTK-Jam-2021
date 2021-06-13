using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BallSound))]
public class BallSound : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var magnitude = other.relativeVelocity.magnitude;
        if (magnitude > 3)
            _audioSource.Play();
    }
}
