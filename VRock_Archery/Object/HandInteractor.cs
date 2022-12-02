using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandInteractor : XRDirectInteractor
{
    public void HandDetection(XRBaseInteractable interactable)
    {
       if(interactable is Arrow arrow)
        {
            arrow.coll.enabled = false;
        }
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
    }

    public void ForceInteract(SelectEnterEventArgs interactable)
    {
        OnSelectEntered(interactable);
    }

    public void ForceDeinteract(SelectExitEventArgs interactable)
    {
        OnSelectExited(interactable);
    }
}
