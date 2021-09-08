using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Mirror;

using UnityEngine;

public class ChangeZombieStat : Malus
{
    public ZombieGameManager ZombieGameManager;

    public ZombieStats ZombieStats;

    public List<GameObject> ZombiesPrefab;

    public float Multiplicator;

    public override void SendMalus()
    {
        if (this.PlayerThatPaidMalus == this.WaveManager.FirstPlayer)
        {
            this.WaveManager.ZombiePlayer2Malus.Add(new KeyValuePair<ZombieStats, float>(this.ZombieStats, this.Multiplicator));
        }
        else if (this.PlayerThatPaidMalus == this.WaveManager.SecondPlayer)
        {
            this.WaveManager.ZombiePlayer1Malus.Add(new KeyValuePair<ZombieStats, float>(this.ZombieStats, this.Multiplicator));
        }
    }
}

public enum ZombieStats
{
    Damage = 0,
    Speed = 1,
    MaxHealth = 2,
    MoneyGain = 3,
}