using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    
    private float hp;
    private float hpmax = 100f;
    private float stamina;
    private float staminamax = 100f;
    public int totalammo = 10;
    public int ammo = 6;
    public int medkit = 1;
    private int glowstick = 2;

    private GameObject playerGO;
    public Player player;
    public GameObject managerGO;
    public GameManager manager;
    public GameObject inventory;
    

    void Awake()
    {
        hp = hpmax;
        stamina = staminamax;
        totalammo = 10;
        ammo = 6;
        medkit = 1;
        glowstick = 2;
        manager.UpdateHP(hp);
    }

    void Start()
    {
        StartingSetUp();
    }
    public void StartingSetUp()
    {
        hp = hpmax;
        stamina = staminamax;
        totalammo = 10;
        ammo = 6;
        medkit = 1;
        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Player>();
        managerGO = GameObject.FindGameObjectWithTag("GameManager");
        manager = managerGO.GetComponent<GameManager>();
        manager.UpdateHP(hp);
        BulletCounter();
        MedkitCounter();
        GlowStickCounter();
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        manager.UpdateHP(hp);
        player.audio.clip = player.hurt;
        player.audio.Play();
        if(hp <= 0)
        {
            hp = 0;
            manager.UpdateHP(hp);
            manager.DeathScreen();
        }
    }
    public void UseStamina(float value) 
    {
        stamina -= value;
        manager.UpdateStamina(stamina);
    }
    public void RegenerateStamina(float value)
    {
        if (stamina <= 100)
        {
            stamina += value;
        }  
        manager.UpdateStamina(stamina);
    }
    public void Heal(int value)
    {
        hp += value;
        if(hp > hpmax)
        {
            hp = hpmax;
        }
        manager.UpdateHP(hp);
        MedkitCounter();
    }
    public float Health()
    {
        return hp;
    }
    public float Stamina()
    {
        return stamina;
    }
    public int Medkit()
    {
        if (medkit <= 0) StartCoroutine(manager.Notification("Ran out of Medkit"));
        return medkit;
    }
    public int GlowStick()
    {
        if (glowstick <= 0) StartCoroutine(manager.Notification("Ran out of Glowstick"));
        return glowstick;
    }

    public void BulletCounter()
    {
        manager.UpdateBullet(ammo, totalammo);
    }
    public void MedkitCounter()
    {
        manager.UpdateMedkit(medkit);
    }
    public void GlowStickCounter()
    {
        manager.UpdateGlowStick(glowstick);
    }
    public void UseGlowStick()
    {
        glowstick--;
        GlowStickCounter();
    }

    public void InventoryDisplay(bool state)
    {
        if(state == false)
        {
            inventory.SetActive(true);
        }
        else
        {
            inventory.SetActive(false);
        }
    }

    public void Pause()
    {
        manager.PauseScreen();
    }

    public void AddItem(string itemtype)
    {
        if(itemtype == "Ammo")
        {
            totalammo += 6;
            BulletCounter();
            StartCoroutine(manager.Notification("Found 6 Bullets!"));
            player.audio.clip = player.pickup;
        }
        else if(itemtype == "Medkit")
        {
            medkit += 1;
            MedkitCounter();
            StartCoroutine(manager.Notification("Found a Medkit!"));
            player.audio.clip = player.pickup;
        }
        else if (itemtype == "GlowStick")
        {
            glowstick += 1;
            GlowStickCounter();
            StartCoroutine(manager.Notification("Found a Glowstick!"));
            player.audio.clip = player.pickup;
        }
        else if(itemtype == "Key")
        {
            manager.UnlockDoor();
            StartCoroutine(manager.Notification("Found The Key! Now Get Out!!!"));
            player.audio.clip = player.keypickup;
        }
        player.audio.Play();
    }
}
