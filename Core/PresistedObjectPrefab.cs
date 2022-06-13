using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresistedObjectPrefab : MonoBehaviour
{
    [SerializeField] GameObject presistedObject;
    static bool hasSpawned = false;
    private void Awake()
    {
        if (hasSpawned) return;
        SpawnPresistedObject();
        hasSpawned = true;
    }

    private void SpawnPresistedObject()
    {
        GameObject presisted = Instantiate(presistedObject);
        DontDestroyOnLoad(presisted);
    }
}
