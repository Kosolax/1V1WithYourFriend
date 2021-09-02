using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeaponAmmoManager : MonoBehaviour
{
    public RaycastWeapon RaycastWeapon { get; set; }

    public RaycastWeaponAmmoManager(RaycastWeapon raycastWeapon)
    {
        this.RaycastWeapon = raycastWeapon;
    }

    private void Start()
    {
        this.SetAmmo(this.RaycastWeapon.MaxAmmo);
    }

    public void SetAmmo(float ammo)
    {
        this.RaycastWeapon.CurrentAmmo = ammo;
        if (this.RaycastWeapon.CurrentAmmo < 0)
        {
            this.RaycastWeapon.CurrentAmmo = 0;
        }

        this.RaycastWeapon.AmmoText.text = $"{this.RaycastWeapon.CurrentAmmo}/{this.RaycastWeapon.MaxAmmo}";
    }

    public void UseAmmo()
    {
        this.SetAmmo(this.RaycastWeapon.CurrentAmmo - this.RaycastWeapon.AmmoUsedPerShot);
        if (this.RaycastWeapon.CurrentAmmo == 0)
        {
            this.StartCoroutine(this.Reload());
        }
    }

    public IEnumerator Reload()
    {
        this.RaycastWeapon.IsReloading = true;
        // TODO start design
        yield return new WaitForSeconds(this.RaycastWeapon.ReloadTime);
        this.SetAmmo(this.RaycastWeapon.MaxAmmo);
        this.RaycastWeapon.IsReloading = false;
    }
}
