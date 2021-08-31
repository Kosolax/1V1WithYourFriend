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
            NetworkServer.Spawn(gameObject);
            NetworkServer.AddPlayerForConnection(conn, gameObject);
            BasePlayer player = gameObject.GetComponent<BasePlayer>();
            player.SetGravity(this.LobbyManager.Gravity);
            player.SetJumpHeight(this.LobbyManager.JumpHeight);
            player.SetMaxHealth(this.LobbyManager.MaxHealth);
            player.SetSpeed(this.LobbyManager.Speed);
        }
    }
}