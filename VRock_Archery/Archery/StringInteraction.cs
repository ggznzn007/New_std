using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StringInteraction : XRBaseInteractable
{
   /* public Transform stringStartPoint;
    public Transform stringEndPoint;
    public GameObject arrowPrefab;

    private XRBaseInteractor stringInteractor = null; 
    private Vector3 pullPosition;
    private Vector3 pullDirection;
    private Vector3 targetDirection;

    public float PullAmount { get; private set; } = 0.0f;
    public Vector3 StringStartPoint { get => stringStartPoint.localPosition;}
    public Vector3 StringEndPoint { get => stringEndPoint.localPosition; }

    protected override void Awake()
    {
        base.Awake();
    }

   
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        this.stringInteractor = args.interactor;
       
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        this.stringInteractor = null;
        this.PullAmount = 0f;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        
        if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic && isSelected)
        {
            this.pullPosition = this.stringInteractor.transform.position;
            this.PullAmount = CalculatePull(this.pullPosition);
						//Debug.Log("<<<<< Pull amount is "+ PullAmount+" >>>>>");
        }           
    }

    private float CalculatePull(Vector3 pullPosition)
    {
        this.pullDirection = pullPosition - stringStartPoint.position;
        this.targetDirection = stringEndPoint.position - stringStartPoint.position;
        float maxLength = targetDirection.magnitude;
        
        targetDirection.Normalize();

        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
        return Mathf.Clamp(pullValue, 0, 1);
    }

    private void CreateAndSelectArrow(SelectEnterEventArgs args)
    {
        // Create arrow, force into interacting hand
        Arrow arrow = CreateArrow(args.interactorObject.transform);
        interactionManager.SelectEnter(args.interactorObject, arrow);
    }

    private Arrow CreateArrow(Transform orientation)
    {
        // Create arrow, and get arrow component
        GameObject arrowObject = PN.Instantiate(arrowPrefab.name, orientation.position, orientation.rotation);
        return arrowObject.GetComponent<Arrow>();
    }*/
}
