using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;

public class CollapseBridge : MonoBehaviourPunCallbacks
{
    public GameObject bridge;
    public GameObject brokenBridge;

    private HashSet<int> playersWhoCrossed = new HashSet<int>();

    public bool breakOnLocal = true;

    public BeginChaseEvent beginChaseEvent = null;

    public AudioSource breakingAudio;

    void Start()
    {
        bridge.SetActive(true);
        brokenBridge.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonView playerPhotonView = other.GetComponent<PhotonView>();

                photonView.RPC("RPC_PlayerCrossedBridge", RpcTarget.MasterClient, playerPhotonView.OwnerActorNr);
            }
            else
            {
                if (breakOnLocal)
                {
                    RPC_BreakBridge();
                }
            }
        }
    }

    [PunRPC]
    void RPC_PlayerCrossedBridge(int playerId)
    {
        if (!playersWhoCrossed.Contains(playerId))
        {
            playersWhoCrossed.Add(playerId);
            Debug.Log($"Player {playerId} crossed the bridge.");

            // Vérifie si tous les joueurs ont traversé
            if (playersWhoCrossed.Count == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                photonView.RPC("RPC_BreakBridge", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void RPC_BreakBridge()
    {
        bridge.SetActive(false);
        brokenBridge.SetActive(true);
        Debug.Log("Bridge collapsed!");

        breakingAudio.Play();

        StartCoroutine(StartChase());
    }

    IEnumerator StartChase()
    {
        yield return new WaitForSeconds(3f);

        if (beginChaseEvent != null)
        {
            beginChaseEvent.OnEventStarted();
        }
    }
}

