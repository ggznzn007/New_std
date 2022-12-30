using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarInputConverter : MonoBehaviour
{

    //Avatar Transforms
    public Transform MainAvatarTransform;
    public Transform AvatarHead;
    public Transform AvatarBody;

    public Transform AvatarHand_Left;
    public Transform AvatarHand_Right;

    //XRRig Transforms
    public Transform XRHead;
    //public Transform XRCam;

    public Transform XRHand_Left;
    public Transform XRHand_Right;

    public Vector3 headPositionOffset;
    public Vector3 handRotationOffset;
    


    // Update is called once per frame
    void Update()
    {
        //XRHead.position = headPositionOffset;

        //Head and Body synch
        MainAvatarTransform.position = Vector3.Lerp(MainAvatarTransform.position, XRHead.position + headPositionOffset, 1f);
       // MainAvatarTransform.position = Vector3.Lerp(MainAvatarTransform.position, XRHead.position+headPositionOffset, 2f);
        AvatarHead.rotation = Quaternion.Lerp(AvatarHead.rotation, XRHead.rotation, 1f);
        //AvatarHead.rotation = Quaternion.Lerp(AvatarHead.rotation, XRHead.rotation,2f);
        AvatarBody.rotation = Quaternion.Lerp(AvatarBody.rotation, Quaternion.Euler(new Vector3(0, AvatarHead.rotation.eulerAngles.y, 0)), 1f);
       // AvatarBody.rotation = Quaternion.Lerp(AvatarBody.rotation, Quaternion.Euler(new Vector3(0, AvatarHead.rotation.eulerAngles.y, 0)),0.05f);

        //Hands synch  // 수정된 코드
       AvatarHand_Right.SetPositionAndRotation(Vector3.Lerp(AvatarHand_Right.position, XRHand_Right.position,1f),
         Quaternion.Lerp(AvatarHand_Right.rotation, XRHand_Right.rotation,1f) * Quaternion.Euler(handRotationOffset));
        //AvatarHand_Right.rotation = new Quaternion(AvatarHand_Right.rotation.x, AvatarHand_Right.rotation.y, AvatarHand_Right.rotation.z,0);

        //AvatarHand_Right.position = Vector3.Lerp(AvatarHand_Right.position, XRHand_Right.position, 0.5f);
        //AvatarHand_Right.rotation = Quaternion.Lerp(AvatarHand_Right.rotation, XRHand_Right.rotation, 0.5f) * Quaternion.Euler(handRotationOffset);

        // 수정된 코드
      AvatarHand_Left.SetPositionAndRotation(Vector3.Lerp(AvatarHand_Left.position,XRHand_Left.position,1f),
      Quaternion.Lerp(AvatarHand_Left.rotation,XRHand_Left.rotation,1f)*Quaternion.Euler(handRotationOffset));
//
       //AvatarHand_Left.position = Vector3.Lerp(AvatarHand_Left.position, XRHand_Left.position, 0.5f);
       // AvatarHand_Left.rotation = Quaternion.Lerp(AvatarHand_Left.rotation, XRHand_Left.rotation, 0.5f) * Quaternion.Euler(handRotationOffset);

       
    }
}
