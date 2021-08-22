using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float MouseSensitivity = 100f;

    public Transform PlayerBody;

    private float xRotation = 0f;

    private void Start()
    {
        // Temp because i hate my mouse to go out like a bitch
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * this.MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * this.MouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        // don't allow people to rotate verticall to much
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        this.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        PlayerBody.Rotate(Vector3.up * mouseX);
    }
}