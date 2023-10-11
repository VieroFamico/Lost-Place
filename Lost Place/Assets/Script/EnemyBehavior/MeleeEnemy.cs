using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject agentGO;
    public Animator animator;
    private AudioSource audioSource;
    public AudioSource audioSource1;
    private GameObject player;
    private PlayerProperties playerprop;
    private Vector3 playerposition;
    private Vector3 tempplayerposition;
    private float hp = 3;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerprop = player.GetComponent<PlayerProperties>();
        audioSource = GetComponent<AudioSource>();
        this.enabled = false;
    }
    void OnEnable()
    {
        PlayAudio();
        animator.SetBool("Active", true);
    }
    void FixedUpdate()
    {
        playerposition = new Vector3(player.transform.position.x, player.transform.position.y, 0f);
        agent.SetDestination(playerposition);
        
    }

    void TakeDamage()
    {
        hp--;
        if(hp == 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(AttackWait());
            playerprop.TakeDamage(20f);
        }
        else if(collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage();
        }
    }
    IEnumerator AttackWait()
    {
        Vector3 temp = agent.velocity;
        agent.velocity = Vector3.zero;
        audioSource1.Pause();
        animator.SetBool("Active", false);
        yield return new WaitForSeconds(1.5f);
        agent.velocity = temp;
        audioSource1.UnPause();
        animator.SetBool("Active", true);
    }
    public void PlayAudio()
    {
        audioSource.Play();
        audioSource1.Play();
    }
}
