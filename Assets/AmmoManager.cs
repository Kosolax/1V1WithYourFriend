using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;
using UnityEngine.UI;

public class AmmoManager : NetworkBehaviour
{
    public WeaponManager weaponManager;

    public Text ammoText;

    public bool isReloading;

    public float timer;


    // Start is called before the first frame update
    void Start()
    {
        this.isReloading = false;
        this.UpdateAmmoCount();
    }

    public void UpdateAmmoCount()
    {
        this.ammoText.text = this.weaponManager.IGun.GetAmmoCount().ToString();
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (isReloading == true)
        {
            this.timer -= Time.deltaTime;
            if (this.timer <= 0f)
            {
                this.weaponManager.IGun.Reload();
                this.isReloading = false;
            }
        }

        if (this.weaponManager.IGun.GetAmmoCount() <= 0 && this.isReloading == false)
        {
            this.isReloading = true;
            this.ammoText.text = "Reloading...";
            this.timer = this.weaponManager.IGun.GetReloadTime();
        } 
        else if (this.isReloading == false)
        {
            this.UpdateAmmoCount();
        }
    }

    public void CancelReload()
    {
        this.isReloading = false;
        this.timer = 0f;
    }
}
