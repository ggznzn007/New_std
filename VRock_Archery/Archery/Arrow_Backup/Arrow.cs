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

public class Arrow : XRGrabInteractable
{
    public static Arrow Ar;

    public float speed = 42.0f;
    public float flightTime = 0f;
    public PhotonView PV;
    public ArrowManager arrowM;
    public new Rigidbody rigidbody;
    public Collider coll;   
    public TrailRenderer tail;
    public float rotSpeed;
    public Transform shootPoint;
    private ArrowCaster caster;
    private bool launched = false;
    private bool isGrip;
    private RaycastHit hit;   

    protected override void Awake()
    {        
        base.Awake();
        Ar= this;
        caster = GetComponent<ArrowCaster>();
        coll = GetComponent<Collider>();
        PV = GetComponent<PhotonView>();
        arrowM = GetComponent<ArrowManager>();          
        rigidbody = GetComponent<Rigidbody>();     
        isGrip = true;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        arrowM.OnSelectedEntered();        
        DataManager.DM.grabArrow = true;        
        PV.RequestOwnership();
        rotSpeed = 0;
        isGrip = true;
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        DataManager.DM.arrowNum = 0;
        if (args.interactorObject is Notch notch)
        {            
            if (notch.CanRelease)
            {
                LaunchArrow(notch);                
                arrowM.OnSelectedExited();
                if (PV.IsMine)
                {
                    if (!PV.IsMine) return;
                    PV.RPC(nameof(Trailer), RpcTarget.AllBuffered);
                }
            }
        }

    }
    
    private void FixedUpdate()
    {
        if(launched)
        {
            flightTime += Time.fixedDeltaTime; 
        }
    }
    public void LaunchArrow(Notch notch)
    {        
        isGrip= false;
        launched = true;
        flightTime = 0f;
        transform.parent = null;
       // rigidbody.isKinematic = false;
       // rigidbody.useGravity = true;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;        
        ApplyForce(notch.PullMeasurer);
       // StartCoroutine(LaunchRoutine());
        DataManager.DM.grabArrow = false;
    }

    public void ApplyForce(PullMeasurer pullMeasurer)
    {       
       //rigidbody.AddForce(transform.forward * (pullMeasurer.PullAmount * speed), ForceMode.Impulse);
       rigidbody.AddForce(shootPoint.forward * (pullMeasurer.PullAmount * speed), ForceMode.Impulse);
    }

    public IEnumerator LaunchRoutine()
    {
        // Set direction while flying
        while (!caster.CheckForCollision(out hit))
        {
           // SetDirection();
            yield return null;
        }

        // Once the arrow has stopped flying
       // DisablePhysics();
       // ChildArrow(hit);
      //  CheckForHittable(hit);        
    }

    public void SetDirection()
    {
        if (rigidbody.velocity.z > 0.5f)
            transform.forward = rigidbody.velocity;
    }

    public void DisablePhysics()
    {
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
    }

    public void ChildArrow(RaycastHit hit)
    {
        transform.SetParent(hit.transform);
        /*AudioManager.AM.PlaySE("Hit");
        switch(DataManager.DM.arrowNum)
        {
            case 0:
                GetComponent<PhotonView>().RPC("DelayArrow", RpcTarget.AllBuffered);
                break; 
            case 1:
                GetComponent<PhotonView>().RPC("DestroyArrow", RpcTarget.AllBuffered);
                break;
            case 2:
                GetComponent<PhotonView>().RPC("DelayArrow", RpcTarget.AllBuffered);
                break;
        }               */
    }

    public void CheckForHittable(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent(out IArrowHittable hittable))
            hittable.Hit(this);
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        return base.IsSelectableBy(interactor) && !launched;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ignore parent collisions
        if (transform.parent != null && collision.transform == transform.parent)
        {
            return;
        }
        string colNameLower = collision.transform.name.ToLower();

