using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyVFX : MonoBehaviour
{
    [System.Obsolete]
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().duration);
        Destroy(gameObject);
    }
}
