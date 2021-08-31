using UnityEngine;

public class PlayerMovementManager
{
    private float groundDistance = 0.4f;

    private bool isGrounded;

    private Vector3 velocity;

    public PlayerMovementManager(CharacterController characterController, Transform groundCheck, LayerMask groundMask, BasePlayer player, Transform playerTransform)
    {
        this.CharacterController = characterController;
        this.GroundCheck = groundCheck;
        this.GroundMask = groundMask;
        this.Player = player;
        this.PlayerTransform = playerTransform;
    }

    public CharacterController CharacterController { get; set; }

    public Transform GroundCheck { get; set; }

    public LayerMask GroundMask { get; set; }

    public BasePlayer Player { get; set; }

    public Transform PlayerTransform { get; set; }

    public void ApplyGravity()
    {
        this.isGrounded = Physics.CheckSphere(this.GroundCheck.position, this.groundDistance, this.GroundMask);

        // When we touch the ground we make sure to reset velocity for the next jump
        // Since we can detect to have touch the ground before we really touched the ground we still let a little velocity to keep going down
        if (this.isGrounded && this.velocity.y < 0)
        {
            this.velocity.y = -2f;
        }

        // Gravity
        this.velocity.y += this.Player.Gravity * Time.deltaTime;
        this.CharacterController.Move(this.velocity * Time.deltaTime);
    }

    public void MoveOrAndJump()
    {
        // Move locally
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = this.PlayerTransform.right * x + this.PlayerTransform.forward * z;
        this.CharacterController.Move(move * this.Player.Speed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && this.isGrounded)
        {
            this.velocity.y = Mathf.Sqrt(this.Player.JumpHeight * -2f * this.Player.Gravity);
        }
    }
}