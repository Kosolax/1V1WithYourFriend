using System.Collections.Generic;

using Mirror;

using UnityEngine;

public class ZombieSpawner : NetworkBehaviour
{
    public ZombieSetup ZombieSetup;

    public ZombiePlayer ZombiePlayer { get; set; }

    public List<GameObject> ZombieSpawns;

    public GameObject ZombiePrefab;

    public float Cost;

    public GameObject ZombiePool;

    [Command(requiresAuthority = false)]
    public void SpawnZombie()
    {
        if (this.ZombiePlayer.Money >= this.Cost)
        {
            var i = Random.Range(0, this.ZombieSpawns.Count);
            GameObject instantiatedZombie = Instantiate(ZombiePrefab, this.ZombieSpawns[i].transform.position, Quaternion.identity);
            this.ZombiePlayer.ZombiePlayerMoneyManager.LoseMoney(this.Cost);
            NetworkServer.Spawn(instantiatedZombie);
            instantiatedZombie.GetComponent<Zombie>().ZombieSetup = this.ZombieSetup;
        }
    }
}