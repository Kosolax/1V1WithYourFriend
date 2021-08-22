using System;
using System.Collections;
using System.Collections.Generic;

using Mirror;

using UnityEngine;

public class Player : NetworkBehaviour
{
    public float Health;

    [Command]
    public void ITookDamage(float damage)
    {
        this.UpdateHpForOthers(damage);
    }

    [ClientRpc]
    private void UpdateHpForOthers(float damage)
    {
        GameManager.Players[this.name].SetHealth(damage);
    }

    private void SetHealth(float damage)
    {
        this.Health -= damage;
        if (this.Health <= 0)
        {
            Debug.Log("you died bitch");
        }
    }
}