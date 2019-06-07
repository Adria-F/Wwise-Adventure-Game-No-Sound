////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    public MaterialChecker materialChecker;

    #region private variables
    private bool inWater;
    #endregion

    public void PlayFootstepSound()
    {
        SoundMaterial.Materials mat = materialChecker.CheckMaterial(gameObject);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(GetRandomAudio(mat), 0.5F);

        // HINT: Play footstep sound here
    }

    public AudioClip GetRandomAudio(SoundMaterial.Materials mat)
    {
        AudioClip[] audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().dirtSteps;

        switch (mat)
        {
            case SoundMaterial.Materials.DIRT:
                audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().dirtSteps;
                break;
            case SoundMaterial.Materials.GRASS:
                audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().grassSteps;
                break;
            case SoundMaterial.Materials.RUBBLE:
                audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().rubbleSteps;
                break;
            case SoundMaterial.Materials.SAND:
                audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().sandSteps;
                break;
            case SoundMaterial.Materials.STONE:
                audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().stoneSteps;
                break;
            case SoundMaterial.Materials.WATER:
                audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().waterSteps;
                break;
            case SoundMaterial.Materials.WOOD:
                audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().woodSteps;
                break;
        }

        return audioList[Random.Range(0, audioList.Length)];
    }

    public void EnterWaterZone()
    {
        inWater = true;
    }

    public void ExitWaterZone()
    {
        inWater = false;
    }

}
