using System.Collections;
using System.Collections.Generic;

using Mirror;

using UnityEngine;
using UnityEngine.AI;

public class Zombie : NetworkBehaviour
{
    public ZombieSetup ZombieSetup;
    public NavMeshAgent Agent;
    public float MaxHealth = 100;
    public float Speed = 20f;
    public float Health = 100;

    private void Start()
    {
        this.Agent.speed = this.Speed;
        this.Health = this.MaxHealth;
    }

    private void Update()
    {
        if (this.ZombieSetup != null && this.ZombieSetup.dude != null)
        {
            this.Agent.SetDestination(this.ZombieSetup.dude.transform.position);
        }
    }

    [Command(requiresAuthority = false)]
    public void TakeDamage(float damage)
    {
        this.UpdateHpForOthers(damage);
    }

    [ClientRpc]
    private void UpdateHpForOthers(float damage)
    {
        this.Health -= damage;
        if (this.Health <= 0)
        {
            NetworkServer.UnSpawn(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
