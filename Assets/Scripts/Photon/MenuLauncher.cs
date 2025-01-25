using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private string waitingRoomSceneName = "WaitingRoomScene"; // Nom de la sc�ne d'attente

    public static bool useVR = true; // Variable statique pour acc�der � useVR partout

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Se connecter � Photon Cloud
        PhotonNetwork.AutomaticallySyncScene = true; // Synchroniser les sc�nes automatiquement
    }

    public void UseVRValueChanged(bool newValue)
    {
        useVR = newValue;
        Debug.Log($"VR option set to: {useVR}");
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

        // Charger la sc�ne d'attente
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(waitingRoomSceneName);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning("JoinRandom failed, creating a new room.");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 3 }); // Cr�er une room si aucune room existante
    }
}
