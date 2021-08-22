using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject BulletPrefab;

    public GameObject RemoteCanon;

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
        GameObject bullet = Instantiate(BulletPrefab, RemoteCanon.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.TransformDirection(this.RemoteCanon.transform.forward * this.BulletSpeed);
    }
}
