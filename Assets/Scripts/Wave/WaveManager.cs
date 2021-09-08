using System.Collections.Generic;
using System.Linq;

using Mirror;

using UnityEngine;

public class WaveManager : NetworkBehaviour
{
    public int currentIndexInItemsToSpawn;

    public float currentTimeBetweenLastSpawning;

    public float currentTimeWaveIsRunning;

    public int CurrentWave;

    public GameObject FirstPlayer;

    public List<GameObject> FirstPlayerSpawnPoints;

    public bool isSpawning;

    [SyncVar]
    public List<Malus> ListMalusPlayer1;

    [SyncVar]
    public List<Malus> ListMalusPlayer2;

    public GameObject SecondPlayer;

    public List<GameObject> SecondPlayerSpawnPoints;

    public List<Wave> Waves;

    public List<KeyValuePair<ZombieStats, float>> ZombiePlayer1Malus;
    public List<KeyValuePair<ZombieStats, float>> ZombiePlayer2Malus;
    
    [Command(requiresAuthority = false)]
    public void SpawnItem()
    {
        Wave wave = this.Waves[this.CurrentWave];
        if (this.currentIndexInItemsToSpawn < wave.ItemsToSpawn.Count)
        {
            this.InstantiateZombie(wave.ItemsToSpawn[this.currentIndexInItemsToSpawn], this.SecondPlayerSpawnPoints, this.SecondPlayer);
            this.InstantiateZombie(wave.ItemsToSpawn[this.currentIndexInItemsToSpawn], this.FirstPlayerSpawnPoints, this.FirstPlayer);
        }
    }

    public void InstantiateZombie(GameObject prefabToSpawn, List<GameObject> spawnPoints, GameObject playerToFollow)
    { 
        var i = Random.Range(0, spawnPoints.Count());
        GameObject itemInstantiated = Instantiate(prefabToSpawn, spawnPoints[i].transform.position, Quaternion.identity);
        itemInstantiated.GetComponent<Zombie>().PlayerToFollow = playerToFollow;

        if (playerToFollow == this.FirstPlayer)
        {
            this.SetZombieStats(itemInstantiated, this.ZombiePlayer1Malus);
        }
        else if (playerToFollow == this.SecondPlayer)
        {
            this.SetZombieStats(itemInstantiated, this.ZombiePlayer2Malus);
        }

        NetworkServer.Spawn(itemInstantiated);
    }

    private GameObject SetZombieStats(GameObject itemInstantiated, List<KeyValuePair<ZombieStats, float>> zombiePlayerMalus)
    {
        Zombie zombie = itemInstantiated.GetComponent<Zombie>();
        foreach (KeyValuePair<ZombieStats, float> zombieMalus in zombiePlayerMalus)
        {
            switch (zombieMalus.Key)
            {
                case ZombieStats.Damage:
                    zombie.Damage *= zombieMalus.Value;
                    break;
                case ZombieStats.Speed:
                    zombie.Speed *= zombieMalus.Value;
                    break;
                case ZombieStats.MaxHealth:
                    zombie.MaxHealth *= zombieMalus.Value;
                    break;
                case ZombieStats.MoneyGain:
                    zombie.MoneyGain *= zombieMalus.Value;
                    break;
            }
        }

        return zombie.gameObject;
    }

    public void StartSpawningWaves()
    {
        this.isSpawning = true;
    }

    public void Update()
    {
        if (this.isSpawning)
        {
            if (this.CurrentWave < this.Waves.Count)
            {
                Wave wave = this.Waves[this.CurrentWave];
                if (wave != null)
                {
                    if (this.currentTimeWaveIsRunning < wave.TimeBeforeConsideringWaveIsFinished)
                    {
                        if (this.currentTimeBetweenLastSpawning >= wave.DelayBetweenItemSpawn)
                        {
                            if (isServer)
                            {
                                this.SpawnItem();
                            }
                            this.currentIndexInItemsToSpawn++;
                            this.currentTimeBetweenLastSpawning = 0;
                        }


                        this.currentTimeBetweenLastSpawning += Time.deltaTime;
                        this.currentTimeWaveIsRunning += Time.deltaTime;
                    }
                    else
                    {
                        this.ZombiePlayer1Malus = new List<KeyValuePair<ZombieStats, float>>();
                        this.ZombiePlayer2Malus = new List<KeyValuePair<ZombieStats, float>>();
                        this.currentIndexInItemsToSpawn = 0;
                        this.CurrentWave++;
                        this.currentTimeWaveIsRunning = 0f;
                        this.currentTimeBetweenLastSpawning = 0;

                        foreach (Malus malus in this.ListMalusPlayer1)
                        {
                            malus.SendMalus();
                        }

                        foreach (Malus malus in this.ListMalusPlayer2)
                        {
                            malus.SendMalus();
                        }

                        this.RemoveMalus();
                    }
                }
                else
                {
                    this.ZombiePlayer1Malus = new List<KeyValuePair<ZombieStats, float>>();
                    this.ZombiePlayer2Malus = new List<KeyValuePair<ZombieStats, float>>();
                    this.currentIndexInItemsToSpawn = 0;
                    this.currentTimeWaveIsRunning = 0f;
                    this.currentTimeBetweenLastSpawning = 0;
                    this.CurrentWave++;
                }
            }
            else
            {
                this.isSpawning = false;
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void RemoveMalus()
    {
        this.RemoveMalusRpc();
    }

    [ClientRpc]
    public void RemoveMalusRpc()
    {
        this.ListMalusPlayer1.Clear();
        this.ListMalusPlayer2.Clear();
    }

    private void Start()
    {
        this.ZombiePlayer1Malus = new List<KeyValuePair<ZombieStats, float>>();
        this.ZombiePlayer2Malus = new List<KeyValuePair<ZombieStats, float>>();
        this.ListMalusPlayer1 = new List<Malus>();
        this.ListMalusPlayer2 = new List<Malus>();
        this.isSpawning = false;
        this.CurrentWave = 0;
        this.currentIndexInItemsToSpawn = 0;
        this.currentTimeBetweenLastSpawning = 0;
        this.currentTimeWaveIsRunning = 0;
    }
}