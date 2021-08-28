using UnityEngine;

public class Map : MonoBehaviour
{
    public LobbyManager LobbyManager;

    public string SceneName;

    public void SetMap()
    {
        this.LobbyManager.SetMap(this.SceneName);
    }

    public void SetUp(string sceneName, LobbyManager lobbyManager)
    {
        this.SceneName = sceneName;
        this.LobbyManager = lobbyManager;
    }
}