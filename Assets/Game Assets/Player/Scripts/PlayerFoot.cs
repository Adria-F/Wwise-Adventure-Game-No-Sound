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
        if (!inWater)
        {
            materialChecker.CheckMaterial(gameObject); //This also sets the material if a SoundMaterial is found!
        }

        // HINT: Play footstep sound here
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
