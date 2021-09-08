using System.Collections;

using UnityEngine;

public class ZombiePlayerMoneyManager
{
    public ZombiePlayerMoneyManager(ZombiePlayer zombiePlayer)
    {
        this.ZombiePlayer = zombiePlayer;
    }

    public ZombiePlayer ZombiePlayer { get; set; }

    public void LoseMoney(float amount)
    {
        if (this.ZombiePlayer.Money - amount >= 0)
        {
            this.ZombiePlayer.Money -= amount;
            this.ZombiePlayer.MoneyText.text = this.ZombiePlayer.Money.ToString();
        }
    }

    public void AddMoney(float amount)
    {
        this.ZombiePlayer.Money += amount;
        this.ZombiePlayer.MoneyText.text = this.ZombiePlayer.Money.ToString();
    }
}