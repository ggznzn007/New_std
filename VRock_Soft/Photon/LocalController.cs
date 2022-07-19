using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SpatialTracking;
using System;

public class LocalController : MonoBehaviour
{
    [SerializeField] bool _showDebugValues = false;
    /*[SerializeField] */public Transform _relativeTo;

    [SerializeField] InputActionAsset _actionMap;

    [Header( "Input Actions:" )]
    [Header( "Events" )]
    [SerializeField] public InputActionProperty _gripGrab;
    [SerializeField] public InputActionProperty _gripDrop;
    [SerializeField] InputActionProperty _triggerGrab;
    [SerializeField] InputActionProperty _triggerDrop;
    [SerializeField] InputActionProperty _WeaponDown;
    [SerializeField] InputActionProperty _WeaponUp;
    [Header( "States" )]
    [SerializeField] InputActionProperty _teleportMode;

    NormalHand Hand;
    public NormalPlayerSetup Setup;

    private void Start()
    {
        Hand = GetComponent<NormalHand>();
    }

    protected void OnEnable()
    {
        _actionMap.Enable();
    }

    protected void OnDisable()
    {
        _actionMap.Disable();
    }

    public Vector3 GetLocalPosition()
    {
        return _relativeTo.InverseTransformPoint(transform.position);
    }
    public Quaternion GetLocalRotation()
    {
        return Quaternion.Inverse(_relativeTo.rotation) * transform.rotation;
    }

    //VR 컨트롤러 Grip, Trigger , Button 눌렀는지 유무 확인
    public void NormalInput()
    {
        if (_gripGrab.action.triggered)
        {
            Hand.Grip = true;
        }

        if (_gripDrop.action.triggered)
        {
            Hand.Grip = false;
        }

        if (_triggerGrab.action.triggered)
        {
            Hand.Trigger = true;
        }

        if (_triggerDrop.action.triggered)
        {
            Hand.Trigger = false;
        }

        if (_WeaponDown.action.triggered)
        {
            Setup.ImgCount -= 1;
            if (Setup.ImgCount < 1)
            {
                Setup.ImgCount = 3;
            }

            Setup.ImgColor = true;
            Setup.SettingImgColor = true;
        }

        if (_WeaponUp.action.triggered)
        {
            Setup.ImgCount += 1;
            if (Setup.ImgCount > 3)
            {
                Setup.ImgCount = 1;
            }

            Setup.ImgColor = true;
            Setup.SettingImgColor = true;
        }
    }
}
