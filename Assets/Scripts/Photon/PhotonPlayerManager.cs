using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

public class PhotonPlayerManager : MonoBehaviourPunCallbacks
{
    public List<Player> playersInRoom = new List<Player>();
    public List<Transform> spawnPoints = new List<Transform>();

    public GameObject playerPrefab;
    public GameObject playerVRPrefab;

    private bool hasSpawnedLocalPlayer = false; // Pour éviter la double instanciation

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            // Ajouter les joueurs existants dans la room
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                AddPlayer(player);
            }

            // Spawner uniquement le joueur local au démarrage
            if (!hasSpawnedLocalPlayer)
            {
                SpawnPlayer(PhotonNetwork.LocalPlayer);
                hasSpawnedLocalPlayer = true;
            }
        }
        else
        {
            SpawnPlayer(null);
        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            AddPlayer(PhotonNetwork.LocalPlayer);
            Debug.Log($"You joined the room. Total players: {playersInRoom.Count}");

            if (!hasSpawnedLocalPlayer)
            {
                SpawnPlayer(PhotonNetwork.LocalPlayer);
                hasSpawnedLocalPlayer = true;
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsConnected)
        {
            AddPlayer(newPlayer);
            Debug.Log($"Player {newPlayer.NickName} joined the room. Total players: {playersInRoom.Count}");
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsConnected)
        {
            RemovePlayer(otherPlayer);
            Debug.Log($"Player {otherPlayer.NickName} left the room. Total players: {playersInRoom.Count}");
        }
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

    private void SpawnPlayer(Player player)
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points available!");
            return;
        }

        int spawnIndex = PhotonNetwork.IsConnected
            ? (player.ActorNumber - 1) % spawnPoints.Count
            : 0; // En offline, on spawn toujours au premier point

        Transform spawnPoint = spawnPoints[spawnIndex];

        if (player == PhotonNetwork.LocalPlayer || !PhotonNetwork.IsConnected)
        {
            GameObject prefabToSpawn = Launcher.useVR ? playerVRPrefab : playerPrefab;
            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation); // Utilisation classique en offline
            Debug.Log($"Player spawned with {(Launcher.useVR ? "VR" : "Standard")} prefab at spawn point {spawnIndex}");
        }
    }
}
