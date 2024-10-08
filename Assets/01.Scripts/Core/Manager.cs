﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager<T> : MonoBehaviour where T : Manager<T>
{
    [Tooltip("매니저 작성용 싱글톤")]
    public static T Instance;

    protected void Awake()
    {
        Instance = (T)this;
    }
}