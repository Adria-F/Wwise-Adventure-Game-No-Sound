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
        AudioSource audio_source;
        audio_source = GameObject.FindGameObjectWithTag("Menu/UI").GetComponent<AudioSource>();
        audio_source.clip = Resources.Load<AudioClip>("Audio/Interface/BAS_Button_Open");
        audio_source.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play OOnPointerEnterSound sound
        AudioSource audio_source;
        audio_source = GameObject.FindGameObjectWithTag("Menu/UI").GetComponent<AudioSource>();
        audio_source.clip = Resources.Load<AudioClip>("Audio/Interface/BAS_Button_Enter");
        audio_source.Play();
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
