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

public class Quiver : MonoBehaviourPunCallbacks//XRBaseInteractable // 화살집
{

    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform spawnArea;
    [SerializeField] private int arrowCount = 10;

    /*protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        CreateAndSelectArrow(args);
    }

    private void CreateAndSelectArrow(SelectEnterEventArgs args)
    {
        // Create arrow, force into interacting hand
        Arrow arrow = CreateArrow(args.interactorObject.transform);
        interactionManager.SelectEnter(args.interactorObject, arrow);
    }*/

    /*private Arrow CreateArrow(Transform orientation)
    {
        // Create arrow, and get arrow component
        GameObject arrowObject = PN.Instantiate(arrowPrefab.name, orientation.position, orientation.rotation);
        return arrowObject.GetComponent<Arrow>();
    }*/
    private void Start()
    {
        if(PN.InRoom)
        CreateArrow();
    }

    private void CreateArrow()
    {
        // Create arrow, and get arrow component
        for (int i = 0; i < arrowCount; i++)
        {
            PN.Instantiate(arrowPrefab.name, spawnArea.position, spawnArea.rotation);
        }
        //GameObject arrowObject = PN.Instantiate(arrowPrefab.name, spawnArea.position, spawnArea.rotation);
        //return arrowObject.GetComponent<Arrow>();
    }

}
