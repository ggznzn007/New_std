using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] Rigidbody rbody;
    [SerializeField] Renderer render;


    [SerializeField] float upForce = 1f;
    [SerializeField] float sideForce = 0.1f;

    private void OnEnable()
    {
        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(upForce, upForce);
        float zForce = Random.Range(-sideForce, sideForce);
        Vector3 force = new Vector3(xForce, yForce, zForce);
        rbody.velocity = force;

        Invoke(nameof(DeactiveDelay), 5);
    }

    public void Setup(Color color)
    {
        render.material.color = color;
    }

    void DeactiveDelay() => gameObject.SetActive(false);

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }
}
