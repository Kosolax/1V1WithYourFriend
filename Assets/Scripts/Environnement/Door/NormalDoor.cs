using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDoor : Door
{
    public override void Open()
    {
        this.CurrentDoor.SetActive(false);
    }
}
