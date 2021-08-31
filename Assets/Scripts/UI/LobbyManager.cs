using Mirror;

using TMPro;

using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;

    public float Gravity;

    public TMP_InputField GravityText;

    public TMP_InputField IpInputField;

    public float JumpHeight;

    public TMP_InputField JumpHeightText;

    public float MaxHealth;

    public TMP_InputField MaxHealthText;

    public NetworkManager NetworkManager;

    public GameObject NextButton;

    public PlayerType PlayerType;

    public string SceneName;

    public float Speed;

    public TMP_InputField SpeedText;

    public void Host()
    {
        this.NetworkManager.onlineScene = this.SceneName;
        this.NetworkManager.StartHost();
    }

    public void Join()
    {
        this.NetworkManager.networkAddress = this.IpInputField.text;
        this.NetworkManager.StartClient();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetGravity(string gravity)
    {
        if (float.TryParse(gravity, out float floatGravity))
        {
            this.Gravity = floatGravity;
        }
        else
        {
            this.Gravity = 0;
        }

        this.GravityText.text = this.Gravity.ToString();
    }

    public void SetJumpHeight(string jumpHeight)
    {
        if (float.TryParse(jumpHeight, out float floatJumpHeight))
        {
            this.JumpHeight = floatJumpHeight;
        }
        else
        {
            this.JumpHeight = 0;
        }

        this.JumpHeightText.text = this.JumpHeight.ToString();
    }

    public void SetMap(string sceneName)
    {
        this.SceneName = sceneName;
        this.NextButton.SetActive(true);
    }

    public void SetMaxHealth(string maxHealth)
    {
        if (float.TryParse(maxHealth, out float floatMaxHealth))
        {
            this.MaxHealth = floatMaxHealth;
        }
        else
        {
            this.MaxHealth = 0;
        }

        this.MaxHealthText.text = this.MaxHealth.ToString();
    }

    public void SetSpeed(string speed)
    {
        if (float.TryParse(speed, out float floatSpeed))
        {
            this.Speed = floatSpeed;
        }
        else
        {
            this.Speed = 0;
        }

        this.SpeedText.text = this.Speed.ToString();
    }

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (Instance == null)
        {
            Instance = this;
            this.IpInputField.text = "localhost";
        }
        else
        {
            Destroy(this);
        }
    }
}