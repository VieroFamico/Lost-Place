using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Canvas findkeytext;
    private GameObject player;
    private bool collidewithplayer;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        findkeytext.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (collidewithplayer)
        {
            findkeytext.enabled = true;
        }
        else
        {
            findkeytext.enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collidewithplayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collidewithplayer = false;
        }
    }
}
