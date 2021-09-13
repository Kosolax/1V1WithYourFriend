using System.Collections;
using System.Collections.Generic;

using Mirror;

using UnityEngine;

public class AddMaxHealthAndHeal : Bonus
{
    public float MaxHealthGain;

    public override void SendBonus(GameObject player)
    {
        ZombiePlayer zombiePlayer = player.GetComponent<ZombiePlayer>();
        zombiePlayer.MaxHealth += MaxHealthGain;
        zombiePlayer.Heal(zombiePlayer.MaxHealth);
    }
}
