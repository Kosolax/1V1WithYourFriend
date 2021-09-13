using System.Collections.Generic;

using Mirror;

using UnityEngine;
using UnityEngine.AI;

public class Zombie : NetworkBehaviour
{
    public float Acceleration = 2000;

    public NavMeshAgent Agent;

    public float AngularSpeed = 100000;

    public float AttackRange = 1.5f;

    public SphereCollider ColliderToAttack;

    public float Damage = 1;

    public float DelayDamage = 0.5f;

    public float Health = 100;

    public bool IsNear;

    public float MaxHealth = 100;

    public float MoneyGain = 1f;

    public GameObject PlayerToFollow;

    public List<GameObject> BonusToDrops;

    [Range(0, 100)]
    public int DropRate;

    [SyncVar]
    public float Speed = 20f;

    [Command(requiresAuthority = false)]
    public void SetSpeed()
    {
        this.SetSpeedRpc();
    }

    [ClientRpc]
    public void SetSpeedRpc()
    {
        this.Speed *= 1.5f;
        this.Agent.speed = this.Speed;
    }

    private float currentDelayDamage;

    public void TakeDamage(float damage)
    {
        int i = -1;
        int localDropRate = Random.Range(0, 101);
        if (localDropRate <= this.DropRate)
        {
            i = Random.Range(0, this.BonusToDrops.Count);
        }

        this.TakeDamageCmd(damage, i);
    }

    [Command(requiresAuthority = false)]
    private void TakeDamageCmd(float damage, int i)
    {
        this.UpdateHpForOthers(damage, i);
    }
    
    private void Attack(Collider other)
    {
        if (other.tag == "Player")
        {
            this.IsNear = true;
            ZombiePlayer zombiePlayer = other.GetComponent<ZombiePlayer>();
            zombiePlayer.TakeDamage(this.Damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        this.Attack(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            this.IsNear = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        this.currentDelayDamage += Time.deltaTime;
        if (this.currentDelayDamage >= this.DelayDamage)
        {
            if (this.IsNear && other.tag == "Player")
            {
                ZombiePlayer zombiePlayer = other.GetComponent<ZombiePlayer>();
                zombiePlayer.TakeDamage(this.Damage);
            }

            this.currentDelayDamage = 0f;
        }
    }

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
        if (this.PlayerToFollow != null)
        {
            this.Agent.SetDestination(this.PlayerToFollow.transform.position);
        }
    }

    [ClientRpc]
    private void UpdateHpForOthers(float damage, int i)
    {
        this.Health -= damage;
        if (this.Health <= 0)
        {
            if (i > -1 && this.isServer)
            {
                GameObject bonus = this.BonusToDrops[i];
                GameObject instantiatedBonus = Instantiate(bonus);
                instantiatedBonus.transform.position = this.transform.position;
                NetworkServer.Spawn(instantiatedBonus);
            }

            NetworkServer.UnSpawn(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}