using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MergeItem : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Item item;
    private bool isSelected;
    private GameObject contactItem;
    private Animator animator;
    private Vector3 initialPosition;
    private int baseGold;

    public int iN;
    public bool SC;
    public GameObject goldImage;
    public float a1, a2, a3;
    private int gold;
    private float getGoldTime = 5.0f;

    public int Gold
    {
        get => gold;
        set => gold = Mathf.Max(0, value);
    }

    public float GetGoldTime
    {
        get => getGoldTime;
        set => getGoldTime = Mathf.Max(0, value);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
    }

    private void OnEnable()
    {
        StartCoroutine(GoldCoroutine());
    }

    private IEnumerator GoldCoroutine()
    {
        while (true)
        {
            float interval = PlayerPrefs.GetFloat("GetGoldTime");
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (sceneIndex == 1)
            {
                Gold = PlayerPrefs.GetInt("Gold");
                Vector3 pos = new Vector3(initialPosition.x, initialPosition.y + 0.35f, initialPosition.z);
                GameObject goldImgInstance = Instantiate(goldImage, pos, Quaternion.identity);
                Destroy(goldImgInstance, 0.125f);

                int upGold = PlayerPrefs.GetInt("UpGold");
                Gold += baseGold * upGold;
                PlayerPrefs.SetInt("Gold", gold);
                Debug.Log(interval);
            }
            else
            {
                Gold = PlayerPrefs.GetInt("Gold");
                int upGold = PlayerPrefs.GetInt("UpGold");
                Gold += baseGold * upGold;
                PlayerPrefs.SetInt("Gold", gold);
            }
            yield return new WaitForSeconds(interval);
        }
    }

    public void InitItem(Item newItem)
    {
        item = newItem;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.itemimg;
        baseGold = item.itemgold;

        SetParentToChp();
    }

    private void SetParentToChp()
    {
        GameObject chp = GameObject.FindGameObjectWithTag("chp");
        if (chp != null)
            transform.SetParent(chp.transform);
    }

    private void OnMouseDown()
    {
        isSelected = true;
    }

    private void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        transform.position = mousePos;
    }

    private void OnMouseUp()
    {
        isSelected = false;
        if (contactItem != null)
        {
            MergeItemPoolManager.instance.ReturnToPool(contactItem);
            MergeItemPoolManager.instance.ReturnToPool(gameObject);
            GameObject.Find("ItemData").GetComponent<Merge>().itemCreate(item.itemNum + 1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var otherMergeItem = collision.GetComponent<MergeItem>();
        if (isSelected && otherMergeItem != null && item.itemNum == otherMergeItem.item.itemNum)
        {
            contactItem = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var otherMergeItem = collision.GetComponent<MergeItem>();
        if (otherMergeItem != null && item.itemNum == otherMergeItem.item.itemNum)
            contactItem = null;
    }

    private void Update()
    {
        iN = item.itemNum;
        SC = item.spawncheck;
        animator.SetInteger("chnum", item.itemNum);
        a1 = item.attack;
        a2 = item.hp;
        a3 = item.itemgold;
        initialPosition = transform.position;
    }
}