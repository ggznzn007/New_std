using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(StringInteraction))]

public class SocketInteraction : XRSocketInteractor
{
    /*private XRBaseInteractor handHoldingArrow = null;
    private XRBaseInteractable currentArrow = null;
    private StringInteraction stringInteraction = null;
    private BowInteraction bowInteraction = null;
    private ArrowInteraction currentArrowInteraction = null;

    protected override void Awake()
    {
        base.Awake();
        this.stringInteraction = GetComponent<StringInteraction>();
        this.bowInteraction = GetComponentInParent<BowInteraction>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        stringInteraction.selectExited.AddListener(ReleasaeArrow);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        stringInteraction.selectExited.RemoveListener(ReleasaeArrow);
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        this.handHoldingArrow = args.interactable.selectingInteractor;
        if (args.interactable.tag == "Arrow" && bowInteraction.BowHeld)
        {
            interactionManager.SelectExit(handHoldingArrow, args.interactable);
            interactionManager.SelectEnter(handHoldingArrow, stringInteraction);
        }
    }

   
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        StoreArrow(args.interactable);
    }

    private void StoreArrow(XRBaseInteractable interactable)
    {
        if (interactable.tag == "Arrow")
        {
            this.currentArrow = interactable;
            this.currentArrowInteraction = currentArrow.gameObject.GetComponent<ArrowInteraction>();
        }
    }

    private void ReleasaeArrow(SelectExitEventArgs arg0)
    {
        if (currentArrow && bowInteraction.BowHeld)
        {
            ForceDetach();
            ReleaseArrowFromSocket();
            ClearVariables();
        }
    }

    public override XRBaseInteractable.MovementType? selectedInteractableMovementTypeOverride
    {
        get { return XRBaseInteractable.MovementType.Instantaneous; }
    }

  /// <summary>
  /// 인터랙션 툴킷을 이용한 활과 화살을 만드는 방법을 설명해주셨는데,
  /// 저의 프로젝트는 툴킷 버전이 2.2.0이라서 코드 에러가 생겨요
  /// 호환이 되는 방법이 없을까요?
  /// </summary>
    private void ForceDetach()
    {
        interactionManager.SelectExit(this, currentArrow);
    }

    private void ReleaseArrowFromSocket()
    {
        currentArrowInteraction.ReleaseArrow(stringInteraction.PullAmount);
    }

    private void ClearVariables()
    {
        this.currentArrow = null;
        this.currentArrowInteraction = null;
        this.handHoldingArrow = null;
    }*/
}
