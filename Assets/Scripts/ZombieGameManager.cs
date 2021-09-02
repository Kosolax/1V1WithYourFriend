using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGameManager : MonoBehaviour
{
    public PreasurePlate PreasurePlate1;
    public PreasurePlate PreasurePlate2;
    public static bool IsStarted;
    public ZombiePlayer ZombiePlayer1;
    public ZombiePlayer ZombiePlayer2;

    public static Dictionary<string, Zombie> Zombies = new Dictionary<string, Zombie>();

    public static void AddZombie(uint netId, Zombie zombie)
    {
        string zombieName = $"Zombie {netId}";
        Zombies.Add(zombieName, zombie);

        zombie.gameObject.transform.name = zombieName;
    }

    public static void RemoveZombie(uint netId)
    {
        Zombies.Remove($"Zombie {netId}");
    }

    private void Update()
    {
        if (!IsStarted && this.PreasurePlate1.IsPressed && this.PreasurePlate2.IsPressed)
        {
            IsStarted = true;
            this.PreasurePlate1.gameObject.SetActive(false);
            this.PreasurePlate2.gameObject.SetActive(false);
            this.ZombiePlayer1.StartGame();
            this.ZombiePlayer2.StartGame();
        } 
    }
}
