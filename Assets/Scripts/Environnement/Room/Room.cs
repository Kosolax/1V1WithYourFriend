using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Interact))]
public class Room : MonoBehaviour
{
    public Door Door;

    public Interact Interact;

    public List<GameObject> SpawnPoints;

    public WaveManager WaveManager;

    private void Update()
    {
        if (this.Interact.CanInteract())
        {
            this.Door.Open();
            if (this.WaveManager.FirstPlayer != this.Interact.PlayerThatPaid)
            {
                this.WaveManager.SecondPlayerSpawnPoints.AddRange(this.SpawnPoints);
            }
            else if (this.WaveManager.SecondPlayer != this.Interact.PlayerThatPaid)
            {
                this.WaveManager.FirstPlayerSpawnPoints.AddRange(this.SpawnPoints);
            }
        }
    }
}
