using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Canvas pickuptext;
    private string itemtype;
    private GameObject player;
    private PlayerProperties playerprop;
    private bool collidewithplayer;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerprop = player.GetComponent<PlayerProperties>();
        itemtype = gameObject.name;
        pickuptext.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (collidewithplayer) 
        {
            pickuptext.enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerprop.AddItem(itemtype);
                Destroy(gameObject);
            }
        }
        else
        {
            pickuptext.enabled = false;
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
