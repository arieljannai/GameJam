using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : Singleton<EventManager>
{
    protected EventManager()
    {
        Init();
    } // guarantee this will be always a singleton only - can't use the constructor!

    public const string EVENT__TUTORIAL_END = "event_tutorial_end";
    public const string EVENT__CONSTELLATION_END = "event_constellation_end";
    public const string EVENT__LAND_ON = "event_land_on";
    public const string EVENT__FINISHED_STRECHING = "event_finished_streching";
    public const string EVENT__STARTED_DRAWING = "event_started_drawing";
    public const string EVENT__FINISHED_DRAWING = "event_finished_drawing";
    public const string EVENT__FINISHED_SHOWING_LINE = "event_finished_showing_line";
    public const string EVENT__FAILED_SHAPE_TOO_MUCH = "event_failed_shape_too_much";
    public const string EVENT__FAILED_SHAPE_TOO_LITTLE = "event_failed_shape_too_little";


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