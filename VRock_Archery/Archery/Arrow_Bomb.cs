using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;
using PN = Photon.Pun.PN;

public class Arrow_Bomb : Arrow
{
    /// <summary>
    ///  ��źȭ�� ��ü ��Ŀ����
    ///  1. FireMissileTiny, Sparks == ������ Ȱ��ȭ
    ///  2. ExploArea == ������ ��Ȱ��ȭ -> ��źȭ���� �߻�Ǿ� ��ǥ���� ���߽� Ȱ��ȭ�ǰ� ���߿����� ���, ��ź ȭ�� �޽� ��Ȱ��ȭ
    ///  3. ���� ȿ�� Ȱ��ȭ�Ǹ� ���߿����� ����, ExploArea ��Ȱ��ȭ, areaColl ª�� ���� Ȱ��ȭ�Ǿ��ٰ� ����� �԰� �ٽ� ��Ȱ��ȭ
    ///  4. ��źȭ�� �ı�
    /// </summary>
    public Collider tagColl;                                                       // ��źȭ�� �±� �ݶ��̴�
    public Collider areaColl;                                                      // ���� ����� ���� �ݶ��̴�    
    public GameObject myMesh;                                                      // ��źȭ�� �޽�
    public GameObject[] myEX;                                                      // ��źȭ�� ȿ����(FireMissileTiny, Sparks, ExploArea)
    private bool isRotate;                                                         // ȸ�� ����

    private readonly float delTime = 0.4f;                                         // ������ �ð�
    private readonly float bombTime = 1f;                                          // ���߽ð�

    protected override void Awake()
    {
        base.Awake();
        isRotate = true;                                                           // ȸ�� on
        rigidbody.useGravity = false;                                              // �߷� off        
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        DataManager.DM.grabArrow = true;                                           // ȭ�� �׸� on
        isRotate = false;                                                          // ȸ�� off
        isGrip = true;                                                             // �׸� on
        PV.RPC(nameof(DelayTagged), RpcTarget.AllBuffered);
        PV.RPC(nameof(OnColl), RpcTarget.AllBuffered);
        // rigidbody.useGravity = true;
        //damageColl.gameObject.SetActive(false);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        isGrip = false;                                                            // �׸� off
        //damageColl.gameObject.SetActive(true); 
        DataManager.DM.grabArrow = false;                                          // ȭ�� �׸� off
                                               
        if (args.interactorObject is Notch notch)                                  // Ȱ ������ �������� ��
        {
            //damageColl.gameObject.SetActive(false);
            if (notch.CanRelease)
            {
                DataManager.DM.arrowNum = 3;
                LaunchArrow(notch);                                                // ȭ�� �߻� �޼���                
                /* if (PV.IsMine)
                 {
                     if (!PV.IsMine) return;
                     PV.RPC(nameof(Active_EX), RpcTarget.AllBuffered);
                 }*/

            }
        }

    }

    

