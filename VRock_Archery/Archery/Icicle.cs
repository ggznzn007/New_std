using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Icicle : SnowBall
{
    public string iceImpact;
    protected override void Awake()
    {
        base.Awake();
        PV = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody>();
        isGrip = true;
        //myColl = GetComponent<Collider>();
        //damageColl = GetComponent<Collider>();   
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        DataManager.DM.grabBall = true;
        //PV.RequestOwnership();        
        isGrip = true;
        rigidbody.isKinematic = false;
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        // PV.RequestOwnership();
        rigidbody.isKinematic = true;
        DataManager.DM.grabBall = false;
        if (args.interactorObject is Notch_S notchs)
        {
            if (notchs.CanRelease)
            {
                DataManager.DM.arrowNum = 0;
                LaunchBall(notchs);

                /*if (PV.IsMine)
                {
                    if (!PV.IsMine) return;
                    //PV.RPC(nameof(LaunchArrow), RpcTarget.AllBuffered,notch);
                    PV.RPC(nameof(ActiveColl), RpcTarget.AllBuffered);
                }*/
            }
        }

    }

    private void FixedUpdate()
    {
        if (!isGrip && rigidbody != null && rigidbody.velocity != Vector3.zero && launched && zVel > 0.02)
        {
            rigidbody.rotation = Quaternion.LookRotation(rigidbody.velocity);
        }

        zVel = transform.InverseTransformDirection(rigidbody.velocity).z;

        if (launched)
        {
            flightTime += Time.fixedDeltaTime;
        }
    }
    public new void LaunchBall(Notch_S notchs)
    {
        isGrip = false;
        launched = true;
        flightTime = 0f;
        transform.parent = null;
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        // rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        //rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        //rigidbody.constraints = RigidbodyConstraints.None;        
        ApplyForce(notchs.PullMeasurer_S);
        //StartCoroutine(ReEnableCollider());
        // StartCoroutine(LaunchRoutine());
        DataManager.DM.grabBall = false;
    }

    public new void ApplyForce(PullMeasurer_S pullMeasurers)
    {
        //rigidbody.AddForce(transform.forward * (pullMeasurer.PullAmount * speed), ForceMode.Impulse);
        rigidbody.AddForce(shootPoint.forward * (pullMeasurers.PullAmount * speed), ForceMode.VelocityChange);
        if (rigidbody && MinForceHit != 0)
        {
            float zVel = System.Math.Abs(transform.InverseTransformDirection(rigidbody.velocity).z);

            // Minimum Force not achieved
            if (zVel < MinForceHit)
            {
                return;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (flightTime < 0.1f)//&& (collision.collider.CompareTag("Body")|| collision.collider.CompareTag("head")))
        {
            Physics.IgnoreCollision(collision.collider, bodyColl, true);
            Physics.IgnoreCollision(collision.collider, headColl, true);
            // return;
        }*/
        // Ignore parent collisions
        if (transform.parent != null && collision.transform == transform.parent)
        {
            return;
        }

        if (isGrip) return;

        /*   if(collision.collider.CompareTag("LeftHand")|| collision.collider.CompareTag("RightHand") 
               || collision.collider.CompareTag("Player") || collision.collider.CompareTag("Body")
               || collision.collider.CompareTag("head"))
           {
               Physics.IgnoreCollision(collision.collider, myColl, true);
               return;
           }*/

        string colNameLower = collision.transform.tag.ToLower();

        //if (flightTime < 0.5f && colNameLower.Contains("slingshot"))//|| colNameLower.Contains("arrow")))
        if (flightTime < 0.1f && (colNameLower.Contains("head") || colNameLower.Contains("body")))
        {
            Physics.IgnoreCollision(collision.collider, myColl, true);
            Physics.IgnoreCollision(collision.collider, damageColl, true);
            return;
        }

        //if (flightTime < 0.01f && (colNameLower.Contains("player") || colNameLower.Contains("lefthand") || colNameLower.Contains("righthand")))
        if (colNameLower.Contains("lefthand") || colNameLower.Contains("righthand"))
        {
            Physics.IgnoreCollision(collision.collider, myColl, true);
            Physics.IgnoreCollision(collision.collider, damageColl, true);
            return;
        }

        /*if (PV.IsMine)
        {
            if (!isGrip && launched)
            {
                TrySticky(collision);
            }
        }*/

        if (collision.collider.CompareTag("Head"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    AudioManager.AM.PlaySE(headShot);
                    if (AvartarController.ATC.isAlive && DataManager.DM.inGame)
                    {
                        if (!AvartarController.ATC.isDamaged)
                        {
                            AvartarController.ATC.HeadShotDamage();
                        }
                    }
                    ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ��Ÿ�� �̷�� ȸ������ ����                           
                    var effect = Instantiate(wording_Cr, contact.point, rot);// �浹 ������ ����Ʈ ����        
                    transform.position = contact.point;
                    Destroy(effect, delTime);
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered); // ��ų ȭ��


                }

            }
        }

        if (collision.collider.CompareTag("Body"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    AudioManager.AM.PlaySE(hitPlayer);
                    if (AvartarController.ATC.isAlive && DataManager.DM.inGame)
                    {
                        if (!AvartarController.ATC.isDamaged)
                        {
                            AvartarController.ATC.NormalDamage();
                        }
                    }
                    ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ��Ÿ�� �̷�� ȸ������ ����                           
                    var effect = Instantiate(wording_Hit, contact.point, rot);// �浹 ������ ����Ʈ ����        
                    transform.position = contact.point;
                    Destroy(effect, delTime);
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered); // ��ų ȭ��

                }
            }
        }

        if (collision.collider.CompareTag("FloorBox") || collision.collider.CompareTag("Cube") || collision.collider.CompareTag("Obtacle"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    AudioManager.AM.PlaySE(iceImpact);
                    ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ��Ÿ�� �̷�� ȸ������ ����                           
                    var effect = Instantiate(ballEX, contact.point, rot);
                    Destroy(effect, delTime);
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered);  // �⺻ ȭ��

                }
            }
        }

        if (collision.collider.CompareTag("Shield") || collision.collider.CompareTag("Bow"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    PV.RPC(nameof(ImpactS), RpcTarget.AllBuffered);
                    ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ��Ÿ�� �̷�� ȸ������ ����                           
                    var effect = Instantiate(ballEX, contact.point, rot);// �浹 ������ ����Ʈ ����        
                    transform.position = contact.point;
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered);  // �⺻ ȭ��
                    Destroy(effect, delTime);

                }

            }
        }
        if (collision.collider.CompareTag("Snowblock"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    AudioManager.AM.PlaySE(iceImpact);
                    ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ��Ÿ�� �̷�� ȸ������ ����                           
                    var effect = Instantiate(ballEX, contact.point, rot);// �浹 ������ ����Ʈ ����        
                    transform.position = contact.point;
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered);
                    Destroy(effect, delTime);
                }

            }
        }
    }

}
