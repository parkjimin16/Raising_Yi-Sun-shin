using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawn : MonoBehaviour
{
    public GameObject enemyPrefabs;
    public int max;
    public int maxcount;
    public int cur;
    Rigidbody2D myrigidbody;
    public GameObject panel;
    public GameObject enemyat;
    public int dieenemy;

    private float spawntime;

    // Start is called before the first frame update
    void Start()
    {
        enemyat.GetComponent<enemydata>().atk = 5;
        dieenemy = 0;
        max = 0;
        cur = maxcount;
        //spawn();
        //InvokeRepeating("spawn", 2f, 1f);
        StartCoroutine(spawn());
        myrigidbody = GetComponent<Rigidbody2D>();
    }

    private IEnumerator spawn()
    {
        while (true)
        {
            float RandomX = Random.Range(-2.9f, 2.9f);
            Vector3 enemypos = new Vector3(RandomX, 6.6f, 1f);

            GameObject go = ObjectPoolManager.instance.GetPooledEnemy();
            go.transform.position = enemypos;

            max += 1;

            if (max % 5 == 0)
            {
                enemyat.GetComponent<enemydata>().hp += 0.3f;
            }
            spawntime = Random.Range(0.5f, 1.6f);
            yield return new WaitForSeconds(spawntime);

            // if (spawntime > 0.5f)
            //  spawntime -= 0.2f;
        }

    }
}
