////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;

public class ArcadeMachine : MonoBehaviour, IInteractable
{
    public bool isPlayingMusic = false;
    public ParticleSystem NoteParticles;

    public void OnInteract()
    {
        if (!isPlayingMusic)
        {
			isPlayingMusic = true;
            NoteParticles.Play();
            // Arcade music start
        }
        else
        {
			isPlayingMusic = false;
            NoteParticles.Stop();
            // Arcade music stop
        }

    }

    public void OnDestroy()
    {
        // Arcade music stop
    }

}
