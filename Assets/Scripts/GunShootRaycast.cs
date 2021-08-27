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

    public float beamDuration = 1f;

    public ParticleSystem muzzleFlash;

    private float damage = 10f;

    private float fireRate = 0f;

    [SyncVar]
    private bool isReloading = false;

    private LineRenderer lr;

    [SerializeField]
    private GameObject shootEmmiter;

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

    [ClientRpc]
    private void ShootRpc()
    {
        this.muzzleFlash.Play();
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

    private void Start()
    {
        this.lr = this.GetComponent<LineRenderer>();
    }
}