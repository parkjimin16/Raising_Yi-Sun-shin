using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class ClickLimit : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private Image image;
    [SerializeField] private Merge merge;

    private float imageTime;
    private float timeLimit;
    private int clickNum = 5;
    private int clickMax;

    private int upCh;
    private float spawnTime;

    private void Awake()
    {
        Input.multiTouchEnabled = false;
        int resetCount = PlayerPrefs.GetInt("UpCh");
        PlayerPrefs.SetInt("Count", resetCount);

        clickNum = PlayerPrefs.GetInt("ClickNum", 5);
    }

    private void OnEnable()
    {
        if (merge == null)
            merge = GameObject.Find("ItemData").GetComponent<Merge>();

        btn.onClick.AddListener(() => merge.itemCreate(upCh));
    }

    private void Update()
    {
        int childMax = PlayerPrefs.GetInt("ChildMax");
        clickMax = PlayerPrefs.GetInt("ClickMax");
        spawnTime = PlayerPrefs.GetFloat("SpawnTime");
        upCh = PlayerPrefs.GetInt("Count");

        if (clickMax > clickNum && image.fillAmount > 0.0f)
        {
            timeLimit += Time.deltaTime;
            if (timeLimit > spawnTime)
            {
                IncreaseClick();
                btn.interactable = true;
                timeLimit = 0f;
            }
            imageTime = Time.deltaTime;
            image.fillAmount -= imageTime / spawnTime;
            if (image.fillAmount <= 0.0f)
                image.fillAmount = 1;
        }

        if (timeLimit > spawnTime && clickMax == clickNum)
            timeLimit = 0f;

        if (clickNum == 0 || childMax == GameObject.Find("chp").transform.childCount)
            gameObject.GetComponent<Button>().interactable = false;
        else
            gameObject.GetComponent<Button>().interactable = true;
    }
    
    public void Click()
    {
        imageTime = 0f;
        clickNum = Mathf.Max(0, PlayerPrefs.GetInt("ClickNum", clickNum) - 1);
        PlayerPrefs.SetInt("ClickNum", clickNum);
    }

    private void IncreaseClick()
    {
        clickNum = PlayerPrefs.GetInt("ClickNum", clickNum) + 1;
        PlayerPrefs.SetInt("ClickNum", clickNum);
        image.fillAmount = 1;
    }
}
