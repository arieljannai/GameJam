﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : Singleton<EventManager>
{
    protected EventManager()
    {
        Init();
    } // guarantee this will be always a singleton only - can't use the constructor!

    public const string EVENT__HIT = "event_hit";


    public class GeneralEvent : UnityEvent<object> { } //empty class; just needs to exist

    private Dictionary<string, GeneralEvent> eventDictionary;


    private void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, GeneralEvent>();
        }
    }

    public void StartListening(string eventName, UnityAction<object> listener)
    {
        GeneralEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new GeneralEvent();
            thisEvent.AddListener(listener);
            eventDictionary.Add(eventName, thisEvent);
        }
    }

    public void StopListening(string eventName, UnityAction<object> listener)
    {
        GeneralEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public void TriggerEvent(string eventName, object obj)
    {
        GeneralEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(obj);
        }
    }
}