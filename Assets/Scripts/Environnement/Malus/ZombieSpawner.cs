using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Mirror;

using UnityEngine;

public class ZombieSpawner : Malus
{
    public List<GameObject> ItemToSpawn;

    public override void SendMalus()
    {
        Debug.Log(isServer);
        if (isServer)
        {
            this.Toto();
        }
    }

    [Command(requiresAuthority = false)]
    public void Toto()
    {
        foreach (GameObject item in this.ItemToSpawn)
        {
            List<GameObject> spawnPoints = this.WaveManager.SecondPlayerSpawnPoints;
            GameObject playerToFollow = this.WaveManager.SecondPlayer;
            this.WaveManager.InstantiateZombie(item, spawnPoints, playerToFollow);
        }
    }
}
