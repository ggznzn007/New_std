using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bow : XRGrabInteractable
{
    public Transform notch;
    private PullMeasurer pullMeasurer;
    private LineRenderer lineRenderer;

    [Header("Sound")]
    public AudioClip grabClip;

    [Header("Quiver")]
    public Transform quiver;
    public Vector2 quiverOffset;

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
    }

    protected override void OnSelectEntered(SelectEnterEventArgs interactor)
    {
        base.OnSelectEntered(interactor);
    }

    protected override void OnSelectExited(SelectExitEventArgs interactor)
    {
        base.OnSelectExited(interactor);
        Destroy(gameObject,1f);
    }

    //public void BowHaptic(XRBaseInteractor interactor)
    //{
    //    if (interactor.TryGetComponent(out XRController controller))
    //        HapticManager.Impulse(1, .05f, controller.inputDevice);
    //}

    public void OffsetQuiver(XRBaseInteractor interactor)
    {
        if (interactor.TryGetComponent(out XRController controller))
        {
            bool right = (controller.inputDevice.role == UnityEngine.XR.InputDeviceRole.RightHanded);
            quiver.localPosition = new Vector3(quiverOffset.x * (right ? -1 : 1), quiverOffset.y, quiver.localPosition.z);
        }
    }
}
