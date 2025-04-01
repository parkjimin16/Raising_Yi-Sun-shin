using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeItemPoolManager : MonoBehaviour
{
    public static MergeItemPoolManager instance;

    [SerializeField] private GameObject mergeItemPrefab;
    [SerializeField] private int poolSize = 20;

    private Queue<GameObject> mergeItemPool = new Queue<GameObject>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject item = Instantiate(mergeItemPrefab);
            item.SetActive(false);
            mergeItemPool.Enqueue(item);
            DontDestroyOnLoad(item);
        }
    }

    public GameObject GetPooledMergeItem()
    {
        if (mergeItemPool.Count > 0)
        {
            GameObject item = mergeItemPool.Dequeue();
            item.SetActive(true);
            return item;
        }
        else
        {
            GameObject item = Instantiate(mergeItemPrefab);
            return item;
        }
    }

    public void ReturnToPool(GameObject item)
    {
        item.transform.SetParent(null);
        item.transform.position = Vector3.zero;

        item.SetActive(false);
        mergeItemPool.Enqueue(item);
    }
}
