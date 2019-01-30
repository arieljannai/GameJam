using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreenScript : MonoBehaviour
{
    public void OpenScreen_FadeOut_End()
    {
        Debug.Log("OpenScreen_FadeOut_End");
        EventManager.Instance.TriggerEvent(EventManager.EVENT__TUTORIAL_END, null);
    }

    public void OpenScreen_FadeIn_End()
    {
        Debug.Log("OpenScreen_FadeIn_End");
        EventManager.Instance.TriggerEvent(EventManager.EVENT__TUTORIAL_TURTLE_VISIBLE, null);
    }
}
