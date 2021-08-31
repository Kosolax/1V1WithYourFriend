using UnityEngine;

public class GunBaseRaycast : MonoBehaviour
{
    [SerializeField]
    private float beamDuration = 0.1f;

    [SerializeField]
    private float damage = 40f;

    [SerializeField]
    private float fireRate = 1f;

    [SerializeField]
    private GunShootRaycast GunShootRaycast;

    [SerializeField]
    private float magazineSize = 5;

    [SerializeField]
    private float range = 100f;

    [SerializeField]
    private float reloadTime = 10f;

    [SerializeField]
    private ParticleSystem muzzleFlashParticle;

    [SerializeField]
    private GameObject shootEmmiter;

    public void UpdateWeaponStats()
    {
        this.GunShootRaycast.enableScripts();
        this.GunShootRaycast.damage = this.damage;
        this.GunShootRaycast.range = this.range;
        this.GunShootRaycast.fireRate = this.fireRate;
        this.GunShootRaycast.magazineSize = this.magazineSize;
        this.GunShootRaycast.reloadTime = this.reloadTime;
        this.GunShootRaycast.beamDuration = this.beamDuration;
        this.GunShootRaycast.muzzleFlash = this.muzzleFlashParticle;
        this.GunShootRaycast.shootEmmiter = this.shootEmmiter;
    }

    private void Start()
    {
        this.UpdateWeaponStats();
    }
}