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
            GameObject instantiatedZombie = Instantiate(ZombiePrefab);
            var i = Random.Range(0, this.ZombieSpawns.Count);
            this.ZombiePlayer.ZombiePlayerMoneyManager.LoseMoney(this.Cost);
            NetworkServer.Spawn(instantiatedZombie);
            instantiatedZombie.transform.position = this.ZombieSpawns[i].transform.position;
            Debug.Log(instantiatedZombie.transform.position);
            instantiatedZombie.GetComponent<Zombie>().ZombieSetup = this.ZombieSetup;
        }
    }

    public void SpawnZombieCommand()
    {

    }
}
