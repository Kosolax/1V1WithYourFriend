using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieIA : MonoBehaviour
{
    public ZombieSetup ZombieSetup;
    public NavMeshAgent agent;
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.ZombieSetup.dude != null)
        {
            agent.SetDestination(this.ZombieSetup.dude.transform.position);
        }
    }
}
