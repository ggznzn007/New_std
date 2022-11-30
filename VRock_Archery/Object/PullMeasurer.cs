using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PullMeasurer : XRBaseInteractable
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private XRSocketInteractor notch;
    private GameObject myArrow;

    public float PullAmount { get; private set; } = 0.0f;

    public Vector3 PullPosition => Vector3.Lerp(start.position, end.position, PullAmount);

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        PullAmount = 0;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (isSelected)
        {
            // Update pull values while the measurer is grabbed
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed)
                UpdatePull();
        }
    }

    private void UpdatePull()
    {
        // Use the interactor's position to calculate amount
        Vector3 interactorPosition = firstInteractorSelecting.transform.position;

        // Figure out the new pull value, and it's position in space
        PullAmount = CalculatePull(interactorPosition);
    }

    private float CalculatePull(Vector3 pullPosition)
    {
        // Direction, and length
        Vector3 pullDirection = pullPosition - start.position;
        Vector3 targetDirection = end.position - start.position;

        // Figure out out the pull direction
        float maxLength = targetDirection.magnitude;
        targetDirection.Normalize();

        // What's the actual distance?
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
        return Mathf.Clamp(pullValue, 0.0f, 1.0f);
    }

    private void OnDrawGizmos()
    {
        // Draw line from start to end point
        if (start && end)
            Gizmos.DrawLine(start.position, end.position);
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        CreateAndSelectArrow(args);
    }
    private void CreateAndSelectArrow(SelectEnterEventArgs args)
    {
        // Create arrow, force into interacting hand
        Arrow arrow = CreateArrow(args.interactorObject.transform);
        interactionManager.SelectEnter(args.interactorObject, arrow);
        notch.socketActive = true;
    }

    private Arrow CreateArrow(Transform orientation)
    {
        // Create arrow, and get arrow component
        myArrow = PN.Instantiate(arrowPrefab.name, orientation.position, orientation.rotation);
        return myArrow.GetComponent<Arrow>();
    }
}
