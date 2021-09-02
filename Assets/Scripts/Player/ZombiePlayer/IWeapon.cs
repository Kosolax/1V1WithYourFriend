using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void Shoot(BasePlayer basePlayer);

    public bool CanShoot();
}
