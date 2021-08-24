using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public InputField sensitivityInputField;

    public Slider sensitivitySlider;

    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private PlayerLook playerLook;

    [SerializeField]
    private GameObject[] UIItemsSwitch;

    public void InputFieldUpdate(string text)
    {
        sensitivitySlider.value = float.Parse(text);
    }

    public void SensitivitySliderUpdate(float value)
    {
        playerLook.SetSensitivity(value);
        sensitivityInputField.text = playerLook.MouseSensitivity.ToString();
    }

    public void ToggleMainMenu()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        MainMenu.isOn = mainMenu.activeSelf;

        //Set the cursor free from FPS lockmode
        Cursor.lockState = CursorLockMode.None;

        if (mainMenu.activeSelf == true)
        {
            //Disable all HUD items who can obstruct the menu
            for (int i = 0; i < UIItemsSwitch.Length; i++)
            {
                UIItemsSwitch[i].SetActive(false);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;

            //Activate all HUD items that were disable
            for (int i = 0; i < UIItemsSwitch.Length; i++)
            {
                UIItemsSwitch[i].SetActive(true);
            }

        }
    }

    private void Start()
    {
        MainMenu.isOn = false;
        this.sensitivitySlider.value = playerLook.MouseSensitivity;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMainMenu();
        }
    }
}