using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit : MonoBehaviour
{
    public float speed;
    public GameObject player;
    private PlayerProperties playerprop;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerprop = player.GetComponent<PlayerProperties>();
    }
    private void Update()
    {
        transform.Translate(new Vector3(0, speed * Time.fixedDeltaTime * Time.timeScale, 0));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerprop.TakeDamage(15f);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
