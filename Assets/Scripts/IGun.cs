using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    void SetTimer(float value);

    float GetTimer();

    float GetFireRate();

    float GetAmmoCount();

    float GetReloadTime();

    void SetAmmoCount(float value);

    bool IsAutomatic();

    void Reload();

    void Shoot(Camera fpsCam, bool isLocalPlayer);

    //Animator later and more ?
}