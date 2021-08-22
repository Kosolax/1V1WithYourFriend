using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            other.GetComponent<Player>().Die();
        }
    }
}
