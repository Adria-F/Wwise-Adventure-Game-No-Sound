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
        audioSource.PlayOneShot(GetRandomAudio(mat), 0.5f* PlayerManager.Instance.player.GetComponent<PlayerMovement>().movementAmount); 

        // HINT: Play footstep sound here
    }

    public AudioClip GetRandomAudio(SoundMaterial.Materials mat)
    {
        AudioClip[] audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().dirtWalk;

        switch (mat)
        {
            case SoundMaterial.Materials.DIRT:
                if (PlayerManager.Instance.player.GetComponent<PlayerMovement>().movementAmount > 0.5f)
                    audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().dirtRun;
                else
                    audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().dirtWalk;
                break;
            case SoundMaterial.Materials.GRASS:
                if (PlayerManager.Instance.player.GetComponent<PlayerMovement>().movementAmount > 0.5f)
                    audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().grassRun;
                else
                    audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().grassWalk;
                break;
            case SoundMaterial.Materials.RUBBLE:
                if (PlayerManager.Instance.player.GetComponent<PlayerMovement>().movementAmount > 0.5f)
                    audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().rubbleRun;
                else
                    audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().rubbleWalk;
                break;
            case SoundMaterial.Materials.SAND:
                if (PlayerManager.Instance.player.GetComponent<PlayerMovement>().movementAmount > 0.5f)
                    audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().sandRun;
                else
                    audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().sandWalk;
                break;
            case SoundMaterial.Materials.STONE:
                if (PlayerManager.Instance.player.GetComponent<PlayerMovement>().movementAmount > 0.5f)
                    audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().stoneRun;
                else
                    audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().stoneWalk;
                break;
            case SoundMaterial.Materials.WATER:
                if (PlayerManager.Instance.player.GetComponent<PlayerMovement>().movementAmount > 0.5f)
                    audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().waterRun;
                else
                    audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().waterWalk;
                break;
            case SoundMaterial.Materials.WOOD:
                if (PlayerManager.Instance.player.GetComponent<PlayerMovement>().movementAmount > 0.5f)
                    audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().woodRun;
                else
                    audioList = GameObject.FindGameObjectWithTag("Player").GetComponent<AdventuressAnimationEventHandler>().woodWalk;
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
