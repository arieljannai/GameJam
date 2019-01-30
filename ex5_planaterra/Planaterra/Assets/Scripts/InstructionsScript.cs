using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsScript : MonoBehaviour
{
    public void Instructions_FadeIn_End()
    {
        EventManager.Instance.TriggerEvent(EventManager.EVENT__INSTRUCTIONS_FADEIN_END, null);
    }
}
