using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BulletManager : MonoBehaviour                                        // �Ѿ� ��ũ��Ʈ
{
    [SerializeField] GameObject bullet;  // �Ѿ� ������

    AudioSource audioSource;             // �Ѿ� �߻� �Ҹ�
    public float speed = 40f;            // �Ѿ� �ӵ�
    public int tagCount = 0;

    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnEnable()
    {
        audioSource.Play();                                                                     // �Ѿ� �߻� �Ҹ� ���
        rb.velocity = GameObject.FindGameObjectWithTag("FirePoint").transform.forward * speed;
        

        Invoke(nameof(DeactiveDelay), 1);
    }
    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ������ ����Ʈ ��������
        if (collision.gameObject.CompareTag("Cube"))
        {
            var exEffect = ObjectPooler.SpawnFromPool<ExploreEffect>("ExplorelEffect", this.transform.position);
            DeactiveDelay();

            
           
            Debug.Log("�±׵�");
        }
    }


    void DeactiveDelay() => gameObject.SetActive(false);

   
}

