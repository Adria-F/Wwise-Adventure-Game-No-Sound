////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public delegate void QuestGiverEvent(QuestGiver questGiver);
    public class QuestGiver : InteractableNPC
    {
        public static event QuestGiverEvent OnQuestlineForcedChangeStarted;
        public static event QuestGiverEvent OnQuestlineForcedChangeEnded;
        public event QuestGiverEvent OnQuestlineComplete;
        public event QuestEvent OnNewQuest;
        public event QuestEvent OnQuestCompleted;

        public float QuestlineProgressionRTPC;
        //public AK.Wwise.Event QuestlineAdvancedEvent;

        public bool StartQuestLineOnStart = true;
        public List<Quest> Quests;

        private AudioSource audio_source;

        #region private variables
        private int currentQuestIdx = 0;
        private bool initializingNewQuest = false;
        private IEnumerator interactionRoutine;
        #endregion

        private void Start()
        {
            audio_source = GameObject.FindGameObjectWithTag("Menu/UI").GetComponent<AudioSource>();
            
            if (StartQuestLineOnStart)
            {
                InitializeQuest(currentQuestIdx);

            }
        }

        private Coroutine InitializeQuest(int questIdx)
        {

            return StartCoroutine(QuestInit(questIdx));
        }

        private IEnumerator QuestInit(int questIdx)
        {

            Quest currentQuest = Quests[questIdx];
            yield return currentQuest.InitializeQuest();
            currentQuest.OnQuestComplete += AdvanceQuestLine;
            SetDialogue(currentQuest.GetDialogue()); 

            if (OnNewQuest != null)
            {
                OnNewQuest(currentQuest);
            }

            QuestlineProgressionRTPC = GetNormalizedQuestlineProgress() * 100f;
            // HINT: Questline progression RTPC changed, does this affect to any sound?      
            initializingNewQuest = false;
        }

        public Quest GetCurrentQuest()
        {
            return Quests[currentQuestIdx];
        }

        public int GetQuestIndex(Quest quest)
        {
            return Quests.IndexOf(quest);
        }

        public void AdvanceQuestLine(Quest quest)
        {
            initializingNewQuest = true;
            quest.OnQuestComplete -= AdvanceQuestLine;

            if(OnQuestCompleted != null)
            {
                OnQuestCompleted(quest);

            }

            currentQuestIdx++;
            if (currentQuestIdx < Quests.Count)
            {
                // HINT: Questline complete, you may want to play a sound here                                

                audio_source.PlayOneShot(Resources.Load<AudioClip>("Audio/Interface/BAS_Quest_CompleteCollection_01"));
                InitializeQuest(currentQuestIdx);
            }
            else
            {
                // HINT: Questline complete, you may want to play a sound here           

                if (OnQuestlineComplete != null)
                {

                    OnQuestlineComplete(this);
                }
            }
        }

        public void SetQuestLineProgress(int targetQuestIdx)
        {
            if (currentQuestIdx != targetQuestIdx)
            {
                StartCoroutine(ForcedQuestlineAdvance(targetQuestIdx));
                
            }
            else
            {
                Quests[currentQuestIdx].RestartQuest();
                Debug.Log("Resetting quest ");
            }
        }

        private IEnumerator ForcedQuestlineAdvance(int targetQuestIdx)
        {
            if (OnQuestlineForcedChangeStarted != null)
            {
                OnQuestlineForcedChangeStarted(this);
            }
            int sign = (int)Mathf.Sign(targetQuestIdx - currentQuestIdx);

            int i = currentQuestIdx;
            while (i != targetQuestIdx)
            {
                if (sign > 0)   //Moving forwards in the questline
                {
                    yield return Quests[currentQuestIdx].ForceCompleteQuest();
                    i = currentQuestIdx;
                    yield return Quests[i].ForceCompleteQuest();
                }
                else            //Moving backwards in the questline
                {
                    Quests[i].OnQuestComplete -= AdvanceQuestLine;
                    yield return Quests[i].ResetQuest();
                    i--;
                }

                i += 1 * sign;
            }
            currentQuestIdx = targetQuestIdx;
            Debug.Log("Resetting quest " + currentQuestIdx);
            yield return Quests[currentQuestIdx].ResetQuest();
            Debug.Log("(forced) Initializing quest " + currentQuestIdx);
            yield return InitializeQuest(currentQuestIdx);

            //new
            if(sign < 0)
            {
                Debug.Log("Resetting quest " + currentQuestIdx);
                yield return Quests[currentQuestIdx].ResetQuest();
                Debug.Log("(forced) Initializing quest " + currentQuestIdx);
                yield return InitializeQuest(currentQuestIdx);
            }

            if (OnQuestlineForcedChangeEnded != null)
            {
                OnQuestlineForcedChangeEnded(this);
            }
            audio_source.PlayOneShot(Resources.Load<AudioClip>("Audio/Interface/BAS_Quest_CompleteCollection_02"));
        }

        public bool IsInitializingQuest()
        {           
            return initializingNewQuest;
        }

        public float GetNormalizedQuestlineProgress()
        {
            return ((float)currentQuestIdx / (float)(Quests.Count - 1));
        }
    }
}
