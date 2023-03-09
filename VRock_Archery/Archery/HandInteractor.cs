using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandInteractor : XRDirectInteractor
{
    public void HandDetection(XRBaseInteractable interactable)
    {
        if (interactable is Arrow arrow)
        {
            arrow.myColl.enabled = false;
        }
    }

    public void HandDetection2(XRBaseInteractable interactable)
    {
        if (interactable is SnowBall ball)
        {
            ball.myColl.enabled = false;
        }
    }

    public void ForceInteract(SelectEnterEventArgs interactable)
    {
        OnSelectEntered(interactable);
    }

    public void ForceDeinteract(SelectExitEventArgs interactable)
    {
        OnSelectExited(interactable);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);        
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
    }
}
