using System;

using Mirror;

using UnityEngine;

public class ZombiePlayer : BasePlayer
{
    [SyncVar] public float Money;
    [SyncVar] public float MoneyPerSecond;

    public IWeapon Weapon;

    public int SpawnIndex;

    public ZombiePlayerMoneyManager ZombiePlayerMoneyManager { get; set; }

    [ClientRpc]
    public void SetMoney(float money)
    {
        this.Money = money;
    }

    [ClientRpc]
    public void SetMoneyPerSecond(float moneyPerSecond)
    {
        this.MoneyPerSecond = moneyPerSecond;
    }

    protected override void StartOverridable()
    {
        base.StartOverridable();
        this.Weapon = this.GetComponentInChildren<RaycastWeapon>();
        this.CharacterController.enabled = false;
        this.SpawnIndex = (FindObjectsOfType<ZombiePlayer>().Length - 1) % NetworkManager.startPositions.Count;
        Transform spawnPoint = NetworkManager.startPositions[this.SpawnIndex];

        this.transform.position = spawnPoint.position;
        this.transform.rotation = spawnPoint.rotation;
        this.CharacterController.enabled = true;
    }

    protected override void Initialise()
    {
        base.Initialise();
        this.ZombiePlayerMoneyManager = new ZombiePlayerMoneyManager(this);
        this.hasFinishInitialisation = true;
    }

    public void StartGame()
    {
        this.StartCoroutine(this.ZombiePlayerMoneyManager.EarnMoney());
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
        if (Input.GetButtonDown("Fire1") && this.Weapon != null && this.Weapon.CanShoot())
        {
            this.Weapon.Shoot(this);
        }
    }
}
