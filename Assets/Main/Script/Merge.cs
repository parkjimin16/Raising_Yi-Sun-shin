using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Progress;
using System.IO;
using System.Linq;
using System.Data;

[System.Serializable]
public class Item
{
    public string name;
    public int itemNum;
    public Sprite itemimg;
    public int itemgold;
    public float attack;
    public float hp;
    public GameObject panel = null;
    public bool spawncheck;
}

public class Merge : MonoBehaviour
{
    public List<Item> itemdata = new List<Item>();
    public GameObject itemPrefabs;
    public int ListMax;
    //public int childMax = 5;
    public int childCount;
    public chbool cb;

    public Vector3 objPosition1;
    public Vector3 mousePosition;

    public chData data = new chData();

    public AudioSource spawnbgm;
    public AudioSource panelbgm;

    private void Awake()
    {
        if (FindObjectsOfType<Merge>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        string filePath = Path.Combine(Application.dataPath, "chData.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<chData>(json);
            foreach (int itemIndex in data.itemNum2)
            {
                itemdata[itemIndex].spawncheck = true;
            }
            Debug.Log("Merge data loaded.");
        }
    }

    private void Update()
    {
        int effect_sound = PlayerPrefs.GetInt("EFFECT");

        if (effect_sound == 0)
        {
            spawnbgm.mute = false;
            panelbgm.mute = false;
        }

        if (effect_sound == 1)
        {
            spawnbgm.mute = true;
            panelbgm.mute = true;
        }
    }

    public void itemCreate(int num)
    {
        int childMax = PlayerPrefs.GetInt("ChildMax");
        int UpCh = PlayerPrefs.GetInt("Count");

        if ((num == UpCh || num < ListMax) && GameObject.Find("chp").transform.childCount < childMax && !cb.save)
        {
            spawnbgm.Play();

            Vector3 spawnPos = (num == UpCh)
                ? new Vector3(Random.Range(-2.2f, 2.2f), Random.Range(-3.5f, 2), 0)
                : Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));

            GameObject item = MergeItemPoolManager.instance.GetPooledMergeItem();
            item.transform.position = spawnPos;
            item.GetComponent<MergeItem>().InitItem(itemdata[num]);

            Invoke(nameof(UpdateStats), 0.2f);
        }

        if (cb.save)
        {
            GameObject item = MergeItemPoolManager.instance.GetPooledMergeItem();
            item.transform.position = objPosition1;
            item.GetComponent<MergeItem>().InitItem(itemdata[num]);
            cb.save = false;
        }

        if (!itemdata[num].spawncheck)
        {
            panelbgm.Play();
            Instantiate(itemdata[num].panel, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
            itemdata[num].spawncheck = true;
        }
    }
    private void UpdateStats()
    {
        GameObject chp = GameObject.Find("chp");
        if (chp != null)
        {
            var chParent = chp.GetComponent<chparent>();
            if (chParent != null)
            {
                chParent.attack();
                chParent.ch_hp();
                chParent.ch_gold();
            }
        }
    }

    /*void attacko()
    {
        GameObject.Find("chp").GetComponent<chparent>().attack();
    }

    void hpo()
    {
        GameObject.Find("chp").GetComponent<chparent>().ch_hp();
    }

    void goldo()
    {
        GameObject.Find("chp").GetComponent<chparent>().ch_gold();
    }*/
}
