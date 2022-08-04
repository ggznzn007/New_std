using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager poolManager;
    public static PoolManager PoolingManager
    {
        get
        { 
            return poolManager;
        }
    }

    public BulletPool pool;

    private void Awake()
    {
        if(poolManager)
        {
            Destroy(gameObject);
            return;
        }
        poolManager = this;
    }
}
