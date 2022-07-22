using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class FireBullet : MonoBehaviour
{

    [SerializeField] Transform firePoint;  // √—±∏
    [SerializeField] BulletManager bulletPrefab; // √—æÀ «¡∏Æ∆’


    // [SerializeField] GameObject muzzleEffect; // √—±∏ ¿Ã∆Â∆Æ    
    private List<BulletManager> bulletPool = new List<BulletManager>();

    bool isEmpty = false;
    private readonly int bullMax = 5;

    private int curBulletIndex = 0;
    AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InitBullet();


    }

    private void Update()
    {
        if (curBulletIndex == bullMax)
        {
            InitBullet();
        }
    }

    public void InitBullet()
    {
        for (int i = 0; i < bullMax; ++i)
        {
            BulletManager bullet = Instantiate<BulletManager>(bulletPrefab);
            bullet.gameObject.SetActive(false);
            bulletPool.Add(bullet);

        }
    }

    public void Fire()
    {
        audioSource.Play();

        if (bulletPool[curBulletIndex].gameObject.activeSelf)
        {
            return;
        }
        bulletPool[curBulletIndex].transform.position = firePoint.position;
        bulletPool[curBulletIndex].transform.forward = firePoint.forward;

        bulletPool[curBulletIndex].gameObject.SetActive(true);

        // StartCoroutine(ReturnBullet());

        if (isEmpty)
        {
            bulletPool[curBulletIndex].gameObject.SetActive(false);
        }

        if (curBulletIndex >= bullMax - 1)
        {
            curBulletIndex = 0;
            isEmpty = true;
        }
        else
        {
            curBulletIndex++;
        }
        //GameObject muzzle = Instantiate(muzzleEffect);
        // var spawnedBullet = BulletPool.GetBullet();
        // muzzle.transform.position = firePoint.position;
        // spawnedBullet.transform.position = firePoint.position;
        // spawnedBullet.transform.forward = firePoint.forward;
        // Destroy(muzzle,0.5f);
        // Destroy(spawnedBullet,2f);

    }

    public IEnumerator ReturnBullet()
    {
        yield return new WaitForSeconds(1f);

    }

}

