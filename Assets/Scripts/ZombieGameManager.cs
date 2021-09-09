using System.Collections.Generic;

using Mirror;

using UnityEngine;

public class ZombieGameManager : NetworkBehaviour
{
    public static bool IsGameStarted;

    public PreasurePlate PreasurePlate1;

    public PreasurePlate PreasurePlate2;

    public WaveManager WaveManager;

    private GameObject firstPlayer;

    private GameObject secondPlayer;

    public GameObject FirstPlayer
    {
        get => this.firstPlayer;
        set
        {
            this.WaveManager.FirstPlayer = value;
            this.firstPlayer = value;
        }
    }

    public GameObject OurMiniMap;
    public GameObject EnemyMiniMap;

    public GameObject SecondPlayer
    {
        get => this.secondPlayer;
        set
        {
            this.WaveManager.SecondPlayer = value;
            this.secondPlayer = value;
        }
    }

    public List<GameObject> ZombiesPlayer1 { get; set; }

    public List<GameObject> ZombiesPlayer2;

    private void Start()
    {
        this.ZombiesPlayer1 = new List<GameObject>();
        this.ZombiesPlayer2 = new List<GameObject>();
        if (this.isServer)
        {
            this.OurMiniMap.transform.position = new Vector3(0, 99, 0);
            this.EnemyMiniMap.transform.position = new Vector3(100, 99, 0);
        }
        else
        {
            this.OurMiniMap.transform.position = new Vector3(100, 99, 0);
            this.EnemyMiniMap.transform.position = new Vector3(0, 99, 0);
        }
    }

    private void Update()
    {
        if (!IsGameStarted && this.PreasurePlate1.IsPressed)
        {
            IsGameStarted = true;
            this.PreasurePlate1.gameObject.SetActive(false);
            this.PreasurePlate2.gameObject.SetActive(false);
            this.WaveManager.StartSpawningWaves();
        }
    }
}