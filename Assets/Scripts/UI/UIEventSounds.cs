////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventSounds : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        // Play OnPointerDownSound sound
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play OOnPointerEnterSound sound
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Play OOnPointerExitSound sound
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Play OOnPointerUpSound sound
    }
}
