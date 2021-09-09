using System.Collections;
using System.Collections.Generic;

using Mirror;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Interact))]
public abstract class Malus : NetworkBehaviour
{
    public Interact Interact;

    public WaveManager WaveManager;

    public Image Image;

    private void Update()
    {
        if (this.Interact.CanInteract())
        {
            this.AddMalus(this.Interact.PlayerInTrigger.gameObject);
        }
    }

    public abstract void SendMalus();

    public abstract void RevertMalus();

    [Command(requiresAuthority = false)]
    public void AddMalus(GameObject player)
    {
        this.AddMalusRpc(player);
    }

    [ClientRpc]
    public void AddMalusRpc(GameObject player)
    {
        this.Interact.PlayerThatPaid = player;
        if (this.Interact.PlayerThatPaid == this.WaveManager.FirstPlayer)
        {
            Image image = Instantiate(Image);
            image.transform.SetParent(this.WaveManager.SecondPlayer.GetComponent<ZombiePlayer>().Malus.transform);
            this.WaveManager.ListMalusPlayer2.Add(this);
        }
        else if (this.Interact.PlayerThatPaid == this.WaveManager.SecondPlayer)
        {
            Image image = Instantiate(Image);
            image.transform.SetParent(this.WaveManager.FirstPlayer.GetComponent<ZombiePlayer>().Malus.transform);
            this.WaveManager.ListMalusPlayer1.Add(this);
        }
    }
}
