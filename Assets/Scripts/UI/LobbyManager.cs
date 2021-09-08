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

    public float Money;

    public TMP_InputField MoneyText;

    public GameObject FightPanel;

    public GameObject ZombiePanel;

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

    public void SelectGoodPanel()
    {
        switch (this.PlayerType)
        {
            case PlayerType.Fight:
                this.FightPanel.SetActive(true);
                this.ZombiePanel.SetActive(false);
                break;
            case PlayerType.Zombie:
                this.FightPanel.SetActive(false);
                this.ZombiePanel.SetActive(true);
                break;
        }
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

    public void SetMoney(string money)
    {
        if (float.TryParse(money, out float floatMoney))
        {
            this.Money = floatMoney;
        }
        else
        {
            this.Money = 0;
        }

        this.MoneyText.text = this.Money.ToString();
    }

    public void Start()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
            this.IpInputField.text = "localhost";
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }
}