using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Transform RemoteCanon;

    public GameObject BulletPrefab;

    public Player Player;

    public Transform CameraTransform;

    public int BulletSpeed;

    public void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            this.Shoot();
        }
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab, RemoteCanon.position, Quaternion.identity);
        bullet.transform.rotation = RemoteCanon.transform.rotation;

        bullet.GetComponentInChildren<Rigidbody>().velocity = this.transform.TransformDirection(bullet.transform.forward * BulletSpeed);
    }
}
