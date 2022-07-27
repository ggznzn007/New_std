using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class FireBullet : MonoBehaviour
{
    
    [SerializeField] Transform firePoint;  // �ѱ�
    [SerializeField] GameObject bulletPrefab; // �Ѿ� ������   
    AudioSource audioSource;

    
    private void Start()
    {
       // audioSource = GetComponent<AudioSource>();       
    }  

    public void Fire()
    {
       // audioSource.Play();

       var bullet = ObjectPooler.SpawnFromPool<BulletManager>("Bullet",firePoint.forward);
       bullet.transform.position = firePoint.position; 
       
      

       var muzzle = ObjectPooler.SpawnFromPool<MuzzleEffect>("MuzzleEffect", firePoint.position);
       //ObjectPooler.SpawnFromPool<MuzzleEffect>("MuzzleEffect", firePoint.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            Debug.Log("�ٴڿ� �±׵�");
            Destroy(gameObject, 1.5f);
        }
    }

}

