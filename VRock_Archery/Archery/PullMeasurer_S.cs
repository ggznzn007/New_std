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


public class PullMeasurer_S : XRBaseInteractable
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] GameObject ball;
    [SerializeField] Transform attachPoint;
    private GameObject myBall;
    public AudioSource audioSource;

    public float PullAmount { get; private set; } = 0.0f;

    public Vector3 PullPosition => Vector3.Lerp(start.position, end.position, PullAmount);

    private void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();        
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        PullAmount = 0;       
        DataManager.DM.grabString = false;
        AudioManager.AM.PlaySE("SnowThrow");
        Debug.Log("새총 시위를 놓았습니다.");
        Debug.Log("눈덩이 던지는 소리재생");
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log("새총 시위를 잡았습니다.");
        DataManager.DM.grabString = true;
        SpawnBall();                                                  // 활시위를 잡았을 때 기본화살 생성하는 메서드 호출
        PullSoundInterval(0, 0.7f, 0.4f);

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
        Vector3 interactorPosition = firstInteractorSelecting.transform.position;
        Debug.Log("새총 시위를 당기는중...");

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

    public void SpawnBall()
    {
        if (!DataManager.DM.grabBall)
        {
            SnowBall ball = CreateBall();
            myBall = ball.gameObject;
        }
    }

    private SnowBall CreateBall() 
    {
        // Create arrow, and get arrow component
        myBall = PN.Instantiate(ball.name, attachPoint.position, attachPoint.rotation);
        return myBall.GetComponent<SnowBall>();
    }
}
