////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QuestSystem;

public class QuestUpdatedVisuals : MonoBehaviour
{
    public float ShowDuration = 6f;

    #region private variables
    [SerializeField]
    private QuestGiver questGiver;

    [SerializeField]
    private Animator questUpdatedAnimator;

    [SerializeField]
    private LocalisedText questTitleText;

    [SerializeField]
    private LocalisedText questDescriptionText; 

    private Quest currentQuest;
    private bool isShowing = false;

    //Cached animator hashes
    private readonly int showTrigger = Animator.StringToHash("Show");
    private readonly int hideTrigger = Animator.StringToHash("Hide");

    private AudioSource audio_source;
    #endregion

    public void OnEnable()
    {
        questGiver.OnNewQuest += UpdateQuestInfo;
        audio_source = GameObject.FindGameObjectWithTag("Menu/UI").GetComponent<AudioSource>();
    }

    public void OnDisable()
    {
        questGiver.OnNewQuest -= UpdateQuestInfo; //TODO: This apparently gives a null reference on exiting the game. Investigate (check script execution order)
    }

    private void UpdateQuestInfo(Quest quest)
    {
        if (currentQuest != null)
        {
            currentQuest.OnDialogueComplete.RemoveListener(ShowAndHideQuestRoll);
        }

        currentQuest = quest;
        currentQuest.OnDialogueComplete.AddListener(ShowAndHideQuestRoll);

        questTitleText.SetKey(currentQuest.TitleKey);
        questDescriptionText.SetKey(currentQuest.DescriptionKey);

        
    }

    public void ShowQuestRoll()
    {
        if (!isShowing)
        {
            questUpdatedAnimator.SetTrigger(showTrigger);            

            isShowing = true;
        }
    }

    public void HideQuestRoll()
    {
        if (isShowing)
        {
            questUpdatedAnimator.SetTrigger(hideTrigger);

            isShowing = false;
        }
    }

    public void ShowAndHideQuestRoll()
    {
        StartCoroutine(ShowAndHide());
    }

    private IEnumerator ShowAndHide()
    {
        audio_source.PlayOneShot(Resources.Load<AudioClip>("Audio/Interface/BAS_QuestRoll_Open"));
        ShowQuestRoll();
        yield return new WaitForSeconds(ShowDuration);
        HideQuestRoll();
        audio_source.PlayOneShot(Resources.Load<AudioClip>("Audio/Interface/BAS_QuestRoll_Close"));
    }
}
