using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

public class PhotonPlayerManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private List<Player> playersInRoom = new List<Player>();

    void Start()
    {
        // Si déjà dans la room au démarrage, ajouter les joueurs existants
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddPlayer(player);
        }
    }

    public override void OnJoinedRoom()
    {
        // Ajouter le joueur local (MasterClient ou non) à la liste
        AddPlayer(PhotonNetwork.LocalPlayer);
        Debug.Log($"You joined the room. Total players: {playersInRoom.Count}");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayer(newPlayer);
        Debug.Log($"Player {newPlayer.NickName} joined the room. Total players: {playersInRoom.Count}");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemovePlayer(otherPlayer);
        Debug.Log($"Player {otherPlayer.NickName} left the room. Total players: {playersInRoom.Count}");
    }

    private void AddPlayer(Player player)
    {
        if (!playersInRoom.Contains(player))
        {
            playersInRoom.Add(player);
        }
    }

    private void RemovePlayer(Player player)
    {
        if (playersInRoom.Contains(player))
        {
            playersInRoom.Remove(player);
        }
    }
}
