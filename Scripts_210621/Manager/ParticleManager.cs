using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Particle
{
    public string paticleName;
    public GameObject gameObject;
}

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;

    [Header("Fairy Effects")]
    [SerializeField] Particle[] Feffect;

    [Header("Plant Effects")]
    [SerializeField] Particle[] Peffect;

    [Header("Button Effects")]
    [SerializeField] Particle[] Beffect;

   /* [Header("Button Effects")]
    [SerializeField] Particle[] Teffect;
*/
    [Header("Environment Effects")]
    [SerializeField] Particle[] Eeffect;

    //Instance
    void Awake() { instance = this; }

    public void PlayFaiEf(string _paticleName, Pose hitPose, float delayTime)
    {
        for (int i = 0; i < Feffect.Length; i++)
        {
            if (_paticleName == Feffect[i].paticleName)
            {
                //GameObject particleF = Instantiate(Feffect[i].gameObject, Vector3.zero, Quaternion.identity);
                GameObject particleF = Instantiate(Feffect[i].gameObject, 
                    hitPose.position /*+ new Vector3(0f, 0.2f, 0f)*/, hitPose.rotation);
                Destroy(particleF, delayTime);                      
            }
        }
    }

    public void PlayPlaEf(string _paticleName, RaycastHit hit, float delayTime)
    {
        for (int i = 0; i < Peffect.Length; i++)
        {
            if (_paticleName == Peffect[i].paticleName)
            {
                //GameObject particleF = Instantiate(Feffect[i].gameObject, Vector3.zero, Quaternion.identity);
                GameObject particleP = Instantiate(Peffect[i].gameObject,
                    hit.transform.position /*+ new Vector3(0f, 0.2f, 0f)*/, hit.transform.rotation);
                Destroy(particleP, delayTime);

            }
        }
    }

    public void PlayBtnEf(string _paticleName, Pose hitPose, float delayTime)
    {
        for (int i = 0; i < Beffect.Length; i++)
        {
            if (_paticleName == Beffect[i].paticleName)
            {
                GameObject particleB = Instantiate(Beffect[i].gameObject,
                    hitPose.position, hitPose.rotation);

                Destroy(particleB, delayTime);                
            }
        }
    }

    /*public void PlayTxtEf(string _paticleName)
    {
        for (int i = 0; i < Teffect.Length; i++)
        {
            if (_paticleName == Teffect[i].paticleName)
            {
                GameObject particleT = Instantiate(Teffect[i].gameObject,
                            Vector3.zero, Quaternion.identity);

                Destroy(particleT, 2f);

            }
        }
    }*/

    public void PlayEnvEf(string _paticleName)
    {
        for (int i = 0; i < Eeffect.Length; i++)
        {
            if (_paticleName == Eeffect[i].paticleName)
            {
                Instantiate(Eeffect[i].gameObject, Vector3.zero, Quaternion.identity);                
            }
        }
    }

    internal void PlayBtnEf(string v1, object hitPose, float v2)
    {
        throw new NotImplementedException();
    }
}
