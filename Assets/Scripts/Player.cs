using System;
using System.Collections;
using System.Collections.Generic;

using Mirror;

using UnityEngine;

public class Player : NetworkBehaviour
{
    public float Health;

    public float MaxHealth;

    private void Update()
    {
        if (!this.isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            this.ITookDamage(999);
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        this.Reset();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        this.transform.position = spawnPoint.position;
        this.transform.rotation = spawnPoint.rotation;
    }

    [SyncVar]
    private bool isDead = false;

    public Behaviour[] DisableOnDeath;

    public bool[] WasEnableOnStart;

    public bool IsDead
    {
        get => this.isDead;
        set => this.isDead = value;
    }

    [Command]
    public void ITookDamage(float damage)
    {
        this.UpdateHpForOthers(damage);
    }

    [ClientRpc]
    private void UpdateHpForOthers(float damage)
    {
        GameManager.Players[this.name].SetHealth(damage);
    }

    private void SetHealth(float damage)
    {
        if (isDead)
        {
            return;
        }

        this.Health -= damage;
        if (this.Health <= 0)
        {
            this.Die();
        }
    }

    public void Setup()
    {
        WasEnableOnStart = new bool[DisableOnDeath.Length];
        for (int i = 0; i < WasEnableOnStart.Length; i++)
        {
            WasEnableOnStart[i] = DisableOnDeath[i].enabled;
        }

        this.Reset();
    }

    public void Reset()
    {
        isDead = false;
        this.Health = this.MaxHealth;
        for (int i = 0; i < WasEnableOnStart.Length; i++)
        {
            DisableOnDeath[i].enabled = WasEnableOnStart[i];
        }
    }

    public void Die()
    {
        isDead = true;
        for (int i = 0; i < DisableOnDeath.Length; i++)
        {
            DisableOnDeath[i].enabled = false;
        }

        this.StartCoroutine(this.Respawn());
    }
}