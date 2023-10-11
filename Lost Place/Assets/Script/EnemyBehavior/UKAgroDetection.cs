using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UKAgroDetection : MonoBehaviour
{
    public GameObject enemy;
    private Unkillable script;
    void Awake()
    {
        script = enemy.GetComponent<Unkillable>();
        script.enabled = false;
    }
    void Start()
    {
        script.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Bullet"))
        {
            script.enabled = true;
        }
    }
}
