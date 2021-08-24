using Mirror;

using System.Collections;

using UnityEngine;

public class GunShootRaycast : NetworkBehaviour
{
    public float damage = 10f;

    public Camera fpsCam;

    public GameObject hitMarker;

    public float hitMarkerDelay = 0.1f;

    public ParticleSystem muzzleFlash;

    public float range = 100f;

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

    [Command]
    private void Shoot()
    {
        this.ShootRpc();
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
        if (MainMenu.isOn == true)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
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
    }
}