        if (flightTime< 0.7f &&(colNameLower.Contains("bow")))
        {
            Physics.IgnoreCollision(collision.collider, coll, true);
            return;
        }
        if (flightTime < 0.7f && (colNameLower.Contains("player")||colNameLower.Contains("lefthand")|| colNameLower.Contains("righthand")))
        {
            Physics.IgnoreCollision(collision.collider, coll, true);
            return;
        }

        if (PV.IsMine)
        {
            if (!isGrip&&launched&&!rigidbody.isKinematic)
            {
                TrySticky(collision);
              
            }
        }
    }
    public void TrySticky(Collision coll)
    {
        AudioManager.AM.PlaySE("Hit");
        Rigidbody colRid = coll.collider.GetComponent<Rigidbody>();
        transform.parent = null;

        if (coll.gameObject.isStatic)
        {
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.isKinematic = true;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        else if (colRid != null && !colRid.isKinematic)
        {
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = colRid;
            joint.enableCollision = false;
            joint.breakForce = float.MaxValue;
            joint.breakTorque = float.MaxValue;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else if (colRid != null && colRid.isKinematic && coll.transform.localScale == Vector3.one)
        {
            transform.SetParent(coll.transform);
            rigidbody.useGravity = false;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.isKinematic = true;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            rigidbody.WakeUp();
        }
        else
        {
            if (coll.transform.localScale == Vector3.one)
            {
                transform.SetParent(coll.transform);
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
            }
        }

        
        switch (DataManager.DM.arrowNum)
        {
            case 0:
                GetComponent<PhotonView>().RPC("DelayArrow", RpcTarget.AllBuffered);
                break;
            case 1:
                GetComponent<PhotonView>().RPC("DestroyArrow", RpcTarget.AllBuffered);
                break;
            case 2:
                GetComponent<PhotonView>().RPC("DelayArrow", RpcTarget.AllBuffered);
                break;
            case 3:
                GetComponent<PhotonView>().RPC("BombArrow", RpcTarget.AllBuffered);
                break;
        }
    }
    [PunRPC]
    public void Trailer()
    {
        StartCoroutine(TrailerCtrl());
    }

    public IEnumerator TrailerCtrl()
    {
        tail.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        tail.gameObject.SetActive(false);
    }

    

    /*    [SerializeField] private float speed = 2000.0f;

        private new Rigidbody rigidbody;
        private ArrowCaster caster;

        private bool launched = false;

        private RaycastHit hit;

        protected override void Awake()
        {
            base.Awake();
            rigidbody = GetComponent<Rigidbody>();
            caster = GetComponent<ArrowCaster>();
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            if (args.interactorObject is Notch notch)
            {
                if (notch.CanRelease)
                    LaunchArrow(notch);
            }
        }

        private void LaunchArrow(Notch notch)
        {
            launched = true;
            ApplyForce(notch.PullMeasurer);
            StartCoroutine(LaunchRoutine());
        }

        private void ApplyForce(PullMeasurer pullMeasurer)
        {
            rigidbody.AddForce(transform.forward * (pullMeasurer.PullAmount * speed));
        }

        private IEnumerator LaunchRoutine()
        {
            // Set direction while flying
            while (!caster.CheckForCollision(out hit))
            {
                SetDirection();
                yield return null;
            }

            // Once the arrow has stopped flying
            DisablePhysics();
            ChildArrow(hit);
            CheckForHittable(hit);
        }

        private void SetDirection()
        {
            if (rigidbody.velocity.z > 0.5f)
                transform.forward = rigidbody.velocity;
        }

        private void DisablePhysics()
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }

        private void ChildArrow(RaycastHit hit)
        {
            transform.SetParent(hit.transform);
        }

        private void CheckForHittable(RaycastHit hit)
        {
            if (hit.transform.TryGetComponent(out IArrowHittable hittable))
                hittable.Hit(this);
        }

        public override bool IsSelectableBy(IXRSelectInteractor interactor)
        {
            return base.IsSelectableBy(interactor) && !launched;
        }*/
}
