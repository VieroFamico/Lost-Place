using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class Bullet : MonoBehaviour
{
    public float speed;
    void Start()
    {

    }
    private void Update()
    {
        transform.Translate(new Vector3(0, speed * Time.fixedDeltaTime * Time.timeScale, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
