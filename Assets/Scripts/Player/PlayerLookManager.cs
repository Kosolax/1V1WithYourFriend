using UnityEngine;

public class PlayerLookManager
{
    private float xRotation = 0f;

    public PlayerLookManager(Player player, Transform xRotationTransform, Transform yRotationTransform)
    {
        this.Player = player;
        this.XRotationTransform = xRotationTransform;
        this.YRotationTransform = yRotationTransform;
    }

    public Player Player { get; set; }

    public Transform XRotationTransform { get; set; }

    public Transform YRotationTransform { get; set; }

    public void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * this.Player.MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * this.Player.MouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        // don't allow people to rotate verticall to much
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        this.YRotationTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        this.XRotationTransform.Rotate(Vector3.up * mouseX);
    }
}