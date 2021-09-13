using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class WeaponManager : NetworkBehaviour
{
    public GameObject currentWeapon;

    public GameObject hand;

    public IGun IGun;

    public AmmoManager ammoManager;

    public List<GameObject> weaponSlots;

    public Camera fpsCam;

    private void Start()
    {
        /*if (isLocalPlayer)
        {
            this.gameObject.layer = LayerMask.NameToLayer("CurrentPlayer");
            foreach (Transform child in this.transform.gameObject.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("CurrentPlayer");
            }
        }*/
        this.currentWeapon = null;
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            if (weaponSlots[i] != null)
            {
                GameObject newWeapon = Instantiate(weaponSlots[i], this.hand.transform.position, this.hand.transform.rotation, this.hand.transform);
                NetworkServer.Spawn(newWeapon);
                this.weaponSlots[i] = newWeapon;
                newWeapon.SetActive(false);
            }
            if (i == 2)
            {
                this.currentWeapon = this.weaponSlots[i];
                this.currentWeapon.SetActive(true);
                this.IGun = GetIGunFromShootType();
            }
        }
        this.SetCurrentWeapon(2);
    }

    private void Update()
    {
        if (MainMenu.isOn || !this.isLocalPlayer)
        {
            return;
        }


        if (this.IGun.IsAutomatic())
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, IGun.GetFireRate());
            } 
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
        else
        {
            {
                if (Input.GetButtonDown("Fire1") && this.IGun.GetTimer() < 0f)
                {
                    this.Shoot();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && weaponSlots[0] != null)
        {
            SetCurrentWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && weaponSlots[1] != null)
        {
            SetCurrentWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && weaponSlots[2] != null)
        {
            SetCurrentWeapon(2);
        }
    }

    [Command]
    public void SetCurrentWeapon(int weaponIndex)
    {
        this.RpcSetCurrentWeapon(weaponIndex);
    }

    [ClientRpc]
    public void RpcSetCurrentWeapon(int weaponIndex)
    {
        if (currentWeapon != weaponSlots[weaponIndex])
        {
            CancelInvoke("Shoot");
            this.ammoManager.CancelReload();
            this.currentWeapon.SetActive(false);
            this.currentWeapon = weaponSlots[weaponIndex];
            this.currentWeapon.SetActive(true);
            this.IGun = GetIGunFromShootType();
            //this should be put if we want to allow weapon swap shoot (double shootgun for example)
            //this.IGun.setTimer(0f);
        }
    }

    [Command]
    public void Shoot()
    {
        if (this.IGun.GetAmmoCount() > 0)
        {
            this.RpcShoot();
        }
    }

    [ClientRpc]
    public void RpcShoot()
    {
        this.IGun.Shoot(this.fpsCam, isLocalPlayer);
    }

    public IGun GetIGunFromShootType()
    {
        ShootType currentShootType = this.currentWeapon.GetComponent<BaseGun>().shootType;
        switch (currentShootType)
        {
            case ShootType.RaycastBeam:
                return this.currentWeapon.GetComponent<GunBaseRaycastBeam>();
            case ShootType.Raycast:
                return this.currentWeapon.GetComponent<GunBaseRaycast>();
            case ShootType.Projectil:
                return this.currentWeapon.GetComponent<GunBaseRaycast>();
            case ShootType.Melee:
                return this.currentWeapon.GetComponent<GunBaseRaycast>();
        }
        return this.currentWeapon.GetComponent<GunBaseRaycastBeam>();
    }
}
