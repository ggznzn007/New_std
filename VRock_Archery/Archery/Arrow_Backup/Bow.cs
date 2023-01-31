using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bow : XRGrabInteractable
{   
  /*  public Transform notch;
    private PullMeasurer pullMeasurer;
    private LineRenderer lineRenderer;
   
    protected override void Awake()
    {
        base.Awake();
        pullMeasurer = GetComponentInChildren<PullMeasurer>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            if (isSelected)
                UpdateBow(pullMeasurer.PullAmount);
    }
    private void UpdateBow(float value)
    {
        Vector3 linePosition = Vector3.forward * Mathf.Lerp(-0.25f, -0.5f, value);
        notch.localPosition = linePosition;
        lineRenderer.SetPosition(1, linePosition);
    }*/

    protected override void OnSelectEntered(SelectEnterEventArgs interactor)
    {
        base.OnSelectEntered(interactor);
      //  quivers[0].gameObject.SetActive(true);
      //  quivers[1].gameObject.SetActive(true);
    }
    protected override void OnSelectExited(SelectExitEventArgs interactor)
    {
        base.OnSelectExited(interactor);        
      //  quivers[0].gameObject.SetActive(false);
      //  quivers[1].gameObject.SetActive(false);
    }

   
}
