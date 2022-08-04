using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    protected BulletPool pool;

    public virtual void CreateBullet(BulletPool pool)
    {
        this.pool = pool;
        gameObject.SetActive(false);
    }

   /* public virtual void Push()
    {
        pool.Push(this);
    }*/

    public virtual void Enqueue()
    {
        pool.Enqueue(this);
    }
}
