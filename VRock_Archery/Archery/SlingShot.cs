using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SlingShot : XRGrabInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs interactor)
    {
        base.OnSelectEntered(interactor);       
    }
    protected override void OnSelectExited(SelectExitEventArgs interactor)
    {
        base.OnSelectExited(interactor);       
    }


}
