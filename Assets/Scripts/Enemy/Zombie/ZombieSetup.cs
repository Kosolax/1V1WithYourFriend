using System.Collections;
using System.Collections.Generic;

using Mirror;

using UnityEngine;

public class ZombieSetup : MonoBehaviour
{
    public GameObject dude;

    public ZombieSpawner ZombieSpawner;

    public ZombieGameManager ZombieGameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            dude = other.gameObject;
            ZombiePlayer zombiePlayer = dude.GetComponent<ZombiePlayer>();
            uint netId = dude.GetComponent<NetworkIdentity>().netId;

            this.ZombieSpawner.ZombiePlayer = zombiePlayer;
            if (this.ZombieGameManager.ZombiePlayer1 == null)
            {
                this.ZombieGameManager.ZombiePlayer1 = zombiePlayer;
            }
            if (this.ZombieGameManager.ZombiePlayer2 == null && netId != this.ZombieGameManager.ZombiePlayer1.gameObject.GetComponent<NetworkIdentity>().netId)
            {
                this.ZombieGameManager.ZombiePlayer2 = zombiePlayer;
            }
        }
    }
}
