using System.Collections.Generic;
using System.Linq;

using Mirror;

using UnityEngine;

public class WaveManager : NetworkBehaviour
{
    public int CurrentWave;

    public GameObject FirstPlayer;

    public List<GameObject> FirstPlayerSpawnPoints;

    public GameObject SecondPlayer;

    public List<GameObject> SecondPlayerSpawnPoints;

    public List<Wave> Waves;

    public int currentIndexInItemsToSpawn;

    public float currentTimeBetweenLastSpawning;

    public float currentTimeWaveIsRunning;

    public bool isSpawning;

    [Command(requiresAuthority = false)]
    public void SpawnItem()
    {
        Wave wave = this.Waves[this.CurrentWave];
        if (this.currentIndexInItemsToSpawn < wave.ItemsToSpawn.Count)
        {
            var i = Random.Range(0, this.FirstPlayerSpawnPoints.Count());
            GameObject itemInstantiated = Instantiate(wave.ItemsToSpawn[this.currentIndexInItemsToSpawn], FirstPlayerSpawnPoints[i].transform.position, Quaternion.identity);
            itemInstantiated.GetComponent<Zombie>().PlayerToFollow = FirstPlayer;
            NetworkServer.Spawn(itemInstantiated);
            i = Random.Range(0, this.SecondPlayerSpawnPoints.Count());
            itemInstantiated = Instantiate(wave.ItemsToSpawn[this.currentIndexInItemsToSpawn], SecondPlayerSpawnPoints[i].transform.position, Quaternion.identity);
            itemInstantiated.GetComponent<Zombie>().PlayerToFollow = SecondPlayer;
            NetworkServer.Spawn(itemInstantiated);
        }
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
                if (this.currentTimeWaveIsRunning < wave.TimeBeforeConsideringWaveIsFinished)
                {
                    if (this.currentTimeBetweenLastSpawning >= wave.DelayBetweenItemSpawn)
                    {
                        this.SpawnItem();
                        this.currentIndexInItemsToSpawn++;
                        this.currentTimeBetweenLastSpawning = 0;
                    }

                    this.currentTimeBetweenLastSpawning += Time.deltaTime;
                    this.currentTimeWaveIsRunning += Time.deltaTime;
                }
                else
                {
                    this.currentIndexInItemsToSpawn = 0;
                    this.CurrentWave++;
                    this.currentTimeWaveIsRunning = 0f;
                }
            }
            else
            {
                this.isSpawning = false;
            }
        }
    }

    private void Start()
    {
        this.isSpawning = false;
        this.CurrentWave = 0;
        this.currentIndexInItemsToSpawn = 0;
        this.currentTimeBetweenLastSpawning = 0;
        this.currentTimeWaveIsRunning = 0;
    }
}