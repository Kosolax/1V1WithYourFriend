using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSetup : MonoBehaviour
{
    public GameObject dude;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            dude = other.gameObject;
        }
    }
}
