using System.Collections;

using UnityEngine;

public class GunBaseRaycast : MonoBehaviour, IGun
{
    public float damage;

    public float currentAmmo;

    public float fireRate;

    public float magazineSize;

    public float range;

    public float reloadTime;

    [SerializeField]
    private ParticleSystem muzzleFlashParticle;

    [SerializeField]
    private GameObject shootEmmiter;

    private Camera fpsCam;

    [SerializeField]
    private bool AutomaticMode;

    public float timer;

    private void Start()
    {
        this.currentAmmo = this.magazineSize;
        this.timer = 0f;
    }

    private void Update()
    {
        this.timer -= Time.deltaTime;
    }

    public void Reload()
    {
        this.currentAmmo = this.magazineSize;
    }

    public float GetAmmoCount()
    {
        return this.currentAmmo;
    }

    public void SetAmmoCount(float value)
    {
        this.currentAmmo = value;
    }

    public float GetReloadTime()
    {
        return this.reloadTime;
    }

    public bool IsAutomatic()
    {
        return AutomaticMode;
    }

    public void SetTimer(float value)
    {
        this.timer = value;
    }

    public float GetTimer()
    {
        return this.timer;
    }

    public float GetFireRate()
    {
        return this.fireRate;
    }

    public void Shoot(Camera fpsCam, bool isLocalPlayer)
    {
        this.fpsCam = fpsCam;
        this.currentAmmo -= 1;
        RaycastHit hit;
        //Here the only way i found to make layermask properly working is to use bits, so here im saying at the end, layer 7 is the only thing the raycast cannot hit (CurrentPlayer for now)
        //if (Physics.Raycast(this.fpsCam.transform.position, this.fpsCam.transform.forward, out hit, this.range, ~(1 << 7)))
        if (Physics.Raycast(this.fpsCam.transform.position, this.fpsCam.transform.forward, out hit))
        {
            Player target = hit.transform.parent.gameObject.GetComponent<Player>();
            if (target != null)
            {
                target.ITookDamage(this.damage);
            }
        }
    }
}