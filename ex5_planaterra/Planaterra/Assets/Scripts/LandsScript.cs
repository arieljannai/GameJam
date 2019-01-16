using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandsScript : MonoBehaviour
{
    public void Land_On()
    {
        EventManager.Instance.TriggerEvent(EventManager.EVENT__LAND_ON, null);
    }
}
