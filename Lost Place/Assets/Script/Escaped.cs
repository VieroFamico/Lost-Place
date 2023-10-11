using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class Escaped : MonoBehaviour
{
    public GameObject managerGO;
    public GameManager manager;
    void Start()
    {
        managerGO = GameObject.FindGameObjectWithTag("GameManager");
        manager = managerGO.GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            manager.Escape();
        }
    }
}
