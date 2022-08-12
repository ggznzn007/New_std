using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using PN = Photon.Pun.PN;
using Random = UnityEngine.Random;
using TMPro;

public class Poolable : MonoBehaviourPun  // 싱글 플레이 풀방식
{
    protected BulletPool pool;

    public virtual void CreateBullet(BulletPool pool)
    {
        this.pool = pool;
        gameObject.SetActive(false);
    }


    public virtual void Push()
    {
        // pool.Push(this);
    }

    public virtual void Enqueue()
    {
        pool.Enqueue(this);
    }


}
