using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Mirror;

using UnityEngine;

public class ZombieSpawner : Malus
{
    public List<GameObject> ItemToSpawn;

    public override void RevertMalus()
    {
    }

    public override void SendMalus()
    {
        if (this.isServer)
        {
            this.SpawnZombie();
        }
    }

    [Command(requiresAuthority = false)]
    public void SpawnZombie()
    {
        if (this.Interact.PlayerThatPaid == this.WaveManager.FirstPlayer)
        {
            List<GameObject> spawnPoints = this.WaveManager.SecondPlayerSpawnPoints;
            GameObject playerToFollow = this.WaveManager.SecondPlayer;
            foreach (GameObject item in this.ItemToSpawn)
            {
                this.WaveManager.InstantiateZombie(item, spawnPoints, playerToFollow);
            }
        }
        else if (this.Interact.PlayerThatPaid == this.WaveManager.SecondPlayer)
        {
            List<GameObject> spawnPoints = this.WaveManager.FirstPlayerSpawnPoints;
            GameObject playerToFollow = this.WaveManager.FirstPlayer;
            foreach (GameObject item in this.ItemToSpawn)
            {
                this.WaveManager.InstantiateZombie(item, spawnPoints, playerToFollow);
            }
        }
    }
}