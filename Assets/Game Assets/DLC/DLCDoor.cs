////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLCDoor : MonoBehaviour
{
    public void OpenDoor()
    {
        // HINT: This is a good place to play the Open door sound
        PlayerManager.Instance.cameraScript.StartShake(0.02f);
    }

    public void StopDoor()
    {
        // HINT: And this is a good place to play the Close door sound
        PlayerManager.Instance.cameraScript.StopShake();
        PlayerManager.Instance.cameraScript.CamShake(new PlayerCamera.CameraShake(0.1f, 0.6f));

    }

}
