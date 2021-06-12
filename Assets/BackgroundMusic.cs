using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public List<AudioClip> Musics;

    private AudioSource _audioSource;
    private int         _current;

    private void Start()
    {
        
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(Musics[_current]);
            _current++;
            if (_current >= Musics.Count)
                _current = 0;
        }
    }
}
