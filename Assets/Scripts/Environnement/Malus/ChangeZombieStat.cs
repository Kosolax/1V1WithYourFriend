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

    public override void RevertMalus()
    {
    }

    public override void SendMalus()
    {
        if (this.Interact.PlayerThatPaid == this.WaveManager.FirstPlayer)
        {
            this.WaveManager.ZombiePlayer2Malus.Add(new KeyValuePair<ZombieStats, float>(this.ZombieStats, this.Multiplicator));
        }
        else if (this.Interact.PlayerThatPaid == this.WaveManager.SecondPlayer)
        {
            this.WaveManager.ZombiePlayer1Malus.Add(new KeyValuePair<ZombieStats, float>(this.ZombieStats, this.Multiplicator));
        }
    }
}