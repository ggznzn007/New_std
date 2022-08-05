using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class MultiplayerVRSynchronization : MonoBehaviourPun, IPunObservable
{

    private PhotonView m_PhotonView;



    //Main VRPlayer Transform Synch
    [Header("Main VRPlayer Transform Synch")]
    public Transform generalVRPlayerTransform;

    //Position
    private float m_Distance_GeneralVRPlayer;
    private Vector3 m_Direction_GeneralVRPlayer;
    private Vector3 m_NetworkPosition_GeneralVRPlayer;
    private Vector3 m_StoredPosition_GeneralVRPlayer;

    //Rotation
    private Quaternion m_NetworkRotation_GeneralVRPlayer;
    private float m_Angle_GeneralVRPlayer;


    //Main Avatar Transform Synch
    [Header("Main Avatar Transform Synch")]
    public Transform mainAvatarTransform;



    //Position
    private float m_Distance_MainAvatar;
    private Vector3 m_Direction_MainAvatar;
    private Vector3 m_NetworkPosition_MainAvatar;
    private Vector3 m_StoredPosition_MainAvatar;

    //Rotation
    private Quaternion m_NetworkRotation_MainAvatar;
    private float m_Angle_MainAvatar;

    //Head  Synch
    //Rotation
    [Header("Avatar Head Transform Synch")]
    public Transform headTransform;

    private Quaternion m_NetworkRotation_Head;
    private float m_Angle_Head;

    //Body Synch
    //Rotation
    [Header("Avatar Body Transform Synch")]
    public Transform bodyTransform;

    private Quaternion m_NetworkRotation_Body;
    private float m_Angle_Body;


    //Hands Synch
    [Header("Hands Transform Synch")]
    public Transform leftHandTransform;
    public Transform rightHandTransform;
    
  //  public Transform leftHandModel;// 추가
   // public Transform rightHandModel;// 추가

    //Left Hand Sync
    //Position
    private float m_Distance_LeftHand;
    //private float m_Distance_LeftHandModel;// 추가
    
    private Vector3 m_Direction_LeftHand;
    private Vector3 m_NetworkPosition_LeftHand;
    private Vector3 m_StoredPosition_LeftHand;
    
  

    // private Vector3 m_Direction_LeftHandModel;// 추가
    // private Vector3 m_NetworkPosition_LeftHandModel;// 추가
    // private Vector3 m_StoredPosition_LeftHandModel;// 추가

    //Rotation
    private Quaternion m_NetworkRotation_LeftHand;
    private float m_Angle_LeftHand; 
    
   
    //    private Quaternion m_NetworkRotation_LeftHandModel;// 추가
    // private float m_Angle_LeftHandModel;// 추가



    //Right Hand Synch
    //Position
    private float m_Distance_RightHand;
    //private float m_Distance_RightHandModel;// 추가

    private Vector3 m_Direction_RightHand;
    private Vector3 m_NetworkPosition_RightHand;
    private Vector3 m_StoredPosition_RightHand;

   // private Vector3 m_Direction_RightHandModel;// 추가
   // private Vector3 m_NetworkPosition_RightHandModel;// 추가
    //private Vector3 m_StoredPosition_RightHandModel;// 추가

    //Rotation
    private Quaternion m_NetworkRotation_RightHand;
    private float m_Angle_RightHand;

  //  private Quaternion m_NetworkRotation_RightHandModel;// 추가
  //  private float m_Angle_RightHandModel;// 추가





    bool m_firstTake = false;

    public void Awake()
    {
        m_PhotonView = GetComponent<PhotonView>();

        //Main VRPlayer Synch Init
        m_StoredPosition_GeneralVRPlayer = generalVRPlayerTransform.position;
        m_NetworkPosition_GeneralVRPlayer = Vector3.zero;
        m_NetworkRotation_GeneralVRPlayer = Quaternion.identity;

        //Main Avatar Synch Init
        m_StoredPosition_MainAvatar = mainAvatarTransform.localPosition;
        m_NetworkPosition_MainAvatar = Vector3.zero;
        m_NetworkRotation_MainAvatar = Quaternion.identity;

        //Head Synch Init
        m_NetworkRotation_Head = Quaternion.identity;

        //Body Synch Init
        m_NetworkRotation_Body = Quaternion.identity;

        //Left Hand Synch Init
        m_StoredPosition_LeftHand = leftHandTransform.localPosition;
        m_NetworkPosition_LeftHand = Vector3.zero;
        m_NetworkRotation_LeftHand = Quaternion.identity;

        //Left Hand Model Synch Init // 추가
       // m_StoredPosition_LeftHandModel = leftHandModel.localPosition;
       // m_NetworkPosition_LeftHandModel = Vector3.zero;
       // m_NetworkRotation_LeftHandModel = Quaternion.identity;

        //Right Hand Synch Init
        m_StoredPosition_RightHand = rightHandTransform.localPosition;
        m_NetworkPosition_RightHand = Vector3.zero;
        m_NetworkRotation_RightHand = Quaternion.identity;

        // Right Hand Model Synch Init // 추가
       // m_StoredPosition_RightHandModel = rightHandModel.localPosition;
       // m_NetworkPosition_RightHandModel = Vector3.zero;
       // m_NetworkRotation_RightHandModel = Quaternion.identity;
    }
 
    void OnEnable()
    {
        m_firstTake = true;
    }

    public void Update()
    {
        if (!this.m_PhotonView.IsMine)
        {

            generalVRPlayerTransform.position = Vector3.MoveTowards(generalVRPlayerTransform.position, this.m_NetworkPosition_GeneralVRPlayer, this.m_Distance_GeneralVRPlayer * (1.0f / PN.SerializationRate));
            generalVRPlayerTransform.rotation = Quaternion.RotateTowards(generalVRPlayerTransform.rotation, this.m_NetworkRotation_GeneralVRPlayer, this.m_Angle_GeneralVRPlayer * (1.0f / PN.SerializationRate));

            mainAvatarTransform.localPosition = Vector3.MoveTowards(mainAvatarTransform.localPosition, this.m_NetworkPosition_MainAvatar, this.m_Distance_MainAvatar * (1.0f / PN.SerializationRate));
            mainAvatarTransform.localRotation = Quaternion.RotateTowards(mainAvatarTransform.localRotation, this.m_NetworkRotation_MainAvatar, this.m_Angle_MainAvatar * (1.0f / PN.SerializationRate));



            headTransform.localRotation = Quaternion.RotateTowards(headTransform.localRotation, this.m_NetworkRotation_Head, this.m_Angle_Head * (1.0f / PN.SerializationRate));

            bodyTransform.localRotation = Quaternion.RotateTowards(bodyTransform.localRotation, this.m_NetworkRotation_Body, this.m_Angle_Body * (1.0f / PN.SerializationRate));


            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, this.m_NetworkPosition_LeftHand, this.m_Distance_LeftHand * (1.0f / PN.SerializationRate));
            leftHandTransform.localRotation = Quaternion.RotateTowards(leftHandTransform.localRotation, this.m_NetworkRotation_LeftHand, this.m_Angle_LeftHand * (1.0f / PN.SerializationRate));
           
            //leftHandModel.localPosition = Vector3.MoveTowards(leftHandModel.localPosition, this.m_NetworkPosition_LeftHandModel, this.m_Distance_LeftHandModel * (1.0f / PN.SerializationRate));  // 추가
            //leftHandModel.localRotation = Quaternion.RotateTowards(leftHandModel.localRotation, this.m_NetworkRotation_LeftHandModel, this.m_Angle_LeftHandModel * (1.0f / PN.SerializationRate)); // 추가


            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, this.m_NetworkPosition_RightHand, this.m_Distance_RightHand * (1.0f / PN.SerializationRate));
            rightHandTransform.localRotation = Quaternion.RotateTowards(rightHandTransform.localRotation, this.m_NetworkRotation_RightHand, this.m_Angle_RightHand * (1.0f / PN.SerializationRate));

           // rightHandModel.localPosition = Vector3.MoveTowards(rightHandModel.localPosition, this.m_NetworkPosition_RightHandModel, this.m_Distance_RightHandModel * (1.0f / PN.SerializationRate));// 추가
            //rightHandModel.localRotation = Quaternion.RotateTowards(rightHandModel.localRotation, this.m_NetworkRotation_RightHandModel, this.m_Angle_RightHandModel * (1.0f / PN.SerializationRate));// 추가
        }


    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)// 자신의 로컬 캐릭터인 경우 자신의 데이터를 다른 네트워크 유저에게 송신
        {
            //////////////////////////////////////////////////////////////////
            //General VRPlayer Transform Synch

            //Send Main Avatar position data
            this.m_Direction_GeneralVRPlayer = generalVRPlayerTransform.position - this.m_StoredPosition_GeneralVRPlayer;
            this.m_StoredPosition_GeneralVRPlayer = generalVRPlayerTransform.position;

            stream.SendNext(generalVRPlayerTransform.position);
            stream.SendNext(this.m_Direction_GeneralVRPlayer);

            //Send Main Avatar rotation data
            stream.SendNext(generalVRPlayerTransform.rotation);


            //////////////////////////////////////////////////////////////////
            //Main Avatar Transform Synch

            //Send Main Avatar position data
            this.m_Direction_MainAvatar = mainAvatarTransform.localPosition - this.m_StoredPosition_MainAvatar;
            this.m_StoredPosition_MainAvatar = mainAvatarTransform.localPosition;

            stream.SendNext(mainAvatarTransform.localPosition);
            stream.SendNext(this.m_Direction_MainAvatar);

            //Send Main Avatar rotation data
            stream.SendNext(mainAvatarTransform.localRotation);



            ///////////////////////////////////////////////////////////////////
            //Head rotation synch

            //Send Head rotation data
            stream.SendNext(headTransform.localRotation);


            ///////////////////////////////////////////////////////////////////
            //Body rotation synch

            //Send Body rotation data
            stream.SendNext(bodyTransform.localRotation);


            ///////////////////////////////////////////////////////////////////
            //Hands Transform Synch
            //Left Hand
            //Send Left Hand position data
            this.m_Direction_LeftHand = leftHandTransform.localPosition - this.m_StoredPosition_LeftHand;
            this.m_StoredPosition_LeftHand = leftHandTransform.localPosition;

            stream.SendNext(leftHandTransform.localPosition);
            stream.SendNext(this.m_Direction_LeftHand);

           // this.m_Direction_LeftHandModel = leftHandModel.localPosition - this.m_StoredPosition_LeftHandModel; // 추가
           // this.m_StoredPosition_LeftHandModel = leftHandModel.localPosition; // 추가

          //  stream.SendNext(leftHandModel.localPosition); // 추가
           // stream.SendNext(this.m_Direction_LeftHandModel); // 추가

            //Send Left Hand rotation data
           stream.SendNext(leftHandTransform.localRotation);

          //  stream.SendNext(leftHandModel.localRotation); // 추가

            //Right Hand
            //Send Right Hand position data
            this.m_Direction_RightHand = rightHandTransform.localPosition - this.m_StoredPosition_RightHand;
            this.m_StoredPosition_RightHand = rightHandTransform.localPosition;

            stream.SendNext(rightHandTransform.localPosition);
            stream.SendNext(this.m_Direction_RightHand);

          //  this.m_Direction_RightHandModel = rightHandModel.localPosition - this.m_StoredPosition_RightHandModel;// 추가
           // this.m_StoredPosition_RightHandModel = rightHandModel.localPosition;// 추가

           // stream.SendNext(rightHandModel.localPosition);// 추가
          //  stream.SendNext(this.m_Direction_RightHandModel);// 추가

            //Send Right Hand rotation data
            stream.SendNext(rightHandTransform.localRotation);

            //stream.SendNext(rightHandModel.localRotation); // 추가

        }
        else
        {
            ///////////////////////////////////////////////////////////////////
            //Ganeral VR Player Transform Synch

            //Get VR Player position data
            this.m_NetworkPosition_GeneralVRPlayer = (Vector3)stream.ReceiveNext();
            this.m_Direction_GeneralVRPlayer = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                generalVRPlayerTransform.position = this.m_NetworkPosition_GeneralVRPlayer;
                this.m_Distance_GeneralVRPlayer = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PN.Time - info.SentServerTime));
                this.m_NetworkPosition_GeneralVRPlayer += this.m_Direction_GeneralVRPlayer * lag;
                this.m_Distance_GeneralVRPlayer = Vector3.Distance(generalVRPlayerTransform.position, this.m_NetworkPosition_GeneralVRPlayer);
            }

            //Get Main Avatar rotation data
            this.m_NetworkRotation_GeneralVRPlayer = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                this.m_Angle_GeneralVRPlayer = 0f;
                generalVRPlayerTransform.rotation = this.m_NetworkRotation_GeneralVRPlayer;
            }
            else
            {
                this.m_Angle_GeneralVRPlayer = Quaternion.Angle(generalVRPlayerTransform.rotation, this.m_NetworkRotation_GeneralVRPlayer);
            }

            ///////////////////////////////////////////////////////////////////
            //Main Avatar Transform Synch

            //Get Main Avatar position data
            this.m_NetworkPosition_MainAvatar = (Vector3)stream.ReceiveNext();
            this.m_Direction_MainAvatar = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                mainAvatarTransform.localPosition = this.m_NetworkPosition_MainAvatar;
                this.m_Distance_MainAvatar = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PN.Time - info.SentServerTime));
                this.m_NetworkPosition_MainAvatar += this.m_Direction_MainAvatar * lag;
                this.m_Distance_MainAvatar = Vector3.Distance(mainAvatarTransform.localPosition, this.m_NetworkPosition_MainAvatar);
            }

            //Get Main Avatar rotation data
            this.m_NetworkRotation_MainAvatar = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                this.m_Angle_MainAvatar = 0f;
                mainAvatarTransform.rotation = this.m_NetworkRotation_MainAvatar;
            }
            else
            {
                this.m_Angle_MainAvatar = Quaternion.Angle(mainAvatarTransform.rotation, this.m_NetworkRotation_MainAvatar);
            }


            ///////////////////////////////////////////////////////////////////
            //Head rotation synch
            //Get Head rotation data 
            this.m_NetworkRotation_Head = (Quaternion)stream.ReceiveNext();

            if (m_firstTake)
            {
                this.m_Angle_Head = 0f;
                headTransform.localRotation = this.m_NetworkRotation_Head;
            }
            else
            {
                this.m_Angle_Head = Quaternion.Angle(headTransform.localRotation, this.m_NetworkRotation_Head);
            }

            ///////////////////////////////////////////////////////////////////
            //Body rotation synch
            //Get Body rotation data 
            this.m_NetworkRotation_Body = (Quaternion)stream.ReceiveNext();

            if (m_firstTake)
            {
                this.m_Angle_Body = 0f;
                bodyTransform.localRotation = this.m_NetworkRotation_Body;
            }
            else
            {
                this.m_Angle_Body = Quaternion.Angle(bodyTransform.localRotation, this.m_NetworkRotation_Body);
            }

            ///////////////////////////////////////////////////////////////////
            //Hands Transform Synch

            //Get Left Hand position data
            this.m_NetworkPosition_LeftHand = (Vector3)stream.ReceiveNext();
            this.m_Direction_LeftHand = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                leftHandTransform.localPosition = this.m_NetworkPosition_LeftHand;
                this.m_Distance_LeftHand = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PN.Time - info.SentServerTime));
                this.m_NetworkPosition_LeftHand += this.m_Direction_LeftHand * lag;
                this.m_Distance_LeftHand = Vector3.Distance(leftHandTransform.localPosition, this.m_NetworkPosition_LeftHand);
            }

            //Get Left Hand rotation data
            this.m_NetworkRotation_LeftHand = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                this.m_Angle_LeftHand = 0f;
                leftHandTransform.localRotation = this.m_NetworkRotation_LeftHand;
            }
            else
            {
                this.m_Angle_LeftHand = Quaternion.Angle(leftHandTransform.localRotation, this.m_NetworkRotation_LeftHand);
            }

            //Get Right Hand position data
            this.m_NetworkPosition_RightHand = (Vector3)stream.ReceiveNext();
            this.m_Direction_RightHand = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                rightHandTransform.localPosition = this.m_NetworkPosition_RightHand;
                this.m_Distance_RightHand = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PN.Time - info.SentServerTime));
                this.m_NetworkPosition_RightHand += this.m_Direction_RightHand * lag;
                this.m_Distance_RightHand = Vector3.Distance(rightHandTransform.localPosition, this.m_NetworkPosition_RightHand);
            }

            //Get Right Hand rotation data
            this.m_NetworkRotation_RightHand = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                this.m_Angle_RightHand = 0f;
                rightHandTransform.localRotation = this.m_NetworkRotation_RightHand;
            }
            else
            {
                this.m_Angle_RightHand = Quaternion.Angle(rightHandTransform.localRotation, this.m_NetworkRotation_RightHand);
            }
            if (m_firstTake)
            {
                m_firstTake = false;
            }



          /*  /////////////////////////////////////////////////////////////////////////////////////////////// 추가
            //Get Left Hand model position data
            this.m_NetworkPosition_LeftHandModel = (Vector3)stream.ReceiveNext();
            this.m_Direction_LeftHandModel = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                leftHandModel.localPosition = this.m_NetworkPosition_LeftHandModel;
                this.m_Distance_LeftHandModel = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PN.Time - info.SentServerTime));
                this.m_NetworkPosition_LeftHandModel += this.m_Direction_LeftHandModel * lag;
                this.m_Distance_LeftHandModel = Vector3.Distance(leftHandModel.localPosition, this.m_NetworkPosition_LeftHandModel);
            }

            //Get Left Hand rotation data
            this.m_NetworkRotation_LeftHandModel = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                this.m_Angle_LeftHandModel = 0f;
                leftHandModel.localRotation = this.m_NetworkRotation_LeftHandModel;
            }
            else
            {
                this.m_Angle_LeftHandModel = Quaternion.Angle(leftHandModel.localRotation, this.m_NetworkRotation_LeftHandModel);
            }

            //Get Right Hand position data
            this.m_NetworkPosition_RightHandModel = (Vector3)stream.ReceiveNext();
            this.m_Direction_RightHandModel = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                rightHandModel.localPosition = this.m_NetworkPosition_RightHandModel;
                this.m_Distance_RightHandModel = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PN.Time - info.SentServerTime));
                this.m_NetworkPosition_RightHandModel += this.m_Direction_RightHandModel * lag;
                this.m_Distance_RightHandModel = Vector3.Distance(rightHandModel.localPosition, this.m_NetworkPosition_RightHandModel);
            }

            //Get Right Hand rotation data
            this.m_NetworkRotation_RightHandModel = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                this.m_Angle_RightHandModel = 0f;
                rightHandModel.localRotation = this.m_NetworkRotation_RightHandModel;
            }
            else
            {
                this.m_Angle_RightHandModel = Quaternion.Angle(rightHandModel.localRotation, this.m_NetworkRotation_RightHandModel);
            }
            if (m_firstTake)
            {
                m_firstTake = false;
            }*/
        }
    }

}
