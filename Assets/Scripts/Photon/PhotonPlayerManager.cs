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

    public bool localUseVR = true;

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
        AddPlayer(PhotonNetwork.LocalPlayer);
        Debug.Log($"You joined the room. Total players: {playersInRoom.Count}");

        if (!hasSpawnedLocalPlayer)
        {
            SpawnPlayer(PhotonNetwork.LocalPlayer);
            hasSpawnedLocalPlayer = true;
        }
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

    private void SpawnPlayer(Player player)
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points available!");
            return;
        }

        int spawnIndex = 0;
        if (player != null)
        {
            spawnIndex = (player.ActorNumber - 1) % spawnPoints.Count;
        }
        Transform spawnPoint = spawnPoints[spawnIndex];


        if (player == null)
        {
            GameObject prefabToSpawn = localUseVR ? playerVRPrefab : playerPrefab;
            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        }
        else if (player == PhotonNetwork.LocalPlayer)
        {
            GameObject prefabToSpawn = Launcher.useVR ? playerVRPrefab : playerPrefab;
            PhotonNetwork.Instantiate(prefabToSpawn.name, spawnPoint.position, spawnPoint.rotation);
        }

        Debug.Log($"Player spawned with {(Launcher.useVR ? "VR" : "Standard")} prefab at spawn point {spawnIndex}");
    }
}