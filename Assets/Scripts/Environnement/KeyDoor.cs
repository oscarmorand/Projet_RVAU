using UnityEngine;
using Photon.Pun;
using System.Collections;

public class KeyDoor : MonoBehaviourPun
{
    public GameObject closedDoor;
    public GameObject openDoor;

    public AudioSource unlockSound;
    public AudioSource openDoorSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_OpenDoor", RpcTarget.All);
        }
        else
        {
            RPC_OpenDoor();
        }
    }

    [PunRPC]
    void RPC_OpenDoor()
    {
        unlockSound.Play();
        closedDoor.SetActive(false);
        openDoor.SetActive(true);
    }

    IEnumerator PlayOpenSound()
    {
        yield return new WaitForSeconds(1f);
        openDoorSound.Play();
    }
}
