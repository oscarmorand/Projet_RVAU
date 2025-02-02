using UnityEngine;
using Photon.Realtime;
using System.Collections.Generic;
using Photon.Pun;

public class EndGame : MonoBehaviourPun
{
    private HashSet<int> playersWhoEntered = new HashSet<int>();

    public ChasePhase lastChasePhase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && lastChasePhase.state == ChasePhase.ChaseState.Ended)
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonView playerPhotonView = other.GetComponent<PhotonView>();

                photonView.RPC("RPC_PlayerEnteredEndGame", RpcTarget.MasterClient, playerPhotonView.OwnerActorNr);
            }
            else
            {
                End();
            }
        }
    }

    [PunRPC]
    void RPC_PlayerEnteredEndGame(int playerId)
    {
        if (!playersWhoEntered.Contains(playerId))
        {
            playersWhoEntered.Add(playerId);
            Debug.Log($"Player {playerId} entered end game.");

            if (playersWhoEntered.Count == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                Debug.Log("Game ended");

                if (PhotonNetwork.IsMasterClient)
                {
                    End();
                }
            }
        }
    }

    void End()
    {
        PhotonNetwork.LoadLevel("MenuScene");
    }
}
