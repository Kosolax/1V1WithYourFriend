using Mirror;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShootRaycast : NetworkBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject hitMarker;

    // Update is called once per frame
    void Update()
    {
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

    IEnumerator HitMarker()
    {
        if (hitMarker.activeInHierarchy)
        {
            hitMarker.SetActive(false);
        }

        hitMarker.SetActive(true);

        yield return new WaitForSeconds(0.2f);

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


    [Command]
    public void Touched(Player target)
    {
        this.TouchedRpc(target);
    }

    [ClientRpc]
    private void TouchedRpc(Player target)
    {
        target.ITookDamage(damage);
        StartCoroutine(HitMarker());
    }
}
