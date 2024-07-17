using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour
{
    void Start()
    {
        LivingRoom livingroom = new LivingRoom();
        livingroom.Couch = 2;
        livingroom.Light = 10;

        // clone LivingRoom object with Clone method
        // If you will not set the new value for any field the it will take the default value
        // from original object
        LivingRoom livingroomClone = (LivingRoom)livingroom.Clone();
        livingroomClone.Couch = 5; // 값만 수정해준다.
        livingroomClone.Light = 20; // 값만 수정해준다.

        Debug.Log("LivingRoom Details");
        Debug.LogFormat("Couch: {0} / Light: {1}", livingroom.Couch, livingroom.Light);

        Debug.Log("LivingroomClone Details");
        Debug.LogFormat("Couch: {0} / Light: {1}", livingroomClone.Couch, livingroomClone.Light);

        InnerRoom innerRoom = new InnerRoom();
        innerRoom.Bed = 2;
        innerRoom.Light = 15;

        // clone InnerRoom object with Clone method
        // If you will not set the new value for any field the it will take the default value
        // from original object
        InnerRoom innerRoomClone = (InnerRoom)innerRoom.Clone();
        innerRoomClone.Bed = 4; // 값만 수정해준다.
        innerRoomClone.Light = 30; // 값만 수정해준다.

        Debug.Log("InnerRoom Details");
        Debug.LogFormat("Bed: {0} / Light: {1}", innerRoom.Bed, innerRoom.Light);

        Debug.Log("innerRoomClone Details");
        Debug.LogFormat("Bed: {0} / Light: {1}", innerRoomClone.Bed, innerRoomClone.Light);
    }
}
