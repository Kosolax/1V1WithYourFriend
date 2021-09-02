using System.Collections;

using UnityEngine;

public class ZombiePlayerMoneyManager : MonoBehaviour
{
    public ZombiePlayerMoneyManager(ZombiePlayer zombiePlayer)
    {
        this.ZombiePlayer = zombiePlayer;
    }

    public ZombiePlayer ZombiePlayer { get; set; }

    public IEnumerator EarnMoney()
    {
        while (ZombieGameManager.IsStarted)
        {
            yield return new WaitForSeconds(1);
            this.ZombiePlayer.Money += this.ZombiePlayer.MoneyPerSecond;
        }
    }

    public void LoseMoney(float amountToLose)
    {
        if (this.ZombiePlayer.Money - amountToLose >= 0)
        {
            this.ZombiePlayer.Money -= amountToLose;
        }
    }
}