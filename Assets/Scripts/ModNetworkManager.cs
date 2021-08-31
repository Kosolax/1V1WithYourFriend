using Mirror;

using UnityEngine;

public class ModNetworkManager : NetworkManager
{
    public GameObject fightModPlayerPrefab;

    public LobbyManager LobbyManager;

    public GameObject zombieModPlayerPrefab;

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        PlayerMessage playerMessage = new PlayerMessage()
        {
        };

        conn.Send(playerMessage);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<PlayerMessage>(this.OnCreateCharacter);
    }

    private void OnCreateCharacter(NetworkConnection conn, PlayerMessage message)
    {
        GameObject gameObject = null;
        switch (this.LobbyManager.PlayerType)
        {
            case PlayerType.Fight:
                gameObject = Instantiate(fightModPlayerPrefab);
                break;
            case PlayerType.Zombie:
                gameObject = Instantiate(zombieModPlayerPrefab);
                break;
        }

        if (gameObject != null)
        {
            Player player = gameObject.GetComponent<Player>();
            player.Gravity = this.LobbyManager.Gravity;
            player.JumpHeight = this.LobbyManager.JumpHeight;
            player.MaxHealth = this.LobbyManager.MaxHealth;
            player.Speed = this.LobbyManager.Speed;
            NetworkServer.Spawn(gameObject);
            NetworkServer.AddPlayerForConnection(conn, gameObject);
        }
    }
}