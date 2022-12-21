using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShieldGrabInteractable : XRGrabInteractable
{
    public Transform left_Grab;
    public Transform right_Grab;
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
       
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
    }
}
