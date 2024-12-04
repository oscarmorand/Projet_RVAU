using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private string gameSceneName = "SampleScene"; // Nom de la scène de jeu

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Se connecter à Photon Cloud
        PhotonNetwork.AutomaticallySyncScene = true; // Synchroniser les scènes automatiquement
    }

    public void OnPlayButtonClicked()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRandomOrCreateRoom(
                roomOptions: new RoomOptions { MaxPlayers = 3 }
            );
        }
        else
        {
            Debug.LogError("Not connected to Master Server yet!");
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}, Players: {PhotonNetwork.CurrentRoom.PlayerCount}");

        // Charger la scène uniquement si le joueur est le premier à entrer dans la room
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(gameSceneName);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning("JoinRandom failed, creating a new room.");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 3 }); // Créer une room si aucune room existante
    }
}
