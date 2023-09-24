using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepUI : MonoBehaviour
{
    private static KeepUI instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
