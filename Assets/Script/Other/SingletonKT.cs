using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonKT<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance => _instance;

    protected virtual bool UseDontDestroyOnLoad => false;

    protected virtual void Awake()
    {
        // Debug.Log("Server Awake");
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this as T;

        if (UseDontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
}