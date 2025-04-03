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
    private float maxSearchDistance = 13f;
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
        Movement();
        FindNearestObject();
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
            EnemyPoolManager.instance.ReturnToPool(gameObject);
            EnemyManager.instance.UnregisterEnemy(gameObject);

            var spawnManager = FindObjectOfType<enemySpawn>();
            spawnManager.cur -= 1;
            spawnManager.dieenemy += 1;

            int gameGold = PlayerPrefs.GetInt("GameGold");
            PlayerPrefs.SetInt("GameGold", gameGold + 1);
        }
    }
    private void FindNearestObject()
    {
        List<GameObject> objects = CharacterManager.instance.characters;
        float shortestDistance = maxSearchDistance;
        GameObject nearest = null;

        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = obj;
            }
        }
        target = nearest != null ? nearest : castle;
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
    void attack()
    {
        shootbgm.Play();

        Collider2D[] hitObjects = Physics2D.OverlapBoxAll(boxpos.position, boxSize, 0);
        foreach (Collider2D hit in hitObjects)
        {
            if (hit.CompareTag("player"))
            {
                Spawn player = hit.GetComponent<Spawn>();
                player.hp -= atk;
                StartCoroutine(player.HitColorAnimation());
            }
            else if (hit.CompareTag("castle"))
            {
                hit.GetComponent<castledata>().hp -= atk;
            }
        }
    }
}
