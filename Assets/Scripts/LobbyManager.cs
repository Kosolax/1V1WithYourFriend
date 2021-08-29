using Mirror;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public GameObject Container;

    public GameObject DoneButton;

    public TMP_InputField IpInputField;

    public NetworkManager NetworkManager;

    public PlayerSetting PlayerSetting;

    public string SceneName;

    public InputField SensitivityInputField;

    public Slider SensitivitySlider;

    public void Host()
    {
        if (this.SceneName != default)
        {
            this.NetworkManager.onlineScene = this.SceneName;
            this.NetworkManager.StartHost();
        }
    }

    public void InputFieldUpdate(string text)
    {
        this.SensitivitySlider.value = float.Parse(text);
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

    public void ResetHost()
    {
        this.SceneName = string.Empty;
        this.DoneButton.SetActive(false);
        foreach (Transform child in this.Container.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void SensitivitySliderUpdate(float value)
    {
        PlayerSetting.MouseSensitivity = value;
        this.SensitivityInputField.text = PlayerSetting.MouseSensitivity.ToString();
    }

    public void SetMap(string sceneName)
    {
        this.SceneName = sceneName;
        this.DoneButton.SetActive(true);
    }

    private void Start()
    {
        this.SensitivitySliderUpdate(400);
        this.IpInputField.text = "localhost";
        this.SceneName = default;
        Cursor.lockState = CursorLockMode.None;
    }
}