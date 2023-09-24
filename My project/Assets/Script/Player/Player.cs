using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class Player : MonoBehaviour
{
    public float walkspeed;
    public float runspeed;
    private float speed;
    private bool runnable;
    private bool healing = false;

    public PlayerProperties playerProperties;
    public Animator animator;
    public new AudioSource audio;
    public AudioSource audio1; 

    public Rigidbody2D bullet;
    public GameObject bulletspawnpoint;
    public GameObject flashlight;

    private Rigidbody2D rb2d;
    private Camera maincam;
    private Vector3 mousepos;
    private float movespeed;
    public Vector3 offset;

    private int state = 0;

    private bool Gun = false;
    private float currentshoot = 0f;
    [SerializeField] private GameObject muzzleflash;
    [SerializeField] public AudioClip gunshot, reload, hurt, walk, run, pickup, keypickup;

    private bool GlowStick = false;
    public GameObject glowsticklight;
    private void Start()
    {
        maincam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb2d = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        Debug.Log(playerProperties.ammo + "left//");
        Debug.Log(playerProperties.totalammo);
    }
    private void Update()
    {
        if(playerProperties.manager.pause == false)
        {
            Movement();
            EquipedItem();
        }
        Menus();
    }
    private void Menus()
    {
       if (Input.GetKeyDown(KeyCode.Escape))
        {
            playerProperties.Pause();
        }
    }
    private void FixedUpdate()
    {
        Camera();

        maincam.transform.position = new Vector3(rb2d.position.x + offset.x, rb2d.position.y + offset.y, offset.z);
    }
    private void Camera()
    {
        mousepos = maincam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = new Vector3(mousepos.x - transform.position.x, mousepos.y - transform.position.y, 
        (float)-10);
        float rotz = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotz - 90);
    }
    private void EquipedItem()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && state != 1)
        {
            animator.SetBool("Gun", true);
            animator.SetBool("GlowStick", false);
            Gun = true;
            flashlight.SetActive(false);
            state = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && state != 2 && !GlowStick)
        {
            if(playerProperties.GlowStick() <= 0)
            {
                
            }
            else
            {
                animator.SetBool("Gun", false);
                animator.SetBool("GlowStick", true);
                Gun = false;
                playerProperties.UseGlowStick();
                StartCoroutine(GlowStickActive());
                flashlight.SetActive(false);
                state = 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q) && state != 0)
        {
            animator.SetBool("Gun", false);
            animator.SetBool("GlowStick", false);
            Gun = false;
            flashlight.SetActive(true);
            state = 0;
        }

        if(Input.GetKeyDown(KeyCode.H) && playerProperties.Health() < 100 && !healing)
        {
            if (playerProperties.Medkit() <= 0)
            {
                
            }
            else
            {
                StartCoroutine(HealingDelay());
            }
        }

        switch (state)
        {
            case 0:

                break;
            case 1:
                if (Input.GetMouseButtonDown(0) && Gun && Time.time - currentshoot > 0.5f)
                {
                    if (playerProperties.ammo > 0)
                    {
                        animator.SetTrigger("Shoot");
                        Rigidbody2D shotbullet = Instantiate(bullet, bulletspawnpoint.transform.position, Quaternion.identity);
                        shotbullet.transform.rotation = transform.rotation;
                        playerProperties.ammo--;
                        currentshoot = Time.time;
                        playerProperties.BulletCounter();
                        audio.clip = gunshot;
                        audio.Play();
                        StartCoroutine(FlashWait(seconds: 0.1f));
                    }
                    else
                    {
                        
                    }

                }
                if (Input.GetKeyDown(KeyCode.R) && playerProperties.ammo < 6)
                {
                    StartCoroutine(Reload());
                }
                break;
            case 2:
                
                break;
        }
    }
    private void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //rb2d.AddForce(new Vector2(moveHorizontal * speed, moveVertical * speed) - rb2d.velocity, ForceMode2D.Force);

        rb2d.velocity = new Vector2(moveHorizontal * speed, moveVertical * speed);

        movespeed = Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical);

        if (Mathf.Abs(rb2d.velocity.x) > 0.2 || Mathf.Abs(rb2d.velocity.y) > 0.2)
        {
            audio1.enabled = true;
        }
        else
        {
            audio1.enabled = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && playerProperties.Stamina() > 0)
        {
            if(movespeed > 0.1)
            {
                animator.SetFloat("Speed", movespeed + 4.5f);
                speed = runspeed;
                audio1.clip = run;
                if(!audio1.isPlaying && audio1.enabled == true && Time.timeScale == 1f)
                {
                    audio1.Play();
                }
                playerProperties.UseStamina(0.3f);
                
            }
            else
            {
                animator.SetFloat("Speed", movespeed);
                speed = walkspeed;
            }
        }
        else if(Input.GetKey(KeyCode.LeftShift) && playerProperties.Stamina() <= 0)
        {
            animator.SetFloat("Speed", movespeed);
            speed = walkspeed;
            audio1.clip = walk;
            if (!audio1.isPlaying && audio1.enabled == true && Time.timeScale == 1f)
            {
                audio1.Play();
            }
        }
        else if(!Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("Speed", movespeed);
            speed = walkspeed;
            audio1.clip = walk;
            if (!audio1.isPlaying && audio1.enabled == true && Time.timeScale == 1f)
            {
                audio1.Play();
            }
            if (movespeed < 0.01)
            {
                rb2d.velocity = Vector2.zero;
            }
            playerProperties.RegenerateStamina(0.4f);
        }
        
    }

    IEnumerator FlashWait(float seconds)
    {
        muzzleflash.SetActive(true);
        yield return new WaitForSeconds(seconds);
        muzzleflash.SetActive(false);
    }
    IEnumerator GlowStickActive()
    {
        GlowStick = true;
        glowsticklight.SetActive(true);
        yield return new WaitForSeconds(15f);
        GlowStick = false;
        glowsticklight.SetActive(false);
    }
    IEnumerator HealingDelay()
    {
        rb2d.constraints = RigidbodyConstraints2D.FreezePosition;
        healing = true;
        playerProperties.medkit--;
        yield return new WaitForSeconds(2f);
        rb2d.constraints = RigidbodyConstraints2D.None;
        healing = false;
        playerProperties.Heal(30);
    }
    IEnumerator Reload()
    {
        float temp = speed;
        speed = 0.5f;
        Gun = false;
        if (6 - playerProperties.ammo >= playerProperties.totalammo)
        {
            playerProperties.ammo += playerProperties.totalammo;
            playerProperties.totalammo = 0;
            audio.clip = reload;
            audio.Play();
        }
        else
        {
            if(playerProperties.totalammo != 0)
            {
                int tempammo = playerProperties.ammo;
                playerProperties.ammo += 6 - playerProperties.ammo;
                playerProperties.totalammo -= 6 - tempammo;
                audio.clip = reload;
                audio.Play();
            }
        }
        Debug.Log(playerProperties.ammo + "left//");
        Debug.Log(playerProperties.totalammo);
        
        yield return new WaitForSeconds(1.5f);
        playerProperties.BulletCounter();
        speed = temp;
        Gun = true;
    }
}
