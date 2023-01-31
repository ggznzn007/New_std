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

public class PullMeasurer : XRBaseInteractable
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform attachPoint;
    private GameObject myArrow;
    public AudioSource audioSource;
    //public PhotonView PV;
   // private XRBaseInteractor pullingInteractor = null;
   // public HandInteractor HandInteractor { get; private set; }
    //public Notch Notch { get; private set; }

    public float PullAmount { get; private set; } = 0.0f;

    public Vector3 PullPosition => Vector3.Lerp(start.position, end.position, PullAmount);

   // public Quaternion PullRotation =>Quaternion.Lerp(start.rotation, end.rotation, PullAmount);
   

    /* protected override void Awake()
     {
         base.Awake();
         HandInteractor = FindObjectOfType<HandInteractor>();
     }*/

    private void Start()
    {
        audioSource= GetComponentInParent<AudioSource>();
       // PV = GetComponent<PhotonView>();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        PullAmount = 0;
        Debug.Log("활시위를 놓았습니다.");
        DataManager.DM.grabArrow = false;
        AudioManager.AM.PlaySE("bShoot");
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log("활시위를 잡았습니다.");
        SpawnArrow();                                                  // 활시위를 잡았을 때 기본화살 생성하는 메서드 호출
        PullSoundInterval(0, 0.7f, 0.4f);
        //PullSoundInterval(0, 1.66f, 0.4f);
        //pullingInteractor = HandInteractor;        

    }

    public void ForceInteract(SelectEnterEventArgs args)
    {
        OnSelectEntered(args);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (isSelected)
        {
            // Update pull values while the measurer is grabbed
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
                    UpdatePull();     
        }
    }

   
    private void UpdatePull()
    {
        // Use the interactor's position to calculate amount
       // Vector3 interactorPosition = firstInteractorSelecting.transform.position;                       
        Vector3 interactorPosition =firstInteractorSelecting.transform.position;
        Debug.Log("활시위를 당기는중...");
        
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

    public void PullSoundInterval(float fromSeconds, float toSeconds, float volume)
    {
        if (audioSource)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.pitch = Time.timeScale;
            audioSource.time = fromSeconds;
            audioSource.volume = volume;
            audioSource.Play();
            audioSource.SetScheduledEndTime(AudioSettings.dspTime + (toSeconds - fromSeconds));
        }
    }

    public void SpawnArrow()
    {
        if(!DataManager.DM.grabArrow)
        {
            Arrow arrow = CreateArrow();
            myArrow = arrow.gameObject;
        }
    }

    private Arrow CreateArrow()  // 기본화살 생성
    {
        // Create arrow, and get arrow component
        myArrow = PN.Instantiate(arrow.name, attachPoint.position, attachPoint.rotation);
        return myArrow.GetComponent<Arrow>();
    }
}