    private void OnCollisionEnter(Collision collision)
    {
       
        // Ignore parent collisions
        if (transform.parent != null && collision.transform == transform.parent)
        {
            return;
        }

        //if (isGrip) return;

        string colNameLower = collision.transform.name.ToLower();
                
       /* if (flightTime < 0.05f && (colNameLower.Contains("head") || colNameLower.Contains("body")))
        {
            Physics.IgnoreCollision(collision.collider, myColl, true);
            return;
        }*/
        //if (flightTime < 0.01f && (colNameLower.Contains("player") || colNameLower.Contains("lefthand") || colNameLower.Contains("righthand")))
        if (colNameLower.Contains("lefthand") || colNameLower.Contains("righthand"))
        {
            Physics.IgnoreCollision(collision.collider, myColl, true);
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
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ���Ͱ� �̷�� ȸ������ ����                            
                    var effect = Instantiate(arrowEX, contact.point, rot);// �浹 ������ ����Ʈ ����        
                    transform.position = contact.point;                    
                    PV.RPC(nameof(BombA), RpcTarget.AllBuffered);
                } // �浹EX �ı��޼���� �浹EX�� �پ�����

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
                    var effect = Instantiate(arrowEX, contact.point, rot);// �浹 ������ ����Ʈ ����        
                    transform.position = contact.point;                    
                    PV.RPC(nameof(BombA), RpcTarget.AllBuffered);
                }// �浹EX �ı��޼���� �浹EX�� �پ�����
            }
        }

        if (collision.collider.CompareTag("FloorBox") || collision.collider.CompareTag("Cube") || collision.collider.CompareTag("Obtacle"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)                                 // ȭ���� �������
                {
                    AudioManager.AM.PlaySE(aImpact);
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ���Ͱ� �̷�� ȸ������ ����                            
                    var effect = Instantiate(arrowEX, contact.point, rot);// �浹 ������ ����Ʈ ����      
                    PV.RPC(nameof(BombA), RpcTarget.AllBuffered);
                }  // �浹EX �ı��޼���� �浹EX�� �پ�����
                else if(!isGrip&& !launched)                            // ȭ���� �����ʰ� �տ��� ������ ���
                {
                    PV.RPC(nameof(DelayArrow), RpcTarget.AllBuffered);
                }
            }
        }

      
        if (collision.collider.CompareTag("NPC") || collision.collider.CompareTag("Effect"))
        {
            if (PV.IsMine)
            {
                if (!PV.IsMine) return;
                if (!isGrip && launched)
                {
                    AudioManager.AM.PlaySE(aImpact);
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ���Ͱ� �̷�� ȸ������ ����                            
                    var effect = Instantiate(arrowEX, contact.point, rot);// �浹 ������ ����Ʈ ����
                    transform.SetParent(collision.transform);                    
                    PV.RPC(nameof(DestroyArrow), RpcTarget.AllBuffered);
                }// �浹EX �ı��޼���� �浹EX�� �پ�����
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
                    TrySticky(collision);
                    ContactPoint contact = collision.contacts[0];// �浹������ ������ ����                        
                    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);// ���� ���Ͱ� �̷�� ȸ������ ����                            
                    var effect = Instantiate(arrowEX, contact.point, rot);// �浹 ������ ����Ʈ ����        
                    transform.position = contact.point;                    
                    PV.RPC(nameof(BombA), RpcTarget.AllBuffered);
                }// �浹EX �ı��޼���� �浹EX�� �پ�����

            }
        }

      

    }

    public new void TrySticky(Collision coll)                               // ȭ���� ��ǥ���� ������ �� �޼���
    {
        Rigidbody colRid = coll.collider.GetComponent<Rigidbody>();
        transform.parent = null;

        if (coll.gameObject.isStatic) // ����������Ʈ
        {
            //transform.SetParent(coll.transform);
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.isKinematic = true;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        else if (colRid != null && !colRid.isKinematic) // ������ٵ� �ְ� ����������Ʈ
        {
            transform.SetParent(coll.transform);
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = colRid;
            joint.enableCollision = false;
            joint.breakForce = float.MaxValue;
            joint.breakTorque = float.MaxValue;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else if (colRid != null && colRid.isKinematic && coll.transform.localScale == Vector3.one) // ������ٵ� �ְ� �����̰� ���ý������� 1
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
            if (coll.transform.localScale == Vector3.one) // ���ý������� 1
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

    }

    private void Update()
    {
        RotArrow();
    }

    public void RotArrow()
    {
        if (isRotate)
        {
            transform.Rotate(rotSpeed * Time.deltaTime * Vector3.forward);
        }
        else
        {
            rotSpeed = 0;
        }
    }

    [PunRPC]
    public void BombA()                                                       // RPC ��ź ȭ���� ������ ����
    {
        StartCoroutine(Explode());
    }
    public IEnumerator Explode()                                              // ��ź ȭ�� �ڷ�ƾ
    {
        AudioManager.AM.PlaySX(bombBeep);                                                      // ���� ������ ����� ��� 
        myEX[2].SetActive(true);                                              // ���ߴ���� ���� ǥ�� ON
        yield return new WaitForSeconds(2);
        StartCoroutine(ExOnOff());
        // PV.RPC(nameof(BombArrow), RpcTarget.AllBuffered);                     // RPC ���� ��ƼŬ ���
    }

    [PunRPC]
    public void BombArrow()
    {
        StartCoroutine(ExOnOff());
    }

    public IEnumerator ExOnOff()
    {
        AudioManager.AM.StopSX(bombBeep);
        myMesh.SetActive(false);                                              // ��źȭ�� ������ OFF
        myEX[0].SetActive(false);                                             // ��źȭ�� ȿ�� OFF
        myEX[1].SetActive(false);                                             // ��źȭ�� ȿ��2 OFF
        myEX[2].SetActive(false);                                             // ���ߴ���� ���� ǥ�� OFF
        effect.SetActive(true);                                               // ������ƼŬ ON
        StartCoroutine(CollCtrl());
        yield return new WaitForSeconds(1.5f);
        effect.SetActive(false);                                              // ������ƼŬ OFF
        Destroy(PV.gameObject);                                               // ��źȭ�� �ı�
    }

    IEnumerator CollCtrl()
    {
        areaColl.enabled = true;
        yield return new WaitForSeconds(0.03f);
        areaColl.enabled = false;
    }  

    [PunRPC]
    public void DestroyBomb()
    {
        Destroy(gameObject, bombTime);
    }

    [PunRPC]
    public void OnColl()
    {
        tagColl.gameObject.SetActive(true);
    }
}
