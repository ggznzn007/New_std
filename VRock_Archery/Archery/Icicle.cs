using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Icicle : SnowBall
{
    public string iceImpact;
    public MeshRenderer mesh;
    public Collider tagColl;
    public Collider iceDamColl;
    protected override void Awake()
    {
        base.Awake();
        PV = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody>();
        isGrip = true;
        rigidbody.useGravity = false;
        
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        DataManager.DM.grabBall = true;
        isGrip = true;       
        PV.RPC(nameof(DelTagged), RpcTarget.AllBuffered);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        isGrip = false;
        rigidbody.useGravity = true;
        DataManager.DM.grabBall = false;
        PV.RPC(nameof(RotBefore), RpcTarget.AllBuffered);        

        if (args.interactorObject is Notch_S notchs)
        {
            PV.RPC(nameof(RotAfter), RpcTarget.AllBuffered);            
            
            if (notchs.CanRelease)
            {
                LaunchIcicle(notchs);  
            }
        }
        else
        {
            flightTime = 1;
        }
    }

   

    [PunRPC]
    public void RotBefore()
    {
        mesh.gameObject.transform.rotation = shootPoint.rotation;
        iceDamColl.gameObject.SetActive(true);
    }

    [PunRPC]
    public void RotAfter()
    {
        mesh.gameObject.transform.rotation = transform.rotation;
        iceDamColl.gameObject.SetActive(false);
    }

    [PunRPC]
    public void ActiveEX()
    {
        StartCoroutine(ActiveSkill());
    }

    IEnumerator ActiveSkill()
    {
        yield return new WaitForSeconds(0.01f);
        mesh.gameObject.SetActive(false);
        effect.SetActive(true);
        yield return new WaitForSeconds(0.04f);
        
        tagColl.gameObject.SetActive(true) ;        
        yield return StartCoroutine(DelayTime());
    }
    public IEnumerator DelayTime()
    {
        yield return new WaitForSeconds(3.5f);
        Destroy(PV.gameObject);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Body"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    StartCoroutine(DelayHit(coll));
                }
            }
        }
    }

    public IEnumerator DelayHit(Collider coll)
    {
        AudioManager.AM.PlaySE(hitPlayer);
        var skillEX = Instantiate(wording_Hit, coll.transform.position + new Vector3(0, 0.5f, 0), coll.transform.rotation);// �浹 ������ ����Ʈ ����
        Destroy(skillEX, 0.5f);
        yield return new WaitForSeconds(0.02f);
        tagColl.enabled = false;
    }
    private void FixedUpdate()
    {
        if (!isGrip && rigidbody != null && launched)
        {
            rigidbody.rotation = Quaternion.LookRotation(rigidbody.velocity);
        }

        //zVel = transform.InverseTransformDirection(rigidbody.velocity).z;

        if (launched)
        {
            flightTime += Time.fixedDeltaTime;
        }
    }
    public  void LaunchIcicle(Notch_S notchs)
    {
        if (PV.IsMine)
        {
            if (!PV.IsMine) return;
            PV.RPC(nameof(RotAfter), RpcTarget.AllBuffered);
            PV.RPC(nameof(ActiveEX), RpcTarget.AllBuffered);
        }
        isGrip = false;
        launched = true;
        flightTime = 0f;        
        transform.parent = null;        
        rigidbody.useGravity = false;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;             
        ApplyForce(notchs.PullMeasurer_S);        
        DataManager.DM.grabBall = false;
    }

    public new void ApplyForce(PullMeasurer_S pullMeasurers)
    {
        //rigidbody.AddForce(transform.forward * (pullMeasurer.PullAmount * speed), ForceMode.Impulse);
        rigidbody.AddForce(shootPoint.forward * (pullMeasurers.PullAmount * speed), ForceMode.VelocityChange);
        /* if (rigidbody && MinForceHit != 0)
         {
             float zVel = System.Math.Abs(transform.InverseTransformDirection(rigidbody.velocity).z);

             // Minimum Force not achieved
             if (zVel < MinForceHit)
             {
                 return;
             }
         }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("FloorBox") || collision.collider.CompareTag("Cube") || collision.collider.CompareTag("Snowblock"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if(!isGrip)
                {
                    AudioManager.AM.PlaySE(iceImpact);
                    //TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ���Ͱ� �̷�� ȸ������ ����                            
                    var effect = Instantiate(ballEX, contact.point, rot);// �浹 ������ ����Ʈ ����        
                    transform.position = contact.point;
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered);  // �⺻ ȭ��
                    // ��帧 �浹EX �ı��޼���� ��帧 �浹EX�� �پ�����
                }
                else
                {
                    return;
                }
            }
        }
        // Ignore parent collisions
        if (transform.parent != null && collision.transform == transform.parent)
        {
            return;
        }

        if (isGrip) return;


        string colNameLower = collision.transform.tag.ToLower();

        //if (flightTime < 0.5f && colNameLower.Contains("slingshot"))//|| colNameLower.Contains("arrow")))
        if (flightTime < 0.07f && (colNameLower.Contains("head") || colNameLower.Contains("body")))
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

        

        /* if(collision.collider.CompareTag("Body"))
         {
             if (PV.IsMine)
             {
                 if (!isGrip)
                 {
                     AudioManager.AM.PlaySE(hitPlayer);
                     ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                     Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ���Ͱ� �̷�� ȸ������ ����                            
                     var effect = Instantiate(wording_Hit, contact.point, rot);// �浹 ������ ����Ʈ ����        
                     transform.position = contact.point;
                     Destroy(effect, delTime);
                     PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered); // ��ų ȭ��
                 }
             }
         }

         if (collision.collider.CompareTag("Head"))
         {
             if (PV.IsMine)
             {
                 if (!isGrip)
                 {
                     AudioManager.AM.PlaySE(hitPlayer);
                     ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                     Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ���Ͱ� �̷�� ȸ������ ����                            
                     var effect = Instantiate(wording_Cr, contact.point, rot);// �浹 ������ ����Ʈ ����        
                     transform.position = contact.point;
                     Destroy(effect, delTime);
                     PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered); // ��ų ȭ��
                 }
             }
         }
 */

        /*if (collision.collider.CompareTag("Head"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    AudioManager.AM.PlaySE(headShot);
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ���Ͱ� �̷�� ȸ������ ����                            
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
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ���Ͱ� �̷�� ȸ������ ����                            
                    var effect = Instantiate(wording_Hit, contact.point, rot);// �浹 ������ ����Ʈ ����        
                    transform.position = contact.point;
                    Destroy(effect, delTime);
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered); // ��ų ȭ��

                }
            }
        }

        if (collision.collider.CompareTag("FloorBox") || collision.collider.CompareTag("Cube")
            || collision.collider.CompareTag("Obtacle"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    TrySticky(collision);
                    AudioManager.AM.PlaySE(iceImpact);
                    ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ���Ͱ� �̷�� ȸ������ ����                            
                    var effect = Instantiate(ballEX, contact.point, rot);
                    Destroy(effect, delTime);
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered);  // �⺻ ȭ��

                }
            }
        }

        if (collision.collider.CompareTag("SlingShot"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    TrySticky(collision);
                    PV.RPC(nameof(ImpactS), RpcTarget.AllBuffered);
                    ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ���Ͱ� �̷�� ȸ������ ����                            
                    var effect = Instantiate(ballEX, contact.point, rot);// �浹 ������ ����Ʈ ����        
                    transform.position = contact.point;
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered);  // �⺻ ȭ��
                    Destroy(effect, delTime);

                }

            }
        }
        if (collision.collider.CompareTag("Snowblock") || collision.collider.CompareTag("Iceblock")
            || collision.collider.CompareTag("NPC"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    TrySticky(collision);
                    AudioManager.AM.PlaySE(iceImpact);
                    ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ���Ͱ� �̷�� ȸ������ ����                            
                    var effect = Instantiate(ballEX, contact.point, rot);// �浹 ������ ����Ʈ ����        
                    transform.position = contact.point;
                    PV.RPC(nameof(DestroyBall), RpcTarget.AllBuffered);
                    Destroy(effect, delTime);
                }

            }
        }*/
    }

    [PunRPC]
    public void DelTagged()
    {
        StartCoroutine(DelayTag());
    }
    IEnumerator DelayTag()                            // �����ۻ��� ���� ���߻����������� �޼���
    {
        yield return new WaitForSeconds(0.1f);
        myColl.tag = "Icicle";
        damageColl.tag = "Icicle";
    }
}
