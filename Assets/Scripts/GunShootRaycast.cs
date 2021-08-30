using Mirror;

using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class GunShootRaycast : NetworkBehaviour
{
    [SyncVar]
    public float ammo = 0f;

    public Text AmmoCounter;

    public float beamDuration = 1f;

    public float damage = 10f;

    public float fireRate = 0f;

    public Camera fpsCam;

    public GameObject hitMarker;

    public float hitMarkerDelay = 0.1f;

    public float magazineSize = 0f;

    public ParticleSystem muzzleFlash;

    public float range = 100f;

    public float reloadTime = 4f;

    [SyncVar]
    private bool isReloading = false;

    public LineRenderer lr;

    public GameObject shootEmmiter;

    [SyncVar]
    private float timer;

    [Command]
    public void Reload()
    {
        this.timer = this.reloadTime;
        StartCoroutine(ReloadCoroutine(this.reloadTime));
    }

    [Command]
    public void Touched(Player target)
    {
        this.TouchedRpc(target);
    }

    public void enableScripts()
    {
        this.enabled = true;
        this.lr.enabled = true;
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
        this.AmmoCounter.text = "Reloading...";
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

    private IEnumerator ShootBeam(Vector3[] lrPoints)
    {
        lr.positionCount = 2;
        lr.SetPositions(lrPoints);
        yield return new WaitForSeconds(this.beamDuration);
        lr.positionCount = 0;
        Vector3[] emptyList = { };
        lr.SetPositions(emptyList);
    }

    [Command]
    private void ShootBeamCommand(Vector3[] lrPoints)
    {
        this.ShootBeamRPC(lrPoints);
    }

    [ClientRpc]
    private void ShootBeamRPC(Vector3[] lrPoints)
    {
        StartCoroutine(ShootBeam(lrPoints));
    }

    private void ShootLocal()
    {
        this.Shoot();
        RaycastHit hit;
        if (Physics.Raycast(this.fpsCam.transform.position, this.fpsCam.transform.forward, out hit, this.range))
        {
            Player target = hit.transform.GetComponent<Player>();
            if (target != null)
            {
                Touched(target);
            }
            Vector3[] lrPoints = { this.shootEmmiter.transform.position, hit.point };
            ShootBeamCommand(lrPoints);
        }
        else
        {
            Ray ray = new Ray(this.fpsCam.transform.position, this.fpsCam.transform.forward);
            Vector3[] lrPoints = { this.shootEmmiter.transform.position, ray.GetPoint(this.range) };
            ShootBeamCommand(lrPoints);
        }
    }

    [ClientRpc]
    private void ShootRpc()
    {
        this.muzzleFlash.Play();
    }

    private void Start()
    {
        this.lr = this.GetComponent<LineRenderer>();
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

        if (Input.GetKeyDown(KeyCode.R) && this.ammo != this.magazineSize)
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
            this.Reload();
        }
        else
        {
            this.AmmoCounter.text = this.ammo.ToString();
        }
    }
}