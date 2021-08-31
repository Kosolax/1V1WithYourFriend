using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class Mod : MonoBehaviour
{
    public GameObject ButtonMapPrefab;

    public GameObject Container;

    public LobbyManager LobbyManager;

    public List<string> SceneNames;

    public void DisplayMaps()
    {
        foreach (Transform child in this.Container.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (string sceneName in this.SceneNames)
        {
            GameObject button = Instantiate(this.ButtonMapPrefab, this.Container.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = sceneName;
            button.GetComponent<Map>().SetUp(sceneName, LobbyManager);
        }
    }
}