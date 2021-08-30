using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetup : MonoBehaviour
{
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

    [Header("Player Camera")]
    public Camera Camera;
}
