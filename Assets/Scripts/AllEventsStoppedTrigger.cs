////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

/*﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEventsStoppedTrigger : AkTriggerBase
{
    #region private variables
    private AkEvent[] akEventsInScene;
    private int eventsStopped = 0;
    private List<int> callbackIndeces;
    #endregion

    private void OnEnable()
    {
        eventsStopped = 0;
        callbackIndeces = new List<int>();

        //Find all AkEvents in children
        akEventsInScene = GetComponentsInChildren<AkEvent>();
       
        //Subscribe to their EndOfEvent callback
        for(int i=0; i<akEventsInScene.Length; i++)
        {
            var currentEvent = akEventsInScene[i];
            AddCallback(currentEvent);
        }
    }

    private void OnDisable()
    {
        if (akEventsInScene != null)
        {
            //print(akEventsInScene.Length);
            for (int i = 0; i < akEventsInScene.Length; i++)
            {
                var c = akEventsInScene[i].m_callbackData;
                int idx = callbackIndeces[i];
                c.callbackFlags.RemoveAt(idx);
                c.callbackGameObj.RemoveAt(idx);
                c.callbackFunc.RemoveAt(idx);
            }
        }
    }

    private void AddCallback(AkEvent evt){
        var c = evt.m_callbackData;

        if (c == null)
        {
            evt.m_callbackData = ScriptableObject.CreateInstance<AkEventCallbackData>();
            c = evt.m_callbackData;
        }

        int endOfEventBit = 0x0001; //End of event flag
        int newCallbackIdx = c.callbackFlags.Count;

        c.callbackFlags.Add(endOfEventBit);
        c.callbackGameObj.Add(gameObject);
        c.callbackFunc.Add("EventStopped");

        callbackIndeces.Add(newCallbackIdx);
    }

    public void EventStopped()
    {
        eventsStopped++;
        print("Eventsstopped: "+eventsStopped);

        if(eventsStopped == akEventsInScene.Length)
        {
            TriggerDelegate();
        }

    }

    private void TriggerDelegate()
    {
        if(triggerDelegate != null)
        {
            triggerDelegate(gameObject);
        }
    }
}*/
