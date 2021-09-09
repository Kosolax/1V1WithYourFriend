using System.Collections;
using System.Collections.Generic;

using Mirror;

using UnityEngine;

public class HideMiniMap : Malus
{
    public GameObject MiniMapToHide;

    public override void SendMalus()
    {
        if (!this.Interact.PlayerThatPaid.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            this.MiniMapToHide.SetActive(false);
        }
    }

    public override void RevertMalus()
    {
        if (!this.Interact.PlayerThatPaid.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            this.MiniMapToHide.SetActive(true);
        }
    }
}
