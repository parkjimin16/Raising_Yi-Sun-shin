using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.GraphicsBuffer;

public class enemydata : MonoBehaviour
{
    public float hp;
    public int atk;
    public GameObject enemyspawn;
    public List<GameObject> FoundObjects;
    public float shortDis;
    public float speed;
    Animator animator;
    public Transform boxpos;
    public Vector2 boxSize;
    Rigidbody2D myrigidbody;

    public SpriteRenderer sprite;
    public int curcoin;

    public AudioSource shootbgm;
    private GameObject target;
    private GameObject castle;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        castle = GameObject.Find("castle");
        myrigidbody = GetComponent<Rigidbody2D>();
        enemyspawn = GameObject.Find("enemySpawn");
        //atk = 5;
    }

    // Update is called once per frame
    void Update()
    {
        int effect_sound = PlayerPrefs.GetInt("EFFECT");

        if (effect_sound == 0)
        {
            shootbgm.mute = false;
        }

        if (effect_sound == 1)
        {
            shootbgm.mute = true;
        }

        CheckEnemyHealth();

        /*if (Player == false)
        {
            if (Vector2.Distance(gameObject.transform.position, castle.transform.position) < 13f && Vector2.Distance(gameObject.transform.position, castle.transform.position) >= 2.0f)
            {
                animator.SetBool("isattack", false);
                transform.position = Vector2.MoveTowards(transform.position, castle.transform.position, Time.deltaTime * speed);
                
            }
            else if(Vector2.Distance(gameObject.transform.position, castle.transform.position) < 3.8f)
            {
                animator.SetBool("isattack",true);
                //Debug.Log("1");
            }
        }
        else
        {
            if(Vector2.Distance(gameObject.transform.position, Player.transform.position) < 13f && Vector2.Distance(gameObject.transform.position, Player.transform.position) >= 1f)
                {
                    animator.SetBool("isattack", false);
                    transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, Time.deltaTime * speed);
                }
            else if(Vector2.Distance(gameObject.transform.position, Player.transform.position) < 1f)
            {
                animator.SetBool("isattack", true);
            }
        }*/
        //shortD();
        Movement();
        FindNearestPlayer();
    }
    void die()
    {
        hp -= Time.deltaTime;
        if(hp < 0)
        {
            Destroy(gameObject);
            GameObject.Find("enemySpawn").GetComponent<enemySpawn>().max -= 1;
            //hp = 5;
        }
    }
    /*void shortD()
    {
        try
        {
            FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("player"));
            shortDis = Vector3.Distance(gameObject.transform.position, FoundObjects[0].transform.position);
            Player = FoundObjects[0];
            foreach (GameObject found in FoundObjects)
            {
                float Distance = Vector3.Distance(gameObject.transform.position, found.transform.position);

                if (Distance < shortDis) // 위에서 잡은 기준으로 거리 재기
                {
                    Player = found;
                    shortDis = Distance;
                }
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex);
        }
    }*/
    void attack()
    {
        shootbgm.Play();

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(boxpos.position, boxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "player")
            {
                
                GameObject.FindGameObjectWithTag("player").GetComponent<Spawn>().hp -= atk;
                if(GameObject.FindGameObjectWithTag("player").GetComponent<Spawn>().hp > 0)
                {
                    StartCoroutine(GameObject.FindGameObjectWithTag("player").GetComponent<Spawn>().HitColorAnimation());
                }
                
            }
            if (collider.tag == "castle")
            {
                castle.GetComponent<castledata>().hp -= atk;
                Debug.Log(castle.GetComponent<castledata>().hp);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxpos.position, boxSize);
    }

    public IEnumerator HitColorAnimation()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(1f);
        sprite.color = Color.white;
    }

    private void CheckEnemyHealth()
    {
        if (hp <= 0)
        {
            ObjectPoolManager.instance.ReturnToPool(gameObject);
            var spawnManager = FindObjectOfType<enemySpawn>();
            spawnManager.cur -= 1;
            spawnManager.dieenemy += 1;

            int gameGold = PlayerPrefs.GetInt("GameGold");
            PlayerPrefs.SetInt("GameGold", gameGold + 1);
        }
    }
    private void FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("player");
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                target = player;
            }
        }
    }
    private void Movement()
    {
        if (target == null || Vector2.Distance(transform.position, target.transform.position) > 13f)
            target = castle;

        float distance = Vector2.Distance(transform.position, target.transform.position);

        if(target == castle)
        {
            if (distance > 1.8f)
            {
                animator.SetBool("isattack", false);
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
            }
            else
            {
                animator.SetBool("isattack", true);
            }
        }
        else
        {
            if (distance > 1f)
            {
                animator.SetBool("isattack", false);
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
            }
            else
            {
                animator.SetBool("isattack", true);
            }
        }
    }
}
