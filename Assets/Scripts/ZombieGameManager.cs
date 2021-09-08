using System.Collections.Generic;

using UnityEngine;

public class ZombieGameManager : MonoBehaviour
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
    }

    private void Update()
    {
        if (!IsGameStarted && this.PreasurePlate1.IsPressed && this.PreasurePlate2.IsPressed)
        {
            IsGameStarted = true;
            this.PreasurePlate1.gameObject.SetActive(false);
            this.PreasurePlate2.gameObject.SetActive(false);
            this.WaveManager.StartSpawningWaves();
        }
    }
}