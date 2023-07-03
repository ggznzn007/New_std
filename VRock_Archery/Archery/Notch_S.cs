using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;

public class Notch_S : XRSocketInteractor
{
    [SerializeField, Range(0, 1)] private float releaseThreshold = 0f;
    private SnowBall curBall;
    public Collider notchColl;

    public SlingShot Sling { get; private set; }

    public PullMeasurer_S PullMeasurer_S { get; private set; }

    public HandInteractor HandInteractor { get; private set; }

    public bool CanRelease => PullMeasurer_S.PullAmount > releaseThreshold;

    protected override void Awake()
    {
        base.Awake();
        Sling = GetComponentInParent<SlingShot>();
        PullMeasurer_S = GetComponentInChildren<PullMeasurer_S>();
        HandInteractor = FindObjectOfType<HandInteractor>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PullMeasurer_S.selectExited.AddListener(ReleaseBall);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PullMeasurer_S.selectExited.RemoveListener(ReleaseBall);
    }

    public void ReleaseBall(SelectExitEventArgs args)
    {
        if (hasSelection)
            interactionManager.SelectExit(this, firstInteractableSelected);
    }

    public override void ProcessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractor(updatePhase);

        if (Sling.isSelected)
            UpdateAttach();
    }

    public void UpdateAttach()
    {
        // Move attach when bow is pulled, this updates the renderer as well
        attachTransform.position  = PullMeasurer_S.PullPosition;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
       // Debug.Log("활시위에 화살이 붙었습니다.");
        AttachBall(curBall);
        notchColl.enabled = false;
        DataManager.DM.grabBall = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
       // Debug.Log("화살이 발사되었습니다.");
        notchColl.enabled = true;
        DataManager.DM.grabBall = false;
    }

    private void AttachBall(SnowBall interactable)
    {
        if (interactable is SnowBall Ball)
        {
            curBall = Ball;            
        }
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        // We check for the hover here too, since it factors in the recycle time of the socket
        // We also check that notch is ready, which is set once the bow is picked up
        return QuickSelect(interactable) && CanHover(interactable) && interactable is SnowBall && Sling.isSelected;
    }

    private bool QuickSelect(IXRSelectInteractable interactable)
    {
        // This lets the Notch automatically grab the arrow
        return !hasSelection || IsSelecting(interactable);
    }

    private bool CanHover(IXRSelectInteractable interactable)
    {
        if (interactable is IXRHoverInteractable hoverInteractable)
            return CanHover(hoverInteractable) && QuickSelect(interactable);

        return false;
    }
}
