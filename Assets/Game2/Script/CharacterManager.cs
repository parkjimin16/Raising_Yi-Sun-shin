using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    public List<GameObject> characters = new List<GameObject>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterCharacter(GameObject character)
    {
        if (!characters.Contains(character))
            characters.Add(character);
    }

    public void UnregisterCharacter(GameObject character)
    {
        if (characters.Contains(character))
            characters.Remove(character);
    }
}
