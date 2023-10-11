using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Unkillable : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject agentGO;
    public Animator animator;
    private AudioSource audioSource;
    public AudioSource audioSource1;
    private GameObject player;
    private PlayerProperties playerprop;
    private Vector3 playerposition;
    private float hp = 100;
    private int damagetaken = 0;
    private NavMeshPath pathToPlayer;
    private Vector3 originalposition;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerprop = player.GetComponent<PlayerProperties>();
        audioSource = GetComponent<AudioSource>();
        originalposition = this.transform.position;
        pathToPlayer = new NavMeshPath();
        this.enabled = false;
    }
    void OnEnable()
    {
        PlayAudio();
        animator.SetBool("Active", true);
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

        if (agent.CalculatePath(playerposition, pathToPlayer) && pathToPlayer.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(playerposition);
        }
        else
        {
            agent.SetDestination(originalposition);
            float distanceToTarget = Vector3.Distance(transform.position, originalposition);
            if (distanceToTarget < 1f)
            {
                audioSource1.UnPause();
                animator.SetBool("Active", true);
                this.enabled = false;
            }
        }
    }

    void TakeDamage()
    {
        hp--;
        damagetaken++;
        if (damagetaken >2)
        {
            StartCoroutine(playerprop.manager.Notification("It doesn't seem to be working..."));
            damagetaken = 1;
        }
        if (hp == 0)
        {
            Destroy(transform.parent.gameObject);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(AttackWait());
            playerprop.TakeDamage(20f);
        }
        else if (collision.gameObject.CompareTag("Bullet"))
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
        yield return new WaitForSeconds(1f);
        agent.velocity = temp;
        audioSource1.UnPause();
        animator.SetBool("Active", true);
    }
    public void PlayAudio()
    {
        audioSource.Play();
    }
}
