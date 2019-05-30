////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterZoneTrigger : MonoBehaviour
{
    PlayerFoot[] feet;

    void Start()
    {
        feet = PlayerManager.Instance.player.GetComponentsInChildren<PlayerFoot>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            for (int i = 0; i < feet.Length; i++)
            {
                feet[i].EnterWaterZone();
                // HINT: We know the Adenturess has entered water, you might want to place some code here to modify the footsteps sound effect accordingly
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            for (int i = 0; i < feet.Length; i++)
            {
                feet[i].ExitWaterZone();
                // HINT: We know the Adenturess has left water, you might want to place some code here to modify the footsteps sound effect accordingly
            }
        }
    }
}
