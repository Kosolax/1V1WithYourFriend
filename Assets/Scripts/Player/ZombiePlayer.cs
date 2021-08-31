using System;
using System.Collections;
using System.Collections.Generic;

using Mirror;

using UnityEngine;

public class ZombiePlayer : BasePlayer
{
    private bool hasFinishInitialisation;
    private PlayerMovementManager playerMovementManager;
    private PlayerLookManager playerLookManager;

    private void Start()
    {
        this.Initialise();
    }

    private void Initialise()
    {
        this.hasFinishInitialisation = false;
        Cursor.lockState = CursorLockMode.Locked;
        this.playerMovementManager = new PlayerMovementManager(this.CharacterController, this.GroundCheck, this.GroundMask, this, this.PlayerTransform);
        this.playerLookManager = new PlayerLookManager(this, this.XRotationTransform, this.YRotationTransform);
        this.InitialisePlayer();
        this.hasFinishInitialisation = true;
    }

    private void InitialisePlayer()
    {
        if (!this.isLocalPlayer)
        {
            for (int i = 0; i < this.ComponentsToDisable.Length; i++)
            {
                this.ComponentsToDisable[i].enabled = false;
            }

            for (int i = 0; i < GameObjectToDisable.Length; i++)
            {
                GameObjectToDisable[i].SetActive(false);
            }
        }
        else
        {
            //disable main camera on player join
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        // We don't want our action to be applied on others players
        if (!this.isLocalPlayer || !this.hasFinishInitialisation)
        {
            return;
        }

        // NOTE : Here we put everything that need to RUN even if we are in a menu
        this.playerMovementManager.ApplyGravity();

        if (MainMenu.isOn)
        {
            return;
        }

        // NOTE : Here we put everything that need to STOP when we are in a menu
        this.playerMovementManager.MoveOrAndJump();
        this.playerLookManager.RotateCamera();
    }
}
