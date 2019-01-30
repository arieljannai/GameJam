using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleDiscScript : MonoBehaviour
{
    private static bool eventSent = false;

    public void TurtleDisc_FadeIn_End()
    {
        Debug.Log("TurtleDisc_FadeIn_End event");

        if (!eventSent)
        {
            Debug.Log("TurtleDisc_FadeIn_End");
            EventManager.Instance.TriggerEvent(EventManager.EVENT__TURTLE_DISC_FADEIN_END, null);
            eventSent = true;
        }
    }
}
