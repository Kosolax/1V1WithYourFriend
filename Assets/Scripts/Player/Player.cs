using Mirror;

using UnityEngine;
using UnityEngine.UI;

public class Player : BasePlayer
{
    [Header("Player Health Settings")]
    public Text PlayerLifeText;
    private GunShootRaycast gunShootRaycast;

    public bool IsDead { get; set; }

    public void Die()
    {
        this.IsDead = true;
        this.CharacterController.enabled = false;

        this.Respawn();
    }

    protected override void StartOverridable()
    {
        base.StartOverridable();
        this.Die();
    }

    [Command]
    public void ITookDamage(float damage)
    {
        this.UpdateHpForOthers(damage);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        uint netId = this.GetComponent<NetworkIdentity>().netId;
        GameManager.AddPlayer(netId, this);
    }

    protected override void Initialise()
    {
        base.Initialise();
        this.gunShootRaycast = new GunShootRaycast();
        this.hasFinishInitialisation = true;
    }

    protected override void InitialisePlayer()
    {
        base.InitialisePlayer();
        this.Reset();
    }

    private void Reset()
    {
        this.IsDead = false;
        this.Health = this.MaxHealth;
        this.PlayerLifeText.text = this.Health.ToString();
        this.CharacterController.enabled = true;
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
        if (this.IsDead)
        {
            return;
        }

        this.Health -= damage;
        this.PlayerLifeText.text = this.Health.ToString();
        if (this.Health <= 0)
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