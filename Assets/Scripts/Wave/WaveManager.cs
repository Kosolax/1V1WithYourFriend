using System.Collections.Generic;
using System.Linq;

using Mirror;

using UnityEngine;

public class WaveManager : NetworkBehaviour
{
    [SyncVar]
    public int CurrentIndexInItemsToSpawn;

    [SyncVar]
    public float CurrentTimeBetweenLastSpawning;

    [SyncVar]
    public float CurrentTimeWaveIsRunning;

    [SyncVar]
    public int CurrentWave;

    public GameObject FirstPlayer;

    public List<GameObject> FirstPlayerSpawnPoints;

    public bool isSpawning;

    [SyncVar]
    public List<Malus> ListMalusPlayer1;

    [SyncVar]
    public List<Malus> ListMalusPlayer2;

    public Dictionary<int, List<Malus>> ListMalusPlayer1ToRevert; 

    public Dictionary<int, List<Malus>> ListMalusPlayer2ToRevert;

    public GameObject SecondPlayer;

    public List<GameObject> SecondPlayerSpawnPoints;

    public List<Wave> Waves;

    public List<KeyValuePair<ZombieStats, float>> ZombiePlayer1Malus;
    public List<KeyValuePair<ZombieStats, float>> ZombiePlayer2Malus;

    [Command(requiresAuthority = false)]
    public void SpawnItem()
    {
        Wave wave = this.Waves[this.CurrentWave];
        if (this.CurrentIndexInItemsToSpawn < wave.ItemsToSpawn.Count)
        {
            this.InstantiateZombie(wave.ItemsToSpawn[this.CurrentIndexInItemsToSpawn], this.SecondPlayerSpawnPoints, this.SecondPlayer);
            this.InstantiateZombie(wave.ItemsToSpawn[this.CurrentIndexInItemsToSpawn], this.FirstPlayerSpawnPoints, this.FirstPlayer);
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
                if (this.ListMalusPlayer1ToRevert.ContainsKey(this.CurrentWave - 1))
                {
                    foreach (Malus malus in this.ListMalusPlayer1ToRevert[this.CurrentWave - 1])
                    {
                        malus.RevertMalus();
                    }

                    this.ListMalusPlayer1ToRevert.Remove(this.CurrentWave - 1);
                }

                if (this.ListMalusPlayer2ToRevert.ContainsKey(this.CurrentWave - 1))
                {
                    foreach (Malus malus in this.ListMalusPlayer2ToRevert[this.CurrentWave - 1])
                    {
                        malus.RevertMalus();
                    }

                    this.ListMalusPlayer2ToRevert.Remove(this.CurrentWave - 1);
                }

                if (this.isServer)
                {
                    Wave wave = this.Waves[this.CurrentWave];
                    if (this.CurrentTimeWaveIsRunning < wave.TimeBeforeConsideringWaveIsFinished)
                    {
                        if (this.CurrentTimeBetweenLastSpawning >= wave.DelayBetweenItemSpawn)
                        {
                            if (this.isServer)
                            {
                                this.SpawnItem();
                            }
                            this.CurrentIndexInItemsToSpawn++;
                            this.CurrentTimeBetweenLastSpawning = 0;
                        }


                        this.CurrentTimeBetweenLastSpawning += Time.deltaTime;
                        this.CurrentTimeWaveIsRunning += Time.deltaTime;
                    }
                    else
                    {
                        this.ClearMalus();
                        this.CurrentIndexInItemsToSpawn = 0;
                        this.CurrentWave++;
                        this.CurrentTimeWaveIsRunning = 0f;
                        this.CurrentTimeBetweenLastSpawning = 0;

                        this.Toto(this.CurrentWave);
                    }
                }
            }
            else
            {
                this.isSpawning = false;
            }
        }
    }

    [ClientRpc]
    public void ClearMalus()
    {
        this.ZombiePlayer1Malus.Clear();
        this.ZombiePlayer2Malus.Clear();
    }

    [ClientRpc]
    public void Toto(int currentWave)
    {
        foreach (Malus malus in this.ListMalusPlayer1)
        {
            malus.SendMalus();
            Debug.Log($"currentWaveToRevert1 {currentWave}");
            if (this.ListMalusPlayer1ToRevert.ContainsKey(currentWave))
            {
                this.ListMalusPlayer1ToRevert[currentWave].Add(malus);
            }
            else
            {
                this.ListMalusPlayer1ToRevert.Add(currentWave, new List<Malus>() { malus });
            }
        }

        foreach (Malus malus in this.ListMalusPlayer2)
        {
            malus.SendMalus();
            Debug.Log($"currentWaveToRevert2 {currentWave}");
            if (this.ListMalusPlayer2ToRevert.ContainsKey(currentWave))
            {
                this.ListMalusPlayer2ToRevert[currentWave].Add(malus);
            }
            else
            {
                this.ListMalusPlayer2ToRevert.Add(currentWave, new List<Malus>() { malus });
            }
        }

        this.ListMalusPlayer1.Clear();
        this.ListMalusPlayer2.Clear();
        foreach (Transform child in this.FirstPlayer.GetComponent<ZombiePlayer>().Malus.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in this.SecondPlayer.GetComponent<ZombiePlayer>().Malus.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void Start()
    {
        this.ZombiePlayer1Malus = new List<KeyValuePair<ZombieStats, float>>();
        this.ZombiePlayer2Malus = new List<KeyValuePair<ZombieStats, float>>();
        this.ListMalusPlayer1 = new List<Malus>();
        this.ListMalusPlayer2 = new List<Malus>();
        this.ListMalusPlayer1ToRevert = new Dictionary<int, List<Malus>>();
        this.ListMalusPlayer2ToRevert = new Dictionary<int, List<Malus>>();
        this.isSpawning = false;
        this.CurrentWave = 0;
        this.CurrentIndexInItemsToSpawn = 0;
        this.CurrentTimeBetweenLastSpawning = 0;
        this.CurrentTimeWaveIsRunning = 0;
    }
}