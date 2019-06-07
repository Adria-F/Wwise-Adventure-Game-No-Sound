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

    public void SetRTPC(float value)
    {
        if (Menu.isOpen)
        {
            //RTPC.SetGlobalValue(value);
            mixer.SetFloat(volumeName, value-80);
        }
    }

}
