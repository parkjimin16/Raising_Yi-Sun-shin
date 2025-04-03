using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPoolManager : MonoBehaviour
{
    public static CharacterPoolManager instance;

    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private int poolSize = 30;

    private Queue<GameObject> characterPool = new Queue<GameObject>();

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
            GameObject character = Instantiate(characterPrefab);
            character.SetActive(false);
            characterPool.Enqueue(character);
        }
    }

    public GameObject GetPooledCharacter(Vector3 spawnPos)
    {
        GameObject character;

        while (characterPool.Count > 0)
        {
            character = characterPool.Dequeue();
            if (character != null)
            {
                character.transform.position = spawnPos;
                character.SetActive(true);
                CharacterManager.instance.RegisterCharacter(character);
                return character;
            }
        }

        character = Instantiate(characterPrefab, spawnPos, Quaternion.identity);
        CharacterManager.instance.RegisterCharacter(character);
        return character;
    }

    public void ReturnCharacterToPool(GameObject character)
    {
        CharacterManager.instance.UnregisterCharacter(character); // 리스트에서 제거
        character.SetActive(false);
        characterPool.Enqueue(character);
    }
}
