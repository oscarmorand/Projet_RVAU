using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class EndChaseTrigger : MonoBehaviourPun
{
    private HashSet<int> playersWhoEntered = new HashSet<int>();

    public ChasePhase chasePhase;

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
        if (other.gameObject.CompareTag("Player"))
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonView playerPhotonView = other.GetComponent<PhotonView>();

                photonView.RPC("RPC_PlayerEnteredEndZone", RpcTarget.MasterClient, playerPhotonView.OwnerActorNr);
            }
            else
            {
                chasePhase.EndChase();
            }
        }
    }

    [PunRPC]
    void RPC_PlayerEnteredEndZone(int playerId)
    {
        if (!playersWhoEntered.Contains(playerId))
        {
            playersWhoEntered.Add(playerId);
            Debug.Log($"Player {playerId} entered end zone.");

            if (playersWhoEntered.Count == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                chasePhase.EndChase();
            }
        }
    }
}
