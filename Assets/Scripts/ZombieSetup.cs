using System.Collections;
using System.Collections.Generic;

using Mirror;

using UnityEngine;

public class ZombieSetup : MonoBehaviour
{
    public ZombieGameManager ZombieGameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ZombiePlayer zombiePlayer = other.gameObject.GetComponent<ZombiePlayer>();
            uint netId = other.gameObject.GetComponent<NetworkIdentity>().netId;

            if (this.ZombieGameManager.FirstPlayer == null)
            {
                this.ZombieGameManager.FirstPlayer = other.gameObject;
            }
            if (this.ZombieGameManager.SecondPlayer == null && netId != this.ZombieGameManager.FirstPlayer.GetComponent<NetworkIdentity>().netId)
            {
                this.ZombieGameManager.SecondPlayer = other.gameObject;
            }
        }
    }
}
