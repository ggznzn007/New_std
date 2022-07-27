using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class FireBullet : MonoBehaviour
{
    
    [SerializeField] Transform firePoint;  // ÃÑ±¸
    [SerializeField] GameObject bulletPrefab; // ÃÑ¾Ë ÇÁ¸®ÆÕ   
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
            Debug.Log("¹Ù´Ú¿¡ ÅÂ±×µÊ");
            Destroy(gameObject, 1.5f);
        }
    }

}

