using System.Collections;

using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GunBaseRaycastBeam : MonoBehaviour, IGun
{
    public float beamDuration;

    public float damage;

    public float fireRate;

    public float currentAmmo;

    public float magazineSize;

    [SerializeField]
    private float range;

    public float reloadTime;

    [SerializeField]
    private ParticleSystem muzzleFlashParticle;

    [SerializeField]
    private GameObject shootEmmiter;

    public bool AutomaticMode;

    [SerializeField]
    private LineRenderer lr;

    private Camera fpsCam;

    private float timer;

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
        this.currentAmmo = magazineSize;
    }

    public float GetReloadTime()
    {
        return this.reloadTime;
    }

    public float GetAmmoCount()
    {
        return this.currentAmmo;
    }

    public void SetAmmoCount(float value)
    {
        this.currentAmmo = value;
    }

    public void SetTimer(float value)
    {
        this.timer = value;
    }

    public float GetTimer()
    {
        return this.timer;
    }

    public bool IsAutomatic()
    {
        return AutomaticMode;
    }

    public void Shoot(Camera fpsCam, bool isLocalPlayer)
    {
        this.fpsCam = fpsCam;
        this.currentAmmo -= 1;
        RaycastHit hit;
        //Here the only way i found to make layermask properly working is to use bits, so here im saying at the end, layer 7 is the only thing the raycast cannot hit (CurrentPlayer for now)
        if (Physics.Raycast(this.fpsCam.transform.position, this.fpsCam.transform.forward, out hit))
        {
            Player target = hit.transform.parent.gameObject.GetComponent<Player>();
            if (target != null)
            {
                target.ITookDamage(this.damage);
            }
            Vector3[] lrPoints = { this.shootEmmiter.transform.position, hit.point };
            this.ShootBeam(lrPoints);
        }
        else
        {
            Ray ray = new Ray(this.fpsCam.transform.position, this.fpsCam.transform.forward);
            Vector3[] lrPoints = { this.shootEmmiter.transform.position, ray.GetPoint(this.range) };
            this.ShootBeam(lrPoints);
        }
        this.timer = this.fireRate;
    }

    private void ShootBeam(Vector3[] lrPoints)
    {
        StartCoroutine(ShootBeamCoroutine(lrPoints));
    }

    public float GetFireRate()
    {
        return this.fireRate;
    }

    private IEnumerator ShootBeamCoroutine(Vector3[] lrPoints)
    {
        lr.positionCount = 2;
        this.muzzleFlashParticle.Play();
        lr.SetPositions(lrPoints);
        yield return new WaitForSeconds(this.beamDuration);
        lr.positionCount = 0;
        Vector3[] emptyList = { };
        lr.SetPositions(emptyList);
    }
}