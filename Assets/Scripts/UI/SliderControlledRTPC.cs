////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEngine.Audio;

public class SliderControlledRTPC : MonoBehaviour
{
    //public AK.Wwise.RTPC RTPC;
    public AudioMixer mixer;
    public string volumeName;
    public float minVolume = 0.0f;

    float initialVolume = 0.0f;

    private void Start()
    {
        mixer.GetFloat(volumeName, out initialVolume);
    }

    public void SetRTPC(float value)
    {
        if (Menu.isOpen)
        {
            //RTPC.SetGlobalValue(value);
            float normal = Mathf.InverseLerp(0f, 100f, value);
            float volume = Mathf.Lerp(minVolume, initialVolume, normal);

            mixer.SetFloat(volumeName, volume);
        }
    }

}
