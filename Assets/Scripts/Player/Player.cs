using System;
using System.Collections.Generic;

using Mirror;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    [Header("Settings to match player options, modEnum and maps")]
    [Tooltip("We use playerSetup index to match it with ModeEnum so be careful. NOTE THAT INDEX 0 MUST STAY EMPTY")]
    public List<PlayerSetup> PlayerSetups;
    public List<string> SceneNames;
    public List<ModEnum> ModEnumsToMapSceneNames;

    [Header("Player Statistics")]
    public float Gravity = -19.62f;
    public float Health = 100;
    public float JumpHeight = 3f;
    public float MaxHealth = 100;
    public float MouseSensitivity = 400f;
    public float Speed = 20f;

    [Header("Player Rotation Settings")]
    public Transform XRotationTransform;
    public Transform YRotationTransform;

    [Header("Player Movement Settings")]
    public CharacterController CharacterController;
    public Transform GroundCheck;
    public LayerMask GroundMask;
    public Transform PlayerTransform;

    [Header("Player SetUp Settings")]
    public GameObject[] GameObjectToDisable;
    public Behaviour[] ComponentsToDisable;

    [Header("Player Health Settings")]
    public Text PlayerLifeText;


    private bool hasFinishInitialisation;
    private PlayerLookManager playerLookManager;
    private GunShootRaycast gunShootRaycast;
    private PlayerMovementManager playerMovementManager;
    private Camera sceneCamera;

    public bool IsDead { get; set; }

    public void Die()
    {
        this.IsDead = true;
        this.CharacterController.enabled = false;

        this.Respawn();
    }

    [Command]
    public void ITookDamage(float damage)
    {
        this.UpdateHpForOthers(damage);
    }

    public override void OnStartClient()
    {
        Scene scene = SceneManager.GetActiveScene();
        var index = this.SceneNames.IndexOf(scene.name);
        if (index >= 0)
        {
            PlayerSetting.ModEnum = this.ModEnumsToMapSceneNames[index];
        }
        base.OnStartClient();
        uint netId = this.GetComponent<NetworkIdentity>().netId;
        GameManager.AddPlayer(netId, this);
    }

    private void Initialise()
    {
        this.hasFinishInitialisation = false;
        Cursor.lockState = CursorLockMode.Locked;
        this.PreInitialisePlayerBasedOnMod();
        this.playerMovementManager = new PlayerMovementManager(this.CharacterController, this.GroundCheck, this.GroundMask, this, this.PlayerTransform);
        this.playerLookManager = new PlayerLookManager(this, this.XRotationTransform, this.YRotationTransform);
        this.gunShootRaycast = new GunShootRaycast();
        this.InitialisePlayer();
        this.hasFinishInitialisation = true;
    }

    private void PreInitialisePlayerBasedOnMod()
    {
        if (PlayerSetting.ModEnum != ModEnum.Default)
        {
            PlayerSetup modPlayer = this.PlayerSetups[(int) PlayerSetting.ModEnum];
            this.Gravity = modPlayer.Gravity;
            this.Health = modPlayer.Health;
            this.JumpHeight = modPlayer.JumpHeight;
            this.MaxHealth = modPlayer.MaxHealth;
            this.MouseSensitivity = modPlayer.MouseSensitivity;
            this.Speed = modPlayer.Speed;
            this.XRotationTransform = modPlayer.XRotationTransform;
            this.YRotationTransform = modPlayer.YRotationTransform;
            this.CharacterController = modPlayer.CharacterController;
            this.GroundCheck = modPlayer.GroundCheck;
            this.GroundMask = modPlayer.GroundMask;
            this.PlayerTransform = modPlayer.PlayerTransform;
            this.GameObjectToDisable = modPlayer.GameObjectToDisable;
            this.ComponentsToDisable = modPlayer.ComponentsToDisable;
            this.PlayerLifeText = modPlayer.PlayerLifeText;
            this.GetComponent<NetworkTransformChild>().target = modPlayer.Camera.transform;

            for (int i = 0; i < this.PlayerSetups.Count; i++)
            {
                if (i != 0)
                {
                    this.PlayerSetups[i].gameObject.SetActive(i == (int) PlayerSetting.ModEnum);
                }
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
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

        this.Reset();
    }

    private void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }

    private void Reset()
    {
        this.IsDead = false;
        this.Health = this.MaxHealth;
        this.PlayerLifeText.text = this.Health.ToString();
        this.CharacterController.enabled = true;
    }

    private void Respawn()
    {
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        this.transform.position = spawnPoint.position;
        this.transform.rotation = spawnPoint.rotation;

        this.Reset();
    }

    private void SetHealth(float damage)
    {
        if (this.IsDead)
        {
            return;
        }

        this.Health -= damage;
        this.PlayerLifeText.text = this.Health.ToString();
        if (this.Health <= 0)
        {
            this.Die();
        }
    }

    private void Start()
    {
        this.Initialise();
    }

    private void Update()
    {
        // We don't want our action to be applied on others players
        if (!this.isLocalPlayer || !this.hasFinishInitialisation || this.IsDead)
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

    [ClientRpc]
    private void UpdateHpForOthers(float damage)
    {
        GameManager.Players[this.name].SetHealth(damage);
    }
}