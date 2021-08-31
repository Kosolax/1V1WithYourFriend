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
}
