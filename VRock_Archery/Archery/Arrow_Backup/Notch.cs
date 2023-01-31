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
using Photon.Pun.Demo.Procedural;
using UnityEngine.InputSystem.HID;

//[RequireComponent(typeof(PullMeasurer))]
public class Notch : XRSocketInteractor
{
    [SerializeField, Range(0, 1)] private float releaseThreshold = 0f;
    private Arrow curArrow;
    //private Arrow_Skilled curArrow2;
    public Collider coll;
    public Bow Bow { get; private set; }
    public PullMeasurer PullMeasurer { get; private set; }

    public HandInteractor HandInteractor { get; private set; }

    //public Arrow Arrow { get; private set; }

    public bool CanRelease => PullMeasurer.PullAmount > releaseThreshold;

    protected override void Awake()
    {
        base.Awake();
        Bow = GetComponentInParent<Bow>();
        PullMeasurer = GetComponentInChildren<PullMeasurer>();
        HandInteractor = FindObjectOfType<HandInteractor>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PullMeasurer.selectExited.AddListener(ReleaseArrow);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PullMeasurer.selectExited.RemoveListener(ReleaseArrow);
    }

    public void ReleaseArrow(SelectExitEventArgs args)
    {
        if (hasSelection)
            interactionManager.SelectExit(this, firstInteractableSelected);
        
    }

    public override void ProcessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractor(updatePhase);

        if (Bow.isSelected)
            UpdateAttach();
    }

    public void UpdateAttach()
    {
        // Move attach when bow is pulled, this updates the renderer as well
        attachTransform.position = PullMeasurer.PullPosition;        
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log("활시위에 화살이 붙었습니다.");
        AttachArrow(curArrow);        
        coll.enabled = false;        
        //DataManager.DM.grabArrow = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        Debug.Log("화살이 발사되었습니다.");        
        coll.enabled = true;
       // DataManager.DM.grabArrow = false;
    }

    private void AttachArrow(Arrow interactable)
    {
        if (interactable is Arrow Arrow)
        {
            curArrow = Arrow;            
        }        
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        // We check for the hover here too, since it factors in the recycle time of the socket
        // We also check that notch is ready, which is set once the bow is picked up

        return QuickSelect(interactable) && CanHover(interactable) && interactable is Arrow && Bow.isSelected;
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
