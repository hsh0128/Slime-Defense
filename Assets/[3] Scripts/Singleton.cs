using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : class
{
    private static T _instance;
    public static T instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        _instance = this as T;
        DontDestroyOnLoad(gameObject);

        Init();
    }

    protected virtual void Init()
    {

    }
}