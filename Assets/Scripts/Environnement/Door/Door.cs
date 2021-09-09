using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Door : MonoBehaviour
{
    public GameObject CurrentDoor;

    public abstract void Open();
}
