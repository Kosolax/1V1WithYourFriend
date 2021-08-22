using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController CharacterController;

    public float Gravity = -9.81f;

    public Transform GroundCheck;

    public float GroundDistance = 0.4f;

    public LayerMask GroundMask;

    public float JumpHeight = 3f;

    public float Speed = 12f;

    private bool isGrounded;

    private Vector3 velocity;

    private void Update()
    {
        this.isGrounded = Physics.CheckSphere(this.GroundCheck.position, this.GroundDistance, this.GroundMask);

        // When we touch the ground we make sure to reset velocity for the next jump
        // Since we can detect to have touch the ground before we really touched the ground we still let a little velocity to keep going down
        if (this.isGrounded && this.velocity.y < 0)
        {
            this.velocity.y = -2f;
        }

        // Move locally
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = this.transform.right * x + this.transform.forward * z;
        this.CharacterController.Move(move * this.Speed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && this.isGrounded)
        {
            this.velocity.y = Mathf.Sqrt(this.JumpHeight * -2f * this.Gravity);
        }

        // Gravity
        this.velocity.y += this.Gravity * Time.deltaTime;
        this.CharacterController.Move(this.velocity * Time.deltaTime);
    }
}