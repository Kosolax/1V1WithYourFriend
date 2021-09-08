using System;
using System.Linq;

using Mirror;

using TMPro;

using UnityEngine;

public class ZombiePlayer : BasePlayer
{
    [SyncVar] public float Money;

    public int SpawnIndex;

    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI HealthText;

    public ZombiePlayerMoneyManager ZombiePlayerMoneyManager { get; set; }

    [ClientRpc]
    public void SetMoney(float money)
    {
        this.Money = money;
    }

    protected override void StartOverridable()
    {
        base.StartOverridable();
        this.MoneyText.text = this.Money.ToString();
        this.Health = this.MaxHealth;
        this.HealthText.text = this.Health.ToString();
        this.CharacterController.enabled = false;
        var localIndex = (FindObjectsOfType<ZombiePlayer>()).ToList().IndexOf(this);
        this.SpawnIndex = localIndex % NetworkManager.startPositions.Count;
        Transform spawnPoint = NetworkManager.startPositions[this.SpawnIndex];
        this.transform.position = spawnPoint.position;
        this.CharacterController.enabled = true;
    }

    protected override void Initialise()
    {
        base.Initialise();
        this.ZombiePlayerMoneyManager = new ZombiePlayerMoneyManager(this);
        this.hasFinishInitialisation = true;
    }

    protected override void UpdateOverridable()
    {
        base.UpdateOverridable();
        // We don't want our action to be applied on others players
        if (!this.isLocalPlayer || !this.hasFinishInitialisation)
        {
            return;
        }

        // NOTE : Here we put everything that need to RUN even if we are in a menu

        if (MainMenu.isOn)
        {
            return;
        }

        // NOTE : Here we put everything that need to STOP when we are in a menu
    }

    [Command(requiresAuthority = false)]
    public void TakeDamage(float damage)
    {
        this.UpdateHpForOthers(damage);
    }

    [ClientRpc]
    private void UpdateHpForOthers(float damage)
    {
        this.Health -= damage;
        this.HealthText.text = this.Health.ToString();
        if (this.Health <= 0)
        {

        }
    }
}
