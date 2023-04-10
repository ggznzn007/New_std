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

public class PullMeasurer : XRBaseInteractable
{
    [SerializeField] private Transform start; // 활시위 시작점
    [SerializeField] private Transform end;   // 활시위 끝점
    [SerializeField] GameObject arrow;        // 화살
    [SerializeField] Transform attachPoint;   // 화살이 생성되어 붙는 위치
    private GameObject myArrow;               // 현재 화살
    public AudioSource audioSource;           // 활 당길때 오디오
   
    public float PullAmount { get; private set; } = 0.0f; // 활시위 당기는 양

    public Vector3 PullPosition => Vector3.Lerp(start.position, end.position, PullAmount); // 활시위 당기는 양에따라 위치 변화  

    private void Start()
    {
        audioSource= GetComponentInParent<AudioSource>();      
    }

    protected override void OnSelectExited(SelectExitEventArgs args) // 활시위를 놓았을 때 메서드
    {
        base.OnSelectExited(args);
        PullAmount = 0;                    // 당기는 양 초기화
        DataManager.DM.grabString = false; // 활시위를 놓았다는 데이터 저장
        AudioManager.AM.PlaySE("bShoot");  // 화살 발사 오디오 재생
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args) // 활시위를 잡았을 때 메서드
    {
        base.OnSelectEntered(args);
        SpawnArrow();                       // 활시위를 잡았을 때 기본화살 생성하는 메서드 호출
        PullSoundInterval(0, 0.7f, 0.4f);   // 활시위를 잡아 당길때 인터벌
        DataManager.DM.grabString = true;   // 활시위를 잡았다는 데이터 저장        
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

   
    private void UpdatePull() // 활시위 당기는 중이라는 메서드
    {
        // Use the interactor's position to calculate amount
       // Vector3 interactorPosition = firstInteractorSelecting.transform.position;                       
        Vector3 interactorPosition =firstInteractorSelecting.transform.position;
        
        // Figure out the new pull value, and it's position in space
        PullAmount = CalculatePull(interactorPosition);       
    }

    private float CalculatePull(Vector3 pullPosition)            // 활시위 당기는 위치 및 회전 계산 메서드
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
