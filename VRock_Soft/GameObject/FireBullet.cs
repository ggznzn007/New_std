using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FireBullet : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject firePoint;
    [SerializeField] float bulletSpeed = 15f;
    public void Fire()
    {
      GameObject spawnedBullet =  Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        spawnedBullet.GetComponent<Rigidbody>().velocity = bulletSpeed * firePoint.transform.forward;
        Destroy(spawnedBullet,2f);
    }

}