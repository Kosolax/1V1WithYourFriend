using System.Collections;
using System.Collections.Generic;

using Mirror;

using TMPro;

using UnityEngine;
using UnityEngine.AI;

public class Zombie : NetworkBehaviour
{
    public NavMeshAgent Agent;
    public float MaxHealth = 100;
    public float Speed = 20f;
    public float Health = 100;
    public float Acceleration = 2000;
    public float AngularSpeed = 100000;
    public float Damage = 1;
    public float DelayDamage = 0.5f;
    public bool IsNear;
    public SphereCollider ColliderToAttack;
    public float AttackRange = 1.5f;
    public float MoneyGain = 1f;


    private float currentDelayDamage;

    private void Start()
    {
        this.Agent.acceleration = this.Acceleration;
        this.Agent.angularSpeed = this.AngularSpeed;
        this.Agent.speed = this.Speed;
        this.Health = this.MaxHealth;
        this.IsNear = false;
        this.ColliderToAttack.radius = this.AttackRange;
    }

    private void Update()
    {
        //if (this.ZombieSetup != null && this.ZombieSetup.dude != null)
        //{
        //    this.Agent.SetDestination(this.ZombieSetup.dude.transform.position);
        //}
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

    private void OnTriggerEnter(Collider other)
    {
        this.Attack(other);
    }

    private void Attack(Collider other)
    {
        this.Attack(other);
    }

    private void OnTriggerStay(Collider other)
    {
        this.currentDelayDamage += Time.deltaTime;
        if (this.currentDelayDamage >=  this.DelayDamage)
        {
            if (this.IsNear && other.tag == "Player")
            {
                ZombiePlayer zombiePlayer = other.GetComponent<ZombiePlayer>();
                zombiePlayer.TakeDamage(this.Damage);
            }

            this.currentDelayDamage = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            this.IsNear = false;
        }
    }
}
