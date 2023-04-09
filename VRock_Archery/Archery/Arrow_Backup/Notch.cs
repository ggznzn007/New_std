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


//[RequireComponent(typeof(PullMeasurer))]
public class Notch : XRSocketInteractor
{
    [SerializeField, Range(0, 1)] private float releaseThreshold = 0f;                                // 활시위에 릴리즈 시간
    private Arrow curArrow;                                                                           // 현재 화살
    public Collider notchColl;                                                                        // 활시위에 붙기위한 콜라이더
    public Collider[] colls;                                                                            
    public Bow Bow { get; private set; }                                                              // 활
    public PullMeasurer PullMeasurer { get; private set; }                                            // 활시위
    public HandInteractor HandInteractor { get; private set; }                                        // 손 인터렉션
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

    protected override void OnSelectEntered(SelectEnterEventArgs args)            // 활시위에 화살이 붙었을 때
    {
        base.OnSelectEntered(args);               
        AttachArrow(curArrow);                                                    // 활시위에 화살이 붙는 메서드 호출               
        notchColl.enabled = false;                                                // 활시위 콜라이더 off
        DataManager.DM.grabArrow = true;                                          // 활시위에 화살이 붙어있다는 데이터 저장
    }

    protected override void OnSelectExited(SelectExitEventArgs args)              // 활시위에서 화살이 발사되었을 때
    {
        base.OnSelectExited(args);       
        notchColl.enabled = true;                                                 // 활시위 콜라이더 on
        DataManager.DM.grabArrow = false;                                         // 활시위에 화살이 떨어졌다는 데이터 저장
    }

    private void AttachArrow(Arrow interactable)                                  // 활시위에 화살이 붙는 메서드
    {
        if (interactable is Arrow Arrow)
        {
            curArrow = Arrow;           
        }        
    }

    public override bool CanSelect(IXRSelectInteractable interactable)            // 화살을 활시위 가까이 가져가면 자동으로 붙게 만드는 메서드
    {
        // We check for the hover here too, since it factors in the recycle time of the socket
        // We also check that notch is ready, which is set once the bow is picked up

        return QuickSelect(interactable) && CanHover(interactable) && interactable is Arrow && Bow.isSelected;
    }

    private bool QuickSelect(IXRSelectInteractable interactable)                 // 화살을 활시위 가까이 가져가면 자동으로 붙게 만드는 메서드
    {
        // This lets the Notch automatically grab the arrow
        
        return !hasSelection || IsSelecting(interactable);
    }

    private bool CanHover(IXRSelectInteractable interactable)                    // 화살을 활시위 가까이 가져가면 자동으로 붙게 만드는 메서드
    {
        if (interactable is IXRHoverInteractable hoverInteractable)
            return CanHover(hoverInteractable) && QuickSelect(interactable);

        return false;
    }

}
