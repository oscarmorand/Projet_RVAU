using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class JumpBreakLidarGun : MonoBehaviourPun
{
    private HashSet<int> playersWhoJumped = new HashSet<int>();

    GameObject lastPlayer;

    public GameObject brokenCameraUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PhotonNetwork.IsConnected)
        {
            lastPlayer = other.gameObject;
            PhotonView playerPhotonView = other.GetComponent<PhotonView>();

            photonView.RPC("RPC_PlayerJumped", RpcTarget.MasterClient, playerPhotonView.OwnerActorNr);
        }
    }

    [PunRPC]
    void RPC_PlayerJumped(int playerId)
    {
        if (!playersWhoJumped.Contains(playerId))
        {
            playersWhoJumped.Add(playerId);
            Debug.Log($"Player {playerId} jumped.");

            // Vérifie si tous les joueurs ont sauté
            if (playersWhoJumped.Count == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                photonView.RPC("RPC_BreakLidarGun", PhotonNetwork.CurrentRoom.GetPlayer(playerId), playerId);
            }
        }
    }

    [PunRPC]
    void RPC_BreakLidarGun(int playerId)
    {
        Debug.Log("Your lidar gun is broken!");

        brokenCameraUI.SetActive(true);
    }
}
