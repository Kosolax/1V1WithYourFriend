using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    [SerializeField]
    private float damage = 40f;

    [SerializeField]
    private float range = 100f;

    [SerializeField]
    private float fireRate = 1f;

    [SerializeField]
    private float magazineSize = 5;

    [SerializeField]
    private float reloadTime = 10f;

    [SerializeField]
    private GunShootRaycast GunShootRaycast;

    private void Start()
    {
        this.GunShootRaycast.SetDamage(this.damage);
        this.GunShootRaycast.SetRange(this.range);
        this.GunShootRaycast.SetFireRate(this.fireRate);
        this.GunShootRaycast.SetMagazineSize(this.magazineSize);
        this.GunShootRaycast.SetReloadTime(this.reloadTime);
    }
}
