using System.Collections;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionDelayTime = 0.01f;
    [SerializeField] private float explosionRadius = 20f;  // default 10f
    [SerializeField] private float explosionForce = 450f;  // default 500f
    [SerializeField] private int maxHP = 100;

    private int currentHP;
    private bool isExploded = false;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if(currentHP<=0&&!isExploded)
        {
            StartCoroutine(nameof(OnExplosion));
        }
    }

    private IEnumerator OnExplosion()
    {
        yield return new WaitForSeconds(explosionDelayTime);

        // StackOverflow ����
        isExploded = true;

        // ���� ����Ʈ ����
        if(explosionPrefab != null )
        {
            Bounds bounds = GetComponent<Collider>().bounds;
            Instantiate(explosionPrefab, bounds.center, transform.rotation);
        }

        // ���� ������ �ִ� ��� ������Ʈ�� �ݶ��̴� ������ �޾ƿ� ���� ȿ�� ó��
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider hit in colliders)
        {
            // �÷��̾�, �� �� ü���� �����ϰų� �߰����� ó���� �ʿ��� �� ���⼭ ó��

            // ���� ������ �浹�� ������Ʈ ������ ���� ó��
            if(hit.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                rigidbody.AddExplosionForce(explosionForce,transform.position, explosionRadius);
            }
        }
        Destroy(gameObject);
    }
}
