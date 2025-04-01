using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelEvent : MonoBehaviour
{
    public GameObject panel;
    public Game2Data dm;
    public DataManager dm1;

    private void OnEnable()
    {
        if (dm == null)
            dm = GameObject.Find("Save").GetComponent<Game2Data>();
        if (dm1 == null)
            dm1 = GameObject.Find("Save").GetComponent<DataManager>();
    }
    public void OnPanel()
    {
        panel.SetActive(true);
        dm.Game2save();
        dm1.SaveGameData();
    }

    public void OffPanel()
    {
        panel.SetActive(false);
    }
}
