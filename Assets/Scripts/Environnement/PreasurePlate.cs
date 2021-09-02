using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreasurePlate : MonoBehaviour
{
    public bool IsPressed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this.IsPressed = true;
            this.gameObject.transform.localScale = new Vector3(1, 0.1f, 1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        this.IsPressed = false;
        this.gameObject.transform.localScale = new Vector3(1, 0.3f, 1);
    }
}
