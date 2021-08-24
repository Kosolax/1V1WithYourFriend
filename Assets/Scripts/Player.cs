using Mirror;

using UnityEngine;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    public Behaviour[] DisableOnDeath;

    public float Health;

    public float MaxHealth;

    public MenuController menuController;

    public Text PlayerLife;

    public bool[] WasEnableOnStart;

    [SyncVar]
    private bool isDead = false;

    public bool IsDead { get => this.isDead; set => this.isDead = value; }

    public void Die()
    {
        isDead = true;
        for (int i = 0; i < DisableOnDeath.Length; i++)
        {
            DisableOnDeath[i].enabled = false;
        }

        this.GetComponent<CharacterController>().enabled = false;

        this.Respawn();
    }

    [Command]
    public void ITookDamage(float damage)
    {
        this.UpdateHpForOthers(damage);
    }

    public void Reset()
    {
        isDead = false;
        this.Health = this.MaxHealth;
        this.PlayerLife.text = this.Health.ToString();
        for (int i = 0; i < WasEnableOnStart.Length; i++)
        {
            DisableOnDeath[i].enabled = WasEnableOnStart[i];
        }

        this.GetComponent<CharacterController>().enabled = true;
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

    private void Respawn()
    {
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        this.transform.position = spawnPoint.position;
        this.transform.rotation = spawnPoint.rotation;

        this.Reset();
    }

    private void SetHealth(float damage)
    {
        if (isDead)
        {
            return;
        }

        this.Health -= damage;
        this.PlayerLife.text = this.Health.ToString();
        if (this.Health <= 0)
        {
            this.Die();
        }
    }

    private void Update()
    {
        if (!this.isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            this.Die();
        }
    }

    [ClientRpc]
    private void UpdateHpForOthers(float damage)
    {
        GameManager.Players[this.name].SetHealth(damage);
    }
}