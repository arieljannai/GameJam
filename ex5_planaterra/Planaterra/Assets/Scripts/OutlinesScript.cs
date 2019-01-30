using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionsMethods;

public class OutlinesScript : MonoBehaviour
{
    public void Outlines_ShapeFadeOut(int nShape)
    {
        EventManager.Instance.TriggerEvent(EventManager.EVENT__OUTLINES_SHAPE_FADEOUT, nShape);
    }

    public void TurnOff()
    {
        GameController.Instance.outlines.Animator().SetState(Animators.OUTLINES__OFF);
    }
}
