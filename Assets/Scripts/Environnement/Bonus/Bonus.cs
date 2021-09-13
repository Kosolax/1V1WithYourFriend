
using Mirror;

using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Bonus : NetworkBehaviour
{
    public abstract void SendBonus(GameObject player);

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                this.SendBonus(other.gameObject);
            }
        }

        if (this.isServer)
        {
            NetworkServer.UnSpawn(this.gameObject);
        }

        Destroy(this.gameObject);
    }
}