using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI playerCountText; // Texte pour afficher le nombre de joueurs
    [SerializeField] private int maxPlayers = 3; // Nombre maximum de joueurs dans la room
    [SerializeField] private string gameSceneName = "SampleScene"; // Nom de la scène de jeu

    void Start()
    {
        // Mettre à jour le texte avec le nombre actuel de joueurs
        UpdatePlayerCountText();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} joined the room. Total players: {PhotonNetwork.CurrentRoom.PlayerCount}");
        UpdatePlayerCountText();

        // Si la room est pleine, démarrer le jeu
        if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayers)
        {
            StartGame();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} left the room. Total players: {PhotonNetwork.CurrentRoom.PlayerCount}");
        UpdatePlayerCountText();
    }

    private void UpdatePlayerCountText()
    {
        // Mettre à jour le texte avec le nombre actuel de joueurs
        playerCountText.text = $"Players: {PhotonNetwork.CurrentRoom.PlayerCount}/{maxPlayers}";
    }

    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Charger la scène de jeu pour tous les joueurs
            PhotonNetwork.LoadLevel(gameSceneName);
        }
    }
}
