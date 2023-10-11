using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject agentGO;
    public Animator animator;
    public Rigidbody2D spit;
    public GameObject spitspawnpoint;
    public GameObject lookatplayer;
    private AudioSource audioSource;
    public AudioSource audioSource1;
    private float currentspit = 0f;
    private GameObject player;
    private Vector3 playerposition;
    private int hp = 2;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        this.enabled = false;
    }

    void OnEnable()
    {
        PlayAudio();
        animator.SetBool("Active", true);
        currentspit = Time.time;
    }
    void Update()
    {
        if (agent.velocity.x < 0.3 && agent.velocity.y < 0.3 && agent.velocity.z < 0.3)
        {
            audioSource1.Pause();
            animator.SetBool("Active", false);
        }
        else
        {
            audioSource1.UnPause();
            animator.SetBool("Active", true);
        }
    }
    void FixedUpdate()
    {
        playerposition = new Vector3(player.transform.position.x, player.transform.position.y, 0f);
        agent.SetDestination(playerposition);

        if (Time.time - currentspit > 2f)
        {
            Rigidbody2D shotspit = Instantiate(spit, spitspawnpoint.transform.position, Quaternion.identity);
            shotspit.transform.rotation = transform.rotation;
            currentspit = Time.time;
        }
    }
    void TakeDamage()
    {
        hp--;
        if (hp == 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            TakeDamage();
        }
    }
    public void PlayAudio()
    {
        audioSource.Play();
        audioSource1.Play();
    }
}
