using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreenScript : MonoBehaviour
{
    public void OpenScreen_FadeOut_End()
    {
        EventManager.Instance.TriggerEvent(EventManager.EVENT__TUTORIAL_END, null);
    }
}
