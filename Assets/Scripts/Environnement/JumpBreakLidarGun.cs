using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;

public class JumpBreakLidarGun : MonoBehaviourPun
{
    private HashSet<int> playersWhoJumped = new HashSet<int>();

    GameObject lastPlayer;

    public GameObject brokenCameraUI;

    public GameObject breakSoundPoint;

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
        if (other.CompareTag("Player"))
        {
            lastPlayer = other.gameObject;
            if (PhotonNetwork.IsConnected)
            {
                PhotonView playerPhotonView = other.GetComponent<PhotonView>();

                photonView.RPC("RPC_PlayerJumped", RpcTarget.MasterClient, playerPhotonView.OwnerActorNr);
            }
            else
            {
                RPC_BreakLidarGun();
            }
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
                photonView.RPC("RPC_BreakLidarGun", PhotonNetwork.CurrentRoom.GetPlayer(playerId));
            }
        }
    }

    [PunRPC]
    public void RPC_BreakLidarGun()
    {
        StartCoroutine(BreakLidarGun());
    }

    public IEnumerator BreakLidarGun()
    {
        yield return new WaitForSeconds(0.3f);

        Debug.Log("Your lidar gun is broken!");
        brokenCameraUI.SetActive(true);

        lastPlayer.GetComponent<LidarRayCasting>().broken = true;

        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_PlayBreakSound", RpcTarget.All);
        }
        else
        {
            RPC_PlayBreakSound();
        }
    }

    [PunRPC]
    public void RPC_PlayBreakSound()
    {
        breakSoundPoint.GetComponent<AudioSource>().Play();
    }
}
