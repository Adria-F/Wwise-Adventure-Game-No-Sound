////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnAnimationEvent : MonoBehaviour {

    // HINT: Expose the sound to be played in this animation
    //public List<AK.Wwise.Event> Sounds = new List<AK.Wwise.Event>();

    public void PlaySoundWithIdx(int idx){
        // HINT: Play sound with index idx synchronized with animation
        //Sounds[idx].Post(gameObject);
    }
}
