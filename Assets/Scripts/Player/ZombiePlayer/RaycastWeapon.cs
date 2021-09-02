using System.Collections;

using Mirror;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class RaycastWeapon : MonoBehaviour, IWeapon
{
    public float MaxAmmo;
    public float CurrentAmmo;
    public float AmmoUsedPerShot;
    public float Range;

    public float ReloadTime;
    public bool IsReloading;
    public RaycastWeaponAmmoManager RaycastWeaponAmmoManager;

    public TextMeshProUGUI AmmoText;

    public Camera Camera;

    public float DelayShoot;
    public bool HasFinishedShooting;

    public float Damage;

    public bool CanShoot()
    {
        return this.HasFinishedShooting && !this.IsReloading;
    }

    private void Start()
    {
        this.HasFinishedShooting = true;
        this.RaycastWeaponAmmoManager.RaycastWeapon = this;
    }

    public void Shoot(BasePlayer basePlayer)
    {
        this.RaycastWeaponAmmoManager.UseAmmo();
        if (Physics.Raycast(this.Camera.transform.position, this.Camera.transform.forward, out RaycastHit hit, this.Range))
        {
            Zombie target = hit.transform.GetComponent<Zombie>();
            if (target != null)
            {
                this.Touched(target);
            }
            else if (hit.transform.gameObject.tag == "ZombieSpawner")
            {
                ZombieSpawner zombieSpawner = hit.transform.GetComponent<ZombieSpawner>();
                if (zombieSpawner != null)
                {
                    zombieSpawner.SpawnZombie();
                }
            }
        }

        this.StartCoroutine(this.DelayShooting());
    }

    public void Touched(Zombie target)
    {
        target.TakeDamage(this.Damage);
    }

    public IEnumerator DelayShooting()
    {
        this.HasFinishedShooting = false;
        yield return new WaitForSeconds(this.DelayShoot);
        this.HasFinishedShooting = true;
    }

    
}
