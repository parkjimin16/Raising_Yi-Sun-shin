using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    public GameObject enemyPrefab;
    private int poolSize = 30;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;

        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetPooledEnemy()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(enemyPrefab);
            return obj;
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
