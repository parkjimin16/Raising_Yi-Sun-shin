using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[System.Serializable]
public class chData
{
    public List<int> itemNum1 = new List<int>();
    public List<Vector3> Pos1 = new List<Vector3>();
    public List<int> itemNum2 = new List<int>();
    public List<int> itemNum3 = new List<int>();
    public int chCount;
    public bool save;
}

public class DataManager : MonoBehaviour
{
    /*public GameObject itemPrefabs;
    public MergeItem mg;
    public Merge mg1;
    public chbool cb;
    public chData data = new chData();*/

    public GameObject itemPrefab;
    public MergeItem mergeItem;
    public Merge merge;
    public chbool chBool;
    public chData data = new chData();


    /*private void Start()
    {
        var objs = FindObjectsOfType<DataManager>();
        if (objs.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        mg = mg.GetComponent<MergeItem>();
    }*/
    private void Awake()
    {
        if (FindObjectsOfType<DataManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void LoadGameData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "chData.json");
        Debug.Log(filePath);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<chData>(json);

            var mergeComponent = GameObject.Find("ItemData").GetComponent<Merge>();
            foreach (int itemNum in data.itemNum2)
            {
                mergeComponent.itemdata[itemNum].spawncheck = true;
            }

            for (int i = 0; i < data.itemNum1.Count; i++)
            {
                chBool.save = true;
                merge.objPosition1 = data.Pos1[i];
                merge.itemCreate(data.itemNum1[i]);
            }

            Debug.Log("Load complete");
        }
    }

    public void SaveGameData()
    {
        Transform chParent = GameObject.Find("chp").transform;
        int childCount = chParent.childCount;

        data.itemNum1.Clear();
        data.itemNum2.Clear();
        data.Pos1.Clear();

        for (int i = 0; i < childCount; i++)
        {
            var mItem = chParent.GetChild(i).GetComponent<MergeItem>();
            data.itemNum1.Add(mItem.iN);
            data.Pos1.Add(chParent.GetChild(i).position);
        }

        var mergeComponent = GameObject.Find("ItemData").GetComponent<Merge>();
        for (int i = 0; i < mergeComponent.itemdata.Count; i++)
        {
            if (mergeComponent.itemdata[i].spawncheck)
                data.itemNum2.Add(mergeComponent.itemdata[i].itemNum);
        }

        data.chCount = childCount;
        data.save = true;

        string json = JsonUtility.ToJson(data, true);
        string filePath = Path.Combine(Application.persistentDataPath, "chData.json");
        File.WriteAllText(filePath, json);
        Debug.Log("Save complete");
    }
}
