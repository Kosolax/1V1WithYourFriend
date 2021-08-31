using Mirror;

using UnityEngine;

public class BasePlayer : NetworkBehaviour
{
    [Header("Player Statistics")]
    [SyncVar] public float Gravity = -19.62f;
    [SyncVar] public float JumpHeight = 3f;
    [SyncVar] public float MaxHealth = 100;
    [SyncVar] public float Speed = 20f;
    public float Health = 100;
    public float MouseSensitivity = 400f;

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

    protected Camera sceneCamera;
    protected bool hasFinishInitialisation;
    private PlayerMovementManager playerMovementManager;
    private PlayerLookManager playerLookManager;

    [ClientRpc]
    public void SetJumpHeight(float jumpHeight)
    {
        this.JumpHeight = jumpHeight;
    }

    [ClientRpc]
    public void SetMaxHealth(float maxHealth)
    {
        this.MaxHealth = maxHealth;
    }

    [ClientRpc]
    public void SetGravity(float gravity)
    {
        this.Gravity = gravity;
    }

    [ClientRpc]
    public void SetSpeed(float speed)
    {
        this.Speed = speed;
    }

    private void Start()
    {
        this.Initialise();
    }

    protected virtual void Initialise()
    {
        this.hasFinishInitialisation = false;
        Cursor.lockState = CursorLockMode.Locked;
        this.playerMovementManager = new PlayerMovementManager(this.CharacterController, this.GroundCheck, this.GroundMask, this, this.PlayerTransform);
        this.playerLookManager = new PlayerLookManager(this, this.XRotationTransform, this.YRotationTransform);
        this.InitialisePlayer();
    }

    protected virtual void InitialisePlayer()
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

    private void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
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
