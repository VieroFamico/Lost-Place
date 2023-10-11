using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAgroDetection : MonoBehaviour
{
    public GameObject enemy;
    private MeleeEnemy script;
    void Awake()
    {
        script = enemy.GetComponent<MeleeEnemy>();
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
