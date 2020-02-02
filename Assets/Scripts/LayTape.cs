﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayTape : MonoBehaviour
{
    LineRenderer LR;
    public bool taping = false;
    Vector3 LongTape = new Vector3(0, 0, -10);

    public AudioClip[] TapeSoundEffects;
    AudioSource AS;

    public void Start()
    {
        LR = GetComponent<LineRenderer>();
        AS = GetComponent<AudioSource>();
    }

    public void EnableTape()
    {
        taping = true;
    }

    public void DisableTape()
    {
        taping = false;
    }

    private void Update()
    {
        if (taping)
        {
            LR.SetPosition(1, Vector3.Lerp(LR.GetPosition(1), LongTape, 0.1f));
            LoopSoundEffects();
        }
        else
        {
            LR.SetPosition(1, Vector3.Lerp(LR.GetPosition(1), Vector3.zero, 0.1f));
            if (AS.isPlaying)
            {
                AS.Stop();
            }
        }
    }

    void LoopSoundEffects()
    {
        if (AS.isPlaying)
        {
            return;
        }
        else
        {
            int i = Random.Range(0, TapeSoundEffects.Length);
            AS.Stop();
            AS.clip = TapeSoundEffects[i];
            AS.Play();
        }
    }
}