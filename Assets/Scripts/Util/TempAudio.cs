using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TempAudio : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        this.audioSource = base.GetComponent<AudioSource>();
    }

    private IEnumerator DestroyAfterTime(float t)
    {
        bool disposing = true;
        yield return new WaitForSeconds(t);
        PoolingSystem.DestroyAPS(this.gameObject);
    }

public void SetAudio(AudioClip clip, float time)
    {
        this.audioSource.Stop();
        this.audioSource.clip = clip;
        this.audioSource.Play();
        base.StartCoroutine(this.DestroyAfterTime(time));
    }
}
