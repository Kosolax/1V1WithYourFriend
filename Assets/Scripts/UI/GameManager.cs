using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public static Dictionary<string, Player> Players = new Dictionary<string, Player>();

    public static void AddPlayer(uint netId, Player player)
    {
        string playerName = $"Player {netId}";
        Players.Add(playerName, player);

        player.gameObject.transform.name = playerName;
    }

    public static void RemovePlayer(int netId)
    {
        Players.Remove($"Player {netId}");
    }
}
