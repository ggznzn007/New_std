using UnityEngine;

public class ProjectileExplosive : MonoBehaviour
{
    private Explosive explosive;

    private void Awake()
    {
        explosive = GetComponent<Explosive>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        explosive.TakeDamage(10000);
    }

    private void OnCollisionStay(Collision collision)
    {
        explosive.TakeDamage(100);
    }

    private void OnCollisionExit(Collision collision)
    {
        explosive.TakeDamage(1);
    }
}
