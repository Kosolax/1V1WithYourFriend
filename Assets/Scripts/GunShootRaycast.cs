using Mirror;

using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class GunShootRaycast : NetworkBehaviour
{
    [SyncVar]
    public float ammo = 0f;

    public Text AmmoCounter;

    public Camera fpsCam;

    public GameObject hitMarker;

    public float hitMarkerDelay = 0.1f;

    public ParticleSystem muzzleFlash;

    private float damage = 10f;

    private float fireRate = 0f;

    [SyncVar]
    private bool isReloading = false;

    private float magazineSize = 0f;

    private float range = 100f;

    private float reloadTime = 4f;

    [SyncVar]
    private float timer;

    [Command]
    public void Reload()
    {
        this.timer = this.reloadTime;
        StartCoroutine(ReloadCoroutine(this.reloadTime));
    }

    public void SetDamage(float value)
    {
        this.damage = value;
    }

    public void SetFireRate(float value)
    {
        this.fireRate = value;
    }

    public void SetMagazineSize(float value)
    {
        this.magazineSize = value;
        this.ammo = this.magazineSize;
        this.UpdateAmmo();
    }

    public void SetRange(float value)
    {
        this.range = value;
    }

    public void SetReloadTime(float value)
    {
        this.reloadTime = value;
    }

    [Command]
    public void Touched(Player target)
    {
        this.TouchedRpc(target);
    }

    private IEnumerator HitMarker()
    {
        if (hitMarker.activeInHierarchy)
        {
            hitMarker.SetActive(false);
        }

        hitMarker.SetActive(true);

        yield return new WaitForSeconds(hitMarkerDelay);

        hitMarker.SetActive(false);
    }

    private IEnumerator ReloadCoroutine(float waitTime)
    {
        this.isReloading = true;
        yield return new WaitForSeconds(waitTime);
        this.ammo = this.magazineSize;
        this.isReloading = false;
    }

    [Command]
    private void Shoot()
    {
        this.ShootRpc();
        this.timer = this.fireRate;
        this.ammo -= 1;
    }

    private void ShootLocal()
    {
        this.Shoot();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Player target = hit.transform.GetComponent<Player>();
            if (target != null)
            {
                Touched(target);
            }
        }
    }

    [ClientRpc]
    private void ShootRpc()
    {
        muzzleFlash.Play();
    }

    [ClientRpc]
    private void TouchedRpc(Player target)
    {
        target.ITookDamage(damage);
        StartCoroutine(HitMarker());
    }

    // Update is called once per frame
    private void Update()
    {
        timer -= Time.deltaTime;
        if (this.isReloading == false)
        {
            this.UpdateAmmo();
        }

        if (MainMenu.isOn == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            this.Reload();
        }


        if (Input.GetButtonDown("Fire1") && timer < 0)
        {
            this.ShootLocal();
        }
    }

    private void UpdateAmmo()
    {
        if (this.ammo == 0 && this.isReloading == false)
        {
            this.AmmoCounter.text = "Reloading...";
            this.Reload();
        }
        else
        {
            this.AmmoCounter.text = this.ammo.ToString();
        }
    }
